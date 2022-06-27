using Sim.Core;

namespace Pool.Control;

public class StatsViewModel
	: ViewModel
{
	private string? fps;
	private string? time;

	public string? Fps
	{
		get => fps;
		set
		{
			fps = value;
			OnPropertyChanged(nameof(Fps));
		}
	}

	public string? Time
	{
		get => time;
		set
		{
			time = value;
			OnPropertyChanged(nameof(Time));
		}
	}

	public void Update(GameStats gameStats)
	{
		if (gameStats.Fps != null)
			Fps = gameStats.Fps;
		if (gameStats.Time != null)
			Time = gameStats.Time;
	}
}