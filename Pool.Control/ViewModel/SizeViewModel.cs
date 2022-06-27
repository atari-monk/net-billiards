namespace Pool.Control;

public class SizeViewModel
	: ViewModel
{
	private int height;
	private int width;

	public int Height
	{
		get => height;

		set
		{
			height = value;
			OnPropertyChanged(nameof(Height));
		}
	}

	public int Width
	{
		get => width;

		set
		{
			width = value;
			OnPropertyChanged(nameof(Width));
		}
	}
}