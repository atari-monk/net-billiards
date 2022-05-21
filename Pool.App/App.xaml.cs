using System.Windows;
using Pool.Container;
using Unity;

namespace Pool.App;

public partial class App
	: Application
{
	private void ApplicationStartup(object sender, StartupEventArgs e)
	{
		BuildByContainer();
		//BuildByFactory();
	}

	private void BuildByFactory()
	{
		var gameFactory = new GameFactory();
		var gameView = gameFactory.Order();
		MainWindow = gameView as Window;
		MainWindow.Show();
	}

	private void BuildByContainer()
	{
		var container = new UnityContainer();
		var gameContainer = new GameContainer(container);
		var gameView = gameContainer.Order();
		MainWindow = gameView as Window;
		MainWindow.Show();
	}
}