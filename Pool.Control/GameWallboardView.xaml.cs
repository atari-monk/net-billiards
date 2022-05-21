using Sim.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace Pool.Control;

public partial class GameWallboardView
	: UserControl
{
	private Dictionary<Labels, Label> _labels;

	public GameWallboardView()
	{
		InitializeComponent();
	}

	public void CreateGameComponents(ObservableCollection<string> log)
	{
		_labels = new Dictionary<Labels, Label>
		{
			{ Labels.GameState, Player1StateLabel }
			, { Labels.Faul, Player2FaulLabel }
			, { Labels.Player1Score, Player1ScoreLabel }
			, { Labels.Player2Score, Player2ScoreLabel }
			, { Labels.Fps, FpsLabel }
			, { Labels.Time, TimeLabel }
		};
		LogBox.ItemsSource = log;
	}

	public void SetLabel(Labels labelKey, string text)
	{
		var label = _labels[labelKey];
		label.Dispatcher.BeginInvoke((Action)(() => label.Content = text));
	}
}