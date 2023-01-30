using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";

}).AddCookie("Cookies", options => { options.AccessDeniedPath = "/Home/AccessDenied"; }).AddOpenIdConnect("oidc", opts =>
{
    opts.SignInScheme = "Cookies";
    opts.Authority = "https://localhost:7139";
    opts.ClientId = "Client2-Mvc";
    opts.ClientSecret = "secret";
    opts.ResponseType = "code id_token";
    opts.GetClaimsFromUserInfoEndpoint = true;
    opts.SaveTokens = true;
    opts.Scope.Add("api2.read");
    opts.Scope.Add("api1.read");
    opts.Scope.Add("offline_access");

    opts.Scope.Add("CountryAndCity");
    opts.ClaimActions.MapUniqueJsonKey("country", "country");
    opts.ClaimActions.MapUniqueJsonKey("city", "city");

    opts.Scope.Add("Roles");
    opts.ClaimActions.MapUniqueJsonKey("role", "role");
    opts.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        RoleClaimType = "role"
    };
});
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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
