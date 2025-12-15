using Microsoft.Extensions.Logging;
using MyMedia.RCL.Services;

namespace MyMedia.MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

            string devTunnelUrl = "https://qbbm94g5-7092.usw3.devtunnels.ms/";

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(devTunnelUrl) });
            builder.Services.AddScoped<MyMediaService>();
            builder.Services.AddScoped<MyMedia.RCL.Services.CartService>();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
