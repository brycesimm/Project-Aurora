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

				// Static Nunito Fonts for reliable weight rendering
				fonts.AddFont("Nunito-Regular.ttf", "Nunito");
				fonts.AddFont("Nunito-SemiBold.ttf", "NunitoSemiBold");
				fonts.AddFont("Nunito-Bold.ttf", "NunitoBold");
				fonts.AddFont("Nunito-ExtraBold.ttf", "NunitoExtraBold");
				fonts.AddFont("Nunito-Black.ttf", "NunitoBlack");
			});

		// Load configuration files (Production base + Development overrides)
		var assembly = Assembly.GetExecutingAssembly();
		var configBuilder = new ConfigurationBuilder();

		// Load base production configuration
		using var stream = assembly.GetManifestResourceStream("Aurora.appsettings.json");
		if (stream != null)
		{
			configBuilder.AddJsonStream(stream);
		}

		// Load development overrides in DEBUG builds
#if DEBUG
		using var devStream = assembly.GetManifestResourceStream("Aurora.appsettings.Development.json");
		if (devStream != null)
		{
			configBuilder.AddJsonStream(devStream);
		}
#endif

		var config = configBuilder.Build();
		builder.Configuration.AddConfiguration(config);

		builder.Services.AddHttpClient<IContentService, ContentService>(client =>
		{
			var baseUrl = builder.Configuration["ApiSettings:BaseUrl"];

			// Android Network Configuration
			if (DeviceInfo.Platform == DevicePlatform.Android && baseUrl != null && baseUrl.Contains("localhost", StringComparison.OrdinalIgnoreCase))
			{
				if (DeviceInfo.DeviceType == DeviceType.Virtual)
				{
					// Android Emulator
					baseUrl = baseUrl.Replace("localhost", "10.0.2.2", StringComparison.OrdinalIgnoreCase);
				}
				else
				{
					// Physical Device - Use "LocalOverrideIp" from appsettings if available,
					// otherwise fallback to localhost (which will fail on physical devices without a tunnel).
					var overrideIp = builder.Configuration["ApiSettings:LocalOverrideIp"];
					if (!string.IsNullOrEmpty(overrideIp))
					{
						baseUrl = baseUrl.Replace("localhost", overrideIp, StringComparison.OrdinalIgnoreCase);
					}
				}
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
