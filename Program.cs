using ClinicAppointment.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;


var basePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "ClinicAppointment");
var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var cultureInfo = new CultureInfo("en-US");
    cultureInfo.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
    cultureInfo.DateTimeFormat.LongTimePattern = "HH:mm";

    options.DefaultRequestCulture = new RequestCulture(cultureInfo);
    options.SupportedCultures = new List<CultureInfo> { cultureInfo };
    options.SupportedUICultures = new List<CultureInfo> { cultureInfo };
});
builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSession();
builder.Services.AddHttpClient();


var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}




app.UseHttpsRedirection();
app.UseStaticFiles();         // Add this if missing
app.UseRouting();
app.UseSession();             // Session must be after routing, before RazorPages
app.UseAuthorization();
app.MapControllers();
app.MapRazorPages();          // This must be last in the middleware chain
app.UseDeveloperExceptionPage();

app.Run();