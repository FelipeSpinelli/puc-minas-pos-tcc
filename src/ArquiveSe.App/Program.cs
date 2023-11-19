using ArquiveSe.App.Services;
using ArquiveSe.App.Services.Abstractions;
using RestEase.HttpClientFactory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services
    .AddRestEaseClient<IArquiveSeApi>(builder.Configuration.GetValue<string>("ApiBaseAddress"))
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

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
