using ApplicationCore;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews()
//.AddViewOptions(options => options.HtmlHelperOptions.ClientValidationEnabled = false)
.AddRazorOptions(options =>
{
    options.ViewLocationFormats.Add("/Views/Parking/{0}.cshtml");

});

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.RegisterInfrastructureService(connectionString);

builder.Services.RegisterCoreService();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
