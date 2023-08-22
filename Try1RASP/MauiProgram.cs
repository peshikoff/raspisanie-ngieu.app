
namespace Try1RASP;

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

				//Заголовок первого уроня, 30pt, Цвет AlcoRubin
				fonts.AddFont("Circle-Regular.ttf", "Circle-Regular");

				//Заголовок 2 уровня, 24pt, Цвет AlcoRubin
				//Текст, 14pt, Цвет DarkGrey
				fonts.AddFont("Circle-Extralight.ttf", "Circle-Extralight");

			});
		return builder.Build();
	}

}
