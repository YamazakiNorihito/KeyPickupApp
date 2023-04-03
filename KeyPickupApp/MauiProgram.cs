using CommunityToolkit.Maui;
using KeyPickupApp.Services;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace KeyPickupApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{

        var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
        var config = new ConfigurationBuilder().AddJsonFile("KeyPickupApp.appsettings.json").Build();
        builder.Configuration.AddConfiguration(config);

        builder.Services.AddHttpClient<IReceivingSytemService, ReceivingSytemService>(sp => sp.BaseAddress = new Uri("https://toyoko.eoraa.com/"));
        return builder.Build();
	}
}
