using Sim.Core;

namespace Pool.Control;

public class PlayerViewModel
	: ViewModel
{
	private string name;
	private string state;
	private string color;
	private string faul;
	private string score;

	public string Name
	{
		get => name; 
		set
		{
			name = value;
			OnPropertyChanged(nameof(Name));
		}
	}

	public string State
	{
		get => state; 
		set
		{
			state = value;
			OnPropertyChanged(nameof(State));
		}
	}

	public string Color
	{
		get => color;
		set
		{
			color = value;
			OnPropertyChanged(nameof(Color));
		}
	}

	public string Faul
	{
		get => faul;
		set
		{
			faul = value;
			OnPropertyChanged(nameof(Faul));
		}
	}

	public string Score
	{
		get => score;
		set
		{
			score = value;
			OnPropertyChanged(nameof(Score));
		}
	}

	public void Update(PlayerStats playerStats)
	{
		if(playerStats.Name != null)
			Name = playerStats.Name;
		if (playerStats.State != null)
			State = playerStats.State;
		if (playerStats.Color != null)
			Color = playerStats.Color;
		if (playerStats.Faul != null)
			Faul = playerStats.Faul;
		if (playerStats.Score != null)
			Score = playerStats.Score;
	}
}