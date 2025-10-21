//added CommunityToolkit
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Storage;

//added own Services
using MBS_Gatewaykonfigurator.Services;
using Microsoft.Extensions.Logging;
//added MudBlazor
using MudBlazor.Services;

namespace MBS_Gatewaykonfigurator
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit() //CommunityToolkit
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
            //test
#endif
            //added MudBalzor
            builder.Services.AddMudServices();

            //added own Services
            builder.Services.AddSingleton<ProjektService>();
            builder.Services.AddSingleton<GatewayService>();
            builder.Services.AddSingleton<GerätevorlageService>();

            // Register the FolderPicker as a singleton
            builder.Services.AddSingleton<IFolderPicker>(FolderPicker.Default);
            builder.Services.AddTransient<MainPage>();


            return builder.Build();
        }
    }
}
