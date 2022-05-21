using Sim.Core;

namespace Pool.Control;

public class GameViewModel
	: RescaleViewModel
{
	public PlayerViewModel Player1Context { get; set; }

	public PlayerViewModel Player2Context { get; set; }

	public StatsViewModel GameContext { get; set; }

	public void SetPlayer1Stats(PlayerStats playerStats)
	{
		Player1Context.Update(playerStats);
	}

	public void SetPlayer2Stats(PlayerStats playerStats)
	{
		Player2Context.Update(playerStats);
	}

	public void SetGameStats(GameStats gameStats)
	{
		GameContext.Update(gameStats);
	}

	public GameViewModel(IConfigProvider configProvider) : base(configProvider)
	{
		Player1Context = new PlayerViewModel() { Name = "Player1" };
		Player2Context = new PlayerViewModel() { Name = "Player2" };
		GameContext = new StatsViewModel();
	}

	public override void DefaultScale()
	{
		XScale = (double)Width / 1920;
		YScale = (double)(Height - 100) / 1080;
	}
}