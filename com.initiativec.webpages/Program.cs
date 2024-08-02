using com.initiativec.webpages.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.Configure<RequestLocalizationOptions>(
    options =>
    {
        //var supportedCultures = new List<CultureInfo>
        //{
        //    new CultureInfo("en-US"),
        //    new CultureInfo("pt-BR")
        //};

        //options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
        //options.SupportedCultures = supportedCultures;
        //options.SupportedUICultures = supportedCultures;



        var supportedCultures = new[] { "en-US", "pt-BR" };
        options.SetDefaultCulture(supportedCultures[0])
            .AddSupportedCultures(supportedCultures)
            .AddSupportedUICultures(supportedCultures);



        //options.AddSupportedUICultures("en-US", "pt-BR");
        //options.SetDefaultCulture("en-US");
        //options.FallBackToParentUICultures = true;
        //options.RequestCultureProviders.Clear();
    }
);

builder.Services.AddMvc().AddViewLocalization().AddDataAnnotationsLocalization();

builder.Services.AddSingleton<SharedResourceService>();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var localizationOptions = services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;
    app.UseRequestLocalization(localizationOptions);
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();



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
