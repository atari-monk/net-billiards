using Sim.Core;

namespace Pool.Container;

public class GameFactory
	: IOrder<IGameView>
{
	public IGameView Order()
	{
		var pathFactory = new GameDataPath();
		var dependenciesFactory = new DependenciesFactory(pathFactory.Order());
		var dependencies = dependenciesFactory.Order();

		var controlFactory = new ControlsFactory(
			dependencies.ShapeFactory
			, dependencies.SerializerXml
			, dependencies.GameData);

		var control = controlFactory.Order();

		var physicsFactory = new PhysicsFactory(
			dependencies.ShapeFactory
			, dependencies.GameData);

		var physics = physicsFactory.Order();

		var gameContextFactory = new GameContextFactory(
			dependencies.Logger
			, dependencies.ShapeFactory
			, dependencies.GameData
			, physics
			, control.Canvas);

		var gameContext = gameContextFactory.Order();

		var gameLoopFactory = new GameLoopFactory(
			control.Canvas
			, dependencies.GameData
			, gameContext
			, physics
			, dependencies.Logger);

		var gameLoop = gameLoopFactory.Order();

		var gameViewFactory = new GameViewFactory(
			dependencies.Logger
			, control
			, gameContext
			, gameLoop);

		var gameView = gameViewFactory.Order();

		dependencies.Logger.Log("Budowanie komponentów aplikacji zakończone");

		return gameView;
	}
}