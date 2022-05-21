namespace Pool.Control;

public class SizeViewModel
	: ViewModel
{
	private int _height;
	private int _width;

	public int Height
	{
		get => _height;

		set
		{
			_height = value;
			OnPropertyChanged(nameof(Height));
		}
	}

	public int Width
	{
		get => _width;

		set
		{
			_width = value;
			OnPropertyChanged(nameof(Width));
		}
	}
}