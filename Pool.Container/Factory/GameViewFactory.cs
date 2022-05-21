using Pool.Control;
using Sim.Core;

namespace Pool.Container;

public class GameViewFactory
	: IOrder<IGameView>
{
	private readonly ILoggerToMemory _logger;
	private readonly ICanvasVisualControl<IShape> _control;
	private readonly IBilliardGameMasterContext _billiardGameMasterContext;
	private readonly IGameLoop _gameLoop;

	public GameViewFactory(
		ILoggerToMemory logger
		, ICanvasVisualControl<IShape> control
		, IBilliardGameMasterContext billiardGameMasterContext
		, IGameLoop gameLoop)
	{
		_logger = logger;
		_control = control;
		_billiardGameMasterContext = billiardGameMasterContext;
		_gameLoop = gameLoop;
	}

	public IGameView Order()
	{
		var configProvider = new ConfigProvider();

		var gameVm = new GameViewModel(configProvider);

		var gameView = new GameView(
			_gameLoop
			, _control
			, _logger
			, gameVm);

		_billiardGameMasterContext.SetPlayer1StatsEvent += gameVm.SetPlayer1Stats;
		_billiardGameMasterContext.SetPlayer2StatsEvent += gameVm.SetPlayer2Stats;
		_gameLoop.SetLabelEvent += gameVm.SetGameStats;

		return gameView;
	}
}