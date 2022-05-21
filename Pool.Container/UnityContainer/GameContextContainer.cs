using System.Collections.Generic;
using Pool.Logic;
using Shape.Model;
using Sim.Core;
using Unity;
using Unity.Injection;

namespace Pool.Container;

public class GameContextContainer
	: ContainerBase
{
	public GameContextContainer(IUnityContainer unityContainer)
		: base(unityContainer)
	{
	}

	public override void Register()
	{
		Container.RegisterType<IPlayer, EightBallPlayer>(
			"player1"
			, TypeLifetime.Singleton);

		Container.RegisterType<IPlayer, EightBallPlayer>(
			"player2"
			, TypeLifetime.Singleton);

		Container.RegisterType<IBilliardPlayerManager, BilliardPlayerManager>(
			TypeLifetime.Singleton);

		Container.RegisterType<IMovementFactory, MovementFactory>(
			TypeLifetime.Singleton);

		Container.RegisterType<IGameStateFactory, GameStateFactory>(
			TypeLifetime.Singleton);

		Container.RegisterType<IFaulStateFactory, FaulStateFactory>(
			TypeLifetime.Singleton);

		Container.RegisterType<IBilliardGameMasterContext, BilliardGameMasterContext>(
			TypeLifetime.Singleton
			, new InjectionConstructor(
				Container.Resolve<IGameData>()
				, Container.Resolve<IPhysics>()
				, Container.Resolve<ICanvasSerializaton<IShape>>()
				, Container.Resolve<IBilliardPlayerManager>()
				, Container.Resolve<IMovementFactory>()
				, Container.Resolve<IGameStateFactory>()
				, Container.Resolve<IShapeFactory>()
				, Container.Resolve<IFaulStateFactory>()
				, Container.Resolve<ILoggerToMemory>()));

		Container.RegisterType<IFaulState, WhiteScored>(
			nameof(WhiteScored)
			, TypeLifetime.Singleton
			, new InjectionConstructor(
				Container.Resolve<IBilliardGameMasterContext>()));

		Container.RegisterType<IFaulState, BlackScoredInBreak>(
			nameof(BlackScoredInBreak)
			, TypeLifetime.Singleton
			, new InjectionConstructor(
				Container.Resolve<IBilliardGameMasterContext>()));

		Container.RegisterType<IFaulState, BlackScoredNotLast>(
			nameof(BlackScoredNotLast)
			, TypeLifetime.Singleton
			, new InjectionConstructor(
				Container.Resolve<IBilliardGameMasterContext>()));

		Container.RegisterType<IFaulState, BlackScoredLast>(
			nameof(BlackScoredLast)
			, TypeLifetime.Singleton
			, new InjectionConstructor(
				Container.Resolve<IBilliardGameMasterContext>()));
	}

	public override void Initialize()
	{
		var player1 = Container.Resolve<IPlayer>("player1");
		player1.Name = "player1";

		var player2 = Container.Resolve<IPlayer>("player2");
		player2.Name = "player2";

		var gameContext = Container.Resolve<IBilliardGameMasterContext>();
		gameContext.Fauls = new List<IFaulState>(Container.Resolve<IFaulState[]>());
	}
}