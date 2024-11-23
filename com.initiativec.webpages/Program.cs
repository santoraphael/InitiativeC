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
//var vaultUrl = "http://cloud.weela.co:8200/"; // Substitua pela URL do seu Vault
//var token = "hvs.IAdrk1fNxKTAaKT4UrkPHvlQ"; // Substitua pelo seu token de autenticação

//// Configuração do cliente do Vault
//IAuthMethodInfo authMethod = new TokenAuthMethodInfo(token);
//var vaultClientSettings = new VaultClientSettings(vaultUrl, authMethod);
//IVaultClient vaultClient = new VaultClient(vaultClientSettings);

//var healthCheck = await vaultClient.V1.System.GetHealthStatusAsync();

//// Lendo segredos do Vault
//// Lendo segredos do Vault (KV v2)
//var mountPoint = ""; // Caminho onde o KV está montado
//var secretPath = ""; // Nome do seu segredo


//if (builder.Environment.IsDevelopment())
//{
//    mountPoint = "secret";
//    secretPath = "appsettings";
//}
//else if (builder.Environment.IsProduction())
//{
//    mountPoint = "secret";
//    secretPath = "appsettings";
//}


//try
//{
//    var secret = await vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(path: secretPath, mountPoint: mountPoint);
//    // Adiciona as configurações do Vault ao Configuration
//    foreach (var entry in secret.Data.Data)
//    {
//        builder.Configuration[entry.Key] = entry.Value?.ToString();
//    }
//}
//catch (VaultApiException ex)
//{
//    var teste = ex;
//}


builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.Configure<EmailApiSettings>(builder.Configuration.GetSection("EmailApiSettings"));
builder.Services.AddHttpClient<EmailService>();


builder.Services.AddDbContext<DatabaseContext>(options => options.UseNpgsql("Host=193.203.165.26;Port=5432;Database=LehmonBrothersBD;Username=PostgresAdmin;Password=R330p908l548e13s224;"));


builder.Services.Configure<RequestLocalizationOptions>(
    options =>
    {
        var supportedCultures = new[] { "en-US", "zh-CN", "ar-SA", "es-ES", "fr-FR", "pt-BR", "ru-RU", "ja-JP" };
        options.SetDefaultCulture(supportedCultures[0])
               .AddSupportedCultures(supportedCultures)
               .AddSupportedUICultures(supportedCultures);
    }
);

builder.Services.AddMvc().AddViewLocalization().AddDataAnnotationsLocalization();

// Remover a segunda instância de IHttpContextAccessor
// builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<SharedResourceService>();

var apiKey = "mainnet9Zmdv75dfVyFF0DzGoLVbQXoqX16yaOpmainnet9Zmdv75dfVyFF0DzGoLVbQXoqX16yaOp";// builder.Configuration["Blockfrost/Network/ApiKey"];
var baseUrl = "https://cardano-mainnet.blockfrost.io/api/v0https://cardano-mainnet.blockfrost.io/api/v0";// builder.Configuration["Blockfrost/Network/BaseUrl"];
var authConfig = new AuthHeaderConfiguration(apiKey, baseUrl);

builder.Services.AddBlockfrost(authConfig);

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
