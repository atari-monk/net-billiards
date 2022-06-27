using System.Windows;
using System.Windows.Controls;
using Sim.Core;

namespace Pool.Control;

public partial class GameView
	: Window
		, IGameView
{
	private readonly IGameLoop game;
	private readonly ILoggerToMemory logger;

	public ICanvasVisualControl<IShape> CanvasVisualControl { get; }

	public GameView(
		IGameLoop game
		, ICanvasVisualControl<IShape> canvasVisualControl
		, ILoggerToMemory logger
		, GameViewModel gameViewModel)
	{
		this.game = game;
		CanvasVisualControl = canvasVisualControl;
		this.logger = logger;
		InitializeComponent();
		var uiElements = CanvasVisualControl as UIElement;
		RootLayout.Children.Add(uiElements);
		uiElements?.SetValue(Grid.RowProperty, 1);
        Wallboard.GameWallboard.CreateGameComponents(this.logger.LogContent);
		DataContext = gameViewModel; 
	}

	private void MenuItemClick(object sender, RoutedEventArgs e)
	{
		var thread = new Thread(RunGameLoop)
		{
			IsBackground = true
		};
		thread.Start();
	}

	private void RunGameLoop() => game.RunGameLoop();

	private void WindowSizeChanged(object sender, SizeChangedEventArgs e)
	{
		var vm = (RescaleViewModel)DataContext;
		if (WindowState == WindowState.Maximized)
		{
			vm.MaximaziedScale();
		}
		else
		{
			vm.DefaultScale();
		}
	}
}