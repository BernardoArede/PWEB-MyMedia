using MyMedia.Web.Components;
using MyMedia.RCL.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

string devTunnelUrl = "https://qbbm94g5-7092.usw3.devtunnels.ms/swagger/index.html";

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(devTunnelUrl) });
builder.Services.AddScoped<MyMediaService>();
builder.Services.AddScoped<MyMedia.RCL.Services.CartService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
