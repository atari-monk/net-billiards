using Pool.Engine;
using Pool.Logic;
using Sim.Core;

namespace Pool.Container;

public class GameLoopFactory
	: IOrder<IGameLoop>
{
	private readonly ICanvasSerializaton<IShape> _canvasSerializaton;
	private readonly IGameData _gameData;
	private readonly IBilliardGameMasterContext _billiardGameMasterContext;
	private readonly IPhysics _physics;
	private readonly ILoggerToMemory _logger;

	public GameLoopFactory(
		ICanvasSerializaton<IShape> canvasSerializaton
		, IGameData gameData
		, IBilliardGameMasterContext billiardGameMasterContext
		, IPhysics physics
		, ILoggerToMemory logger)
	{
		_canvasSerializaton = canvasSerializaton;
		_gameData = gameData;
		_billiardGameMasterContext = billiardGameMasterContext;
		_physics = physics;
		_logger = logger;
	}

	public IGameLoop Order()
	{
		var sink = new CircleInSink();
		var objectsNotMoving = new ObjectsNotMoving();

		sink.Scored += _billiardGameMasterContext.ScoreBilliardBall;
		objectsNotMoving.IsItMovingEvent += _billiardGameMasterContext.IsMovementOnBilliardTable;
		objectsNotMoving.SignalNoMovementEvent += _billiardGameMasterContext.SetNoMovementState;

		var binaryLogics = new List<IBinaryLogic>
		{
			sink
		};

		var shapeLogic = new List<IShapesLogic>
		{
			objectsNotMoving
		};

		var logicContainer = new LogicEngine(_gameData, binaryLogics, shapeLogic);

		var gameLoop = new ShapeGameLoop(
			_gameData,
			_canvasSerializaton,
			_billiardGameMasterContext,
			_physics,
			logicContainer
			, new Timer()
			, _logger);

		return gameLoop;
	}
}