using System.Windows;
using Pool.Container;
using Sim.Core;
using Unity;

namespace Pool.App;

public partial class App
	: Application
{
	private void ApplicationStartup(
        object sender
        , StartupEventArgs e)
	{
		BuildByContainer();
		//BuildByFactory();
	}

	private void BuildByFactory()
    {
        SetAndShowGameWindow(
            new GameFactory().Order());
    }

    private void SetAndShowGameWindow(
        IGameView gameView)
    {
        MainWindow = gameView as Window;
        ArgumentNullException.ThrowIfNull(MainWindow);
        MainWindow.Show();
    }

    private void BuildByContainer()
	{
        SetAndShowGameWindow(
            new GameContainer(
                new UnityContainer()).Order());
	}
}