using Try1RASP.Views;
using Try1RASP;
namespace Try1RASP;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage=new AppShell();
	}
}
