using Blockfrost.Api;
using Blockfrost.Api.Services;
using CardanoSharp.Blockfrost.Sdk.Common;
using com.cardano;
using com.database;
using com.initiativec.webpages;
using com.initiativec.webpages.Interfaces;
using com.initiativec.webpages.Services;
using com.initiativec.webpages.ViewModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.Configure<TwitterSettings>(builder.Configuration.GetSection("Twitter"));

// Registrar as configurações do Telegram Bot
builder.Services.Configure<TelegramBotSettings>(builder.Configuration.GetSection("TelegramBot"));

// Registrar o cliente do Telegram.Bot
builder.Services.AddSingleton<ITelegramBotClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<TelegramBotSettings>>().Value;
    return new TelegramBotClient(settings.Token);
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

builder.Services.AddMvc().AddViewLocalization().AddDataAnnotationsLocalization();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<SharedResourceService>();


var apiKey = builder.Configuration["Blockfrost:Network:ApiKey"];
var baseUrl = builder.Configuration["Blockfrost:Network:BaseUrl"];
var authConfig = new AuthHeaderConfiguration(apiKey, baseUrl);

builder.Services.AddBlockfrost(authConfig);

builder.Services.AddSingleton<TwitterService>();
builder.Services.AddScoped<TelegramService>();
builder.Services.AddSingleton<BotService>();
builder.Services.AddHostedService<BotHostedService>();


builder.Services.AddScoped<BlockfrostServices>();

builder.Services.AddTransient<IEmailSender, SendGridEmailSender>();
builder.Services.AddTransient<TokenBoutyService>();

var app = builder.Build();
app.UseMiddleware<CultureMiddleware>();


//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;

//    var localizationOptions = services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;
//    app.UseRequestLocalization(localizationOptions);
//}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

// Registrar o CultureMiddleware antes do middleware de localização


app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

app.UseAuthorization();

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
