using ArquiveSe.App.Services;
using ArquiveSe.App.Services.Abstractions;
using Auth0.AspNetCore.Authentication;
using RestEase.HttpClientFactory;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuth0WebAppAuthentication(options => {
     options.Domain = builder.Configuration["Auth0:Domain"];
     options.ClientId = builder.Configuration["Auth0:ClientId"];
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
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services
    .AddRestEaseClient<IArquiveSeApi>(builder.Configuration["ApiBaseAddress"])
    .SetHandlerLifetime(TimeSpan.FromMinutes(2));
builder.Services.AddSingleton<IArquiveSeApiService, ArquiveSeApiService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
