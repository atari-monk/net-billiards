using Sim.Core;
using Unity;

namespace Pool.Container;

public class GameContainer
	: IOrder<IGameView>
{
	private readonly IUnityContainer container;

	public GameContainer(IUnityContainer container)
	{
		this.container = container;
	}

	public IGameView Order()
	{
		container.RegisterInstance(container);

		container.RegisterType<IRegister, DependenciesContainer>(
			nameof(DependenciesContainer)
			, TypeLifetime.Singleton);
		container.RegisterType<IRegister, ControlsContainer>(
			nameof(ControlsContainer)
			, TypeLifetime.Singleton);
		container.RegisterType<IRegister, PhysicsContainer>(
			nameof(PhysicsContainer)
			, TypeLifetime.Singleton);
		container.RegisterType<IRegister, GameContextContainer>(
			nameof(GameContextContainer)
			, TypeLifetime.Singleton);
		container.RegisterType<IRegister, GameLoopContainer>(
			nameof(GameLoopContainer)
			, TypeLifetime.Singleton);
		container.RegisterType<IRegister, GameViewContainer>(
			nameof(GameViewContainer)
			, TypeLifetime.Singleton);

		var containers = container.Resolve<IRegister[]>();
		foreach (var container in containers)
		{
			container.Register();
			container.Initialize();
		}

		var gameView = container.Resolve<IGameView>();

		var logger = container.Resolve<ILoggerToMemory>();
		logger.Log("Budowanie komponentów aplikacji zakończone");

		return gameView;
	}
}