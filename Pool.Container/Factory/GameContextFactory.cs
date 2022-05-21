using Pool.Logic;
using Shape.Model;
using Sim.Core;

namespace Pool.Container;

public class GameContextFactory
	: IOrder<IBilliardGameMasterContext>
{
	private readonly ILoggerToMemory _logger;
	private readonly IShapeFactory _shapeFactory;
	private readonly IGameData _gameData;
	private readonly IPhysics _physics;
	private readonly ICanvasSerializaton<IShape> _canvasSerializaton;

	public GameContextFactory(
		ILoggerToMemory logger
		, IShapeFactory shapeFactory
		, IGameData gameData
		, IPhysics physics
		, ICanvasSerializaton<IShape> canvasSerializaton)
	{
		_logger = logger;
		_shapeFactory = shapeFactory;
		_gameData = gameData;
		_physics = physics;
		_canvasSerializaton = canvasSerializaton;
	}

	public IBilliardGameMasterContext Order()
	{
		var billiardPlayerManager = new BilliardPlayerManager(
			new IPlayer[]
			{
				new EightBallPlayer{ Name = "player1" }
				, new EightBallPlayer{ Name = "player2" }
			});

		var billiardGameMasterContext = new BilliardGameMasterContext(
			_gameData,
			_physics,
			_canvasSerializaton,
			billiardPlayerManager,
			new MovementFactory(),
			new GameStateFactory(),
			_shapeFactory,
			new FaulStateFactory()
			, _logger);

		billiardGameMasterContext.Fauls = new List<IFaulState>
			{
				new WhiteScored(billiardGameMasterContext),
				new BlackScoredInBreak(billiardGameMasterContext),
				new BlackScoredNotLast(billiardGameMasterContext),
				new BlackScoredLast(billiardGameMasterContext)
			};

		return billiardGameMasterContext;
	}
}