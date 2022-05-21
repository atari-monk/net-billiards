using System.Windows;
using System.Windows.Controls;
using Sim.Core;

namespace Pool.Control;

public partial class GameView
	: Window
		, IGameView
{
	private readonly IGameLoop _game;
	private readonly ILoggerToMemory _logger;

	public ICanvasVisualControl<IShape> CanvasVisualControl { get; }

	public GameView(
		IGameLoop game
		, ICanvasVisualControl<IShape> canvasVisualControl
		, ILoggerToMemory logger
		, GameViewModel gameViewModel)
	{
		_game = game;
		CanvasVisualControl = canvasVisualControl;
		_logger = logger;
		InitializeComponent();
		var uiElements = CanvasVisualControl as UIElement;
		RootLayout.Children.Add(uiElements);
		uiElements.SetValue(Grid.RowProperty, 1);
		Wallboard.GameWallboard.CreateGameComponents(_logger.LogContent);
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

	private void RunGameLoop() => _game.RunGameLoop();

	//public void SetLabel(Labels labelKey, string text)
	//{
	//	Wallboard.GameWallboard.SetLabel(labelKey, text);
	//}

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