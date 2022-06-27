using Sim.Core;

namespace Pool.Control;

public class RescaleViewModel
	: SizeViewModel
{
	private double xScale;
	private double yScale;
	private readonly IConfigProvider configProvider;

	public double XScale
	{
		get => xScale;

		set
		{
			xScale = value;
			OnPropertyChanged(nameof(XScale));
		}
	}

	public double YScale
	{
		get => yScale;

		set
		{
			yScale = value;
			OnPropertyChanged(nameof(YScale));
		}
	}

	public RescaleViewModel(IConfigProvider configProvider)
	{
		this.configProvider = configProvider;
        Width = int.Parse(this.configProvider.ReadSetting(nameof(Width)));
        Height = int.Parse(this.configProvider.ReadSetting(nameof(Height)));
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