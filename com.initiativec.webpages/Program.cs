using com.cardano;
using com.database;
using com.initiativec.webpages;
using com.initiativec.webpages.Database;
using com.initiativec.webpages.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

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

builder.Services.AddSingleton<BlockfrostService>();

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

// Registrar o CultureMiddleware antes do middleware de localiza��o


app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();


//PODE DAR ERRO
app.MapGet("/static/bounty", async context =>
{
    var handler = new BountyModel(
        context.RequestServices.GetRequiredService<IOptions<RequestLocalizationOptions>>()
    );

    var result = handler.OnGetBounty();
    var json = System.Text.Json.JsonSerializer.Serialize(result.Value);
    context.Response.ContentType = "application/json";
    await context.Response.WriteAsync(json);
});

app.Run();
