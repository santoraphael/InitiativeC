using Blockfrost.Api;
using Blockfrost.Api.Services;
using CardanoSharp.Blockfrost.Sdk.Common;
using com.cardano;
using com.database;
using com.initiativec.webpages;
using com.initiativec.webpages.Interfaces;
using com.initiativec.webpages.Services;
using com.initiativec.webpages.ViewModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Telegram.Bot;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VaultSharp;
using VaultSharp.V1;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.SecretsEngines;
using VaultSharp.V1.AuthMethods.Token;
using VaultSharp.V1.SystemBackend;
using VaultSharp.Core;
using VaultSharp.V1.Commons;

var builder = WebApplication.CreateBuilder(args);



// Configuração do Vault
var vaultUrl = "http://cloud.weela.co:8200/"; // Substitua pela URL do seu Vault
var token = "hvs.IAdrk1fNxKTAaKT4UrkPHvlQ"; // Substitua pelo seu token de autenticação

// Configuração do cliente do Vault
IAuthMethodInfo authMethod = new TokenAuthMethodInfo(token);
var vaultClientSettings = new VaultClientSettings(vaultUrl, authMethod);
IVaultClient vaultClient = new VaultClient(vaultClientSettings);

var healthCheck = await vaultClient.V1.System.GetHealthStatusAsync();

// Lendo segredos do Vault
// Lendo segredos do Vault (KV v2)
var mountPoint = ""; // Caminho onde o KV está montado
var secretPath = ""; // Nome do seu segredo


if (builder.Environment.IsDevelopment())
{
    mountPoint = "secret";
    secretPath = "appsettings";
}
else if (builder.Environment.IsProduction())
{
    mountPoint = "secret";
    secretPath = "appsettings";
}


try
{
    var secret = await vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(path: secretPath, mountPoint: mountPoint);
    // Adiciona as configurações do Vault ao Configuration
    foreach (var entry in secret.Data.Data)
    {
        builder.Configuration[entry.Key] = entry.Value?.ToString();
    }
}
catch (VaultApiException ex)
{
    var teste = ex;
}


builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.Configure<EmailApiSettings>(builder.Configuration.GetSection("EmailApiSettings"));
builder.Services.AddHttpClient<EmailService>();


builder.Services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(builder.Configuration["ConnectionStrings"]));
builder.Services.Configure<TwitterSettings>(builder.Configuration.GetSection("Twitter"));

// Registrar as configurações do Telegram Bot

// Registrar o cliente do Telegram.Bot
builder.Services.AddSingleton<ITelegramBotClient>(sp =>
{
    return new TelegramBotClient(builder.Configuration["TelegramBot/Token"]);
});

builder.Services.Configure<RequestLocalizationOptions>(
    options =>
    {
        var supportedCultures = new[] { "en-US", "zh-CN", "ar-SA", "es-ES", "fr-FR", "pt-BR", "ru-RU", "ja-JP" };
        options.SetDefaultCulture(supportedCultures[0])
               .AddSupportedCultures(supportedCultures)
               .AddSupportedUICultures(supportedCultures);
    }
);

// Configura a autenticação
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = "Discord"; // Define Discord como esquema padrão para desafios
})
.AddCookie()
.AddOAuth("Discord", options =>
{
    options.ClientId = "1288955489224233124";//builder.Configuration["Discord:ClientId"];
    options.ClientSecret = "gcYaZV93s8cpPRJQ-_TZfTkTOJs5y4fo"; //builder.Configuration["Discord:ClientSecret"];
    options.CallbackPath = new PathString("/signin-discord"); // Deve corresponder à Redirect URI registrada

    options.AuthorizationEndpoint = "https://discord.com/api/oauth2/authorize";
    options.TokenEndpoint = "https://discord.com/api/oauth2/token";
    options.UserInformationEndpoint = "https://discord.com/api/users/@me";

    options.Scope.Add("identify");

    options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
    options.ClaimActions.MapJsonKey(ClaimTypes.Name, "username");
    options.ClaimActions.MapJsonKey("urn:discord:discriminator", "discriminator");
    options.ClaimActions.MapJsonKey("urn:discord:avatar", "avatar");

    options.SaveTokens = true;

    options.Events.OnCreatingTicket = async context =>
    {
        var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", context.AccessToken);
        request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

        var response = await context.Backchannel.SendAsync(request, System.Net.Http.HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
        response.EnsureSuccessStatusCode();

        var user = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

        context.RunClaimActions(user.RootElement);
    };
});

// Configuração de Cookies
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});

// Remover registros duplicados e adicionar serviços adicionais
builder.Services.AddSingleton<DiscordBotService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<DiscordBotService>());
builder.Services.AddSingleton<IDiscordService, DiscordService>();

builder.Services.AddMvc().AddViewLocalization().AddDataAnnotationsLocalization();

// Remover a segunda instância de IHttpContextAccessor
// builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<SharedResourceService>();

var apiKey = builder.Configuration["Blockfrost/Network/ApiKey"];
var baseUrl = builder.Configuration["Blockfrost/Network/BaseUrl"];
var authConfig = new AuthHeaderConfiguration(apiKey, baseUrl);

builder.Services.AddBlockfrost(authConfig);

builder.Services.AddSingleton<TwitterService>();
builder.Services.AddScoped<TelegramService>();
builder.Services.AddSingleton<BotService>();
builder.Services.AddHostedService<BotHostedService>();

// Já foi adicionado anteriormente
// builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<BlockfrostServices>();

builder.Services.AddTransient<IEmailSender, SendGridEmailSender>();
builder.Services.AddTransient<TokenBoutyService>();

var app = builder.Build();

// Configura o pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

// 1. Configurar a localização
app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

// 2. Servir arquivos estáticos
app.UseStaticFiles();

// 3. Roteamento
app.UseRouting();

// 4. Autenticação
app.UseAuthentication();

// 5. Autorização
app.UseAuthorization();

// 6. Middleware de Cultura (opcional)
app.UseMiddleware<CultureMiddleware>();

// 7. Mapear endpoints
app.MapRazorPages();
app.MapControllers();




//PODE DAR ERRO
app.MapGet("/static/bounty", async context =>
{
    var handler = new BountyModel(
        context.RequestServices.GetRequiredService<DatabaseContext>(),
        context.RequestServices.GetRequiredService<IOptions<RequestLocalizationOptions>>()
    );

    var result = handler.OnGetBounty();
    var json = System.Text.Json.JsonSerializer.Serialize(result.Value);
    context.Response.ContentType = "application/json";
    await context.Response.WriteAsync(json);
});






app.Run();
