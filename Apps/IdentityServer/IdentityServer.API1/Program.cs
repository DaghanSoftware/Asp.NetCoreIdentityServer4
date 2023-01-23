using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opts => {
        opts.Authority = "https://localhost:7139";
        opts.Audience = "resource_api1";
        });
builder.Services.AddAuthorization(options => { 

        options.AddPolicy("ReadProduct", policy =>{
            policy.RequireClaim("scope", "api1.read");
        });

        options.AddPolicy("UpdateOrCreate", policy => {
            policy.RequireClaim("scope", new[] {"api1.update","api.write"});
        });

});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
