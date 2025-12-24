using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using Aurora.Client.Core.Services;
using Aurora.Shared.Interfaces;

namespace Aurora;

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
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("materialdesignicons-webfont.ttf", "MDI");
				fonts.AddFont("Nunito-VariableFont_wght.ttf", "Nunito");
				fonts.AddFont("Nunito-Italic-VariableFont_wght.ttf", "NunitoItalic");
			});

		var assembly = Assembly.GetExecutingAssembly();
		using var stream = assembly.GetManifestResourceStream("Aurora.appsettings.json");

		if (stream != null)
		{
			var config = new ConfigurationBuilder()
				.AddJsonStream(stream)
				.Build();

			builder.Configuration.AddConfiguration(config);
		}

		builder.Services.AddHttpClient<IContentService, ContentService>(client =>
		{
			var baseUrl = builder.Configuration["ApiSettings:BaseUrl"];

			// Android Emulator localhost fix
			if (DeviceInfo.Platform == DevicePlatform.Android && baseUrl != null && baseUrl.Contains("localhost", StringComparison.OrdinalIgnoreCase))
			{
				baseUrl = baseUrl.Replace("localhost", "10.0.2.2", StringComparison.OrdinalIgnoreCase);
			}

			if (!string.IsNullOrEmpty(baseUrl))
			{
				client.BaseAddress = new Uri(baseUrl);
			}
		});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
