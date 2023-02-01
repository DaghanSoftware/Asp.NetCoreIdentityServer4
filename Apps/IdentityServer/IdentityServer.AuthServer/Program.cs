using IdentityServer.AuthServer;
using IdentityServer.AuthServer.Models;
using IdentityServer.AuthServer.Repository;
using IdentityServer.AuthServer.Services;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<ICustomUserRepository, CustomUserRepository>();
// Add services to the container.
builder.Services.AddDbContext<CustomDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("LocalDb"));
});
var assemblyName = typeof(Program).GetTypeInfo().Assembly.GetName().Name;
builder.Services.AddIdentityServer()
    //AddConfigurationStore fonksiyonu ile �ConfigurationDbContext�in konfig�rasyonu
    .AddConfigurationStore(opts => 
    {
        opts.ConfigureDbContext = c => c.UseSqlServer(builder.Configuration.GetConnectionString("LocalDb"),sqlOpts=>
        sqlOpts.MigrationsAssembly(assemblyName));
    })
    //�AddOperationalStore� fonksiyonu ile ise �PersistedGrantDbContext� konfig�rasyonu
    .AddOperationalStore(opts =>
    {
        opts.ConfigureDbContext = c => c.UseSqlServer(builder.Configuration.GetConnectionString("LocalDb"), sqlOpts =>
        sqlOpts.MigrationsAssembly(assemblyName));
    })
    .AddInMemoryApiResources(Config.GetApiResources())
    .AddInMemoryApiScopes(Config.GetApiScopes())
    .AddInMemoryClients(Config.GetClients())
    .AddInMemoryIdentityResources(Config.GetIdentityResources())
    //.AddTestUsers(Config.GetTestUsers().ToList())
    .AddDeveloperSigningCredential()
    .AddProfileService<CustomProfileService>()
    .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>();

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
app.UseIdentityServer();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
