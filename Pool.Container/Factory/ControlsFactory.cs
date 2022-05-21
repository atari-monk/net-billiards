using System.Windows.Media;
using Canvas;
using Shape.Model;
using Sim.Core;

namespace Pool.Container;

public class ControlsFactory 
	: IOrder<ICanvasVisualControl<IShape>>
{
	private readonly IShapeFactory _shapeFactory;
	private readonly ISerializer _serializer;
	private readonly IGameData _gameData;

	public ControlsFactory(
		IShapeFactory shapeFactory
		, ISerializer serializer
		, IGameData gameData)
	{
		_shapeFactory = shapeFactory;
		_serializer = serializer;
		_gameData = gameData;
	}

	public ICanvasVisualControl<IShape> Order()
	{
		var canvasSerializaton = new CanvasSerializaton(
			OrderBackground()
			, _serializer);

		var control = new CanvasVisualControl(canvasSerializaton);

		control.Canvas.Shapes.AddRange(_gameData.Shapes);

		return control;
	}

	private IRectangle OrderBackground()
	{
		return _shapeFactory.GetShape(ShapeTypes.Rectangle, Colors.White) as IRectangle;
	}
}