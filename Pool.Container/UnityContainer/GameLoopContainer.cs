using Pool.Engine;
using Pool.Logic;
using Sim.Core;
using Unity;
using Unity.Injection;

namespace Pool.Container;

public class GameLoopContainer
	: ContainerBase
{
	public GameLoopContainer(IUnityContainer unityContainer)
		: base(unityContainer)
	{
	}

	public override void Register()
	{
		var circleInSink = Container.Resolve<CircleInSink>();
		Container.RegisterInstance<IBinaryLogic>(circleInSink);
		Container.RegisterInstance<ICircleInSink>(circleInSink);

		var objectsNotMoving = Container.Resolve<ObjectsNotMoving>();
		Container.RegisterInstance<IShapesLogic>(objectsNotMoving);
		Container.RegisterInstance<IObjectsNotMoving>(objectsNotMoving);

		Container.RegisterType<ILogics, LogicEngine>(
			TypeLifetime.Singleton
			, new InjectionConstructor(
				Container.Resolve<IGameData>()
				, Container.Resolve<List<IBinaryLogic>>()
				, Container.Resolve<List<IShapesLogic>>()));

		Container.RegisterType<ITimer, Timer>(
			TypeLifetime.Singleton);

		Container.RegisterType<IGameLoop, ShapeGameLoop>(
			TypeLifetime.Singleton
			, new InjectionConstructor(
				Container.Resolve<IGameData>()
				, Container.Resolve<ICanvasSerializaton<IShape>>()
				, Container.Resolve<IBilliardGameMasterContext>()
				, Container.Resolve<IPhysics>()
				, Container.Resolve<ILogics>()
				, Container.Resolve<ITimer>()
				, Container.Resolve<ILoggerToMemory>()));
	}

	public override void Initialize()
	{
		var sink = Container.Resolve<ICircleInSink>();

		var billiardGameMasterContext = Container.Resolve<IBilliardGameMasterContext>();

		sink.Scored += billiardGameMasterContext.ScoreBilliardBall;

		var objectsNotMoving = Container.Resolve<IObjectsNotMoving>();

		objectsNotMoving.IsItMovingEvent += billiardGameMasterContext.IsMovementOnBilliardTable;
		objectsNotMoving.SignalNoMovementEvent += billiardGameMasterContext.SetNoMovementState;
	}
}