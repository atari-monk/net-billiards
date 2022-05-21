using Sim.Core;

namespace Pool.Control;

public class RescaleViewModel
	: SizeViewModel
{
	private double _xScale;
	private double _yScale;
	private readonly IConfigProvider _configProvider;

	public double XScale
	{
		get => _xScale;

		set
		{
			_xScale = value;
			OnPropertyChanged(nameof(XScale));
		}
	}

	public double YScale
	{
		get => _yScale;

		set
		{
			_yScale = value;
			OnPropertyChanged(nameof(YScale));
		}
	}

	public RescaleViewModel(IConfigProvider configProvider)
	{
		_configProvider = configProvider;
		Width = int.Parse(_configProvider.ReadSetting(nameof(Width)));
		Height = int.Parse(_configProvider.ReadSetting(nameof(Height)));
		DefaultScale();
	}

	public virtual void DefaultScale()
	{
		XScale = (double)Width / 1920;
		YScale = (double)Height / 1080;
	}

	public void MaximaziedScale()
	{
		XScale = 1;
		YScale = 1;
	}
}