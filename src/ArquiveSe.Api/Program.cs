using ArquiveSe.Infra.Cache.Configurations;
using ArquiveSe.Infra.Databases.Persistence.Configurations;
using ArquiveSe.Infra.Databases.Read.Configurations;
using ArquiveSe.Infra.Messaging.Configurations;
using ArquiveSe.Infra.Storage.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddUseCases();
builder.Services.AddPersistenceDb(builder.Configuration);
builder.Services.AddReadingDb(builder.Configuration);
builder.Services.AddMessaging(builder.Configuration);
builder.Services.AddStorage(builder.Configuration);
builder.Services.AddCache(builder.Configuration);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = builder.Configuration["Auth0:Authority"];
    options.Audience = builder.Configuration["Auth0:Audience"];
    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = ClaimTypes.NameIdentifier
    };
});
builder.Services.AddAuthorization(options =>
{
    new[]
    {
        "documents:read",
        "documents:create",
        "documents:review",
        "documents:approve",
    }
    .ToList()
    .ForEach(claim =>
    {
        options.AddPolicy(claim, authBuilder =>
        {
            authBuilder.RequireClaim(claim);
        });
    });
});

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

app.UseSeeds();
app.UseMessaging();

app.Run();
