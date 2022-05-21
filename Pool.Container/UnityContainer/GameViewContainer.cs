using Pool.Control;
using Sim.Core;
using Unity;

namespace Pool.Container;

public class GameViewContainer
	: ContainerBase
{
	public GameViewContainer(IUnityContainer unityContainer)
        : base(unityContainer)
	{
	}

	public override void Register()
	{
		Container.RegisterType<IConfigProvider, ConfigProvider>(
			TypeLifetime.Singleton);

		Container.RegisterType<GameViewModel>(
			TypeLifetime.Singleton);

		Container.RegisterType<IGameView, GameView>(
			TypeLifetime.Singleton);
	}

	public override void Initialize()
	{
		var billiardGameMasterContext = Container.Resolve<IBilliardGameMasterContext>();

		var gameVm = Container.Resolve<GameViewModel>();

		var gameLoop = Container.Resolve<IGameLoop>();

		billiardGameMasterContext.SetPlayer1StatsEvent += gameVm.SetPlayer1Stats;
		billiardGameMasterContext.SetPlayer2StatsEvent += gameVm.SetPlayer2Stats;
		gameLoop.SetLabelEvent += gameVm.SetGameStats;
	}
}