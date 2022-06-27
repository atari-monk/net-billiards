using System.Windows.Media;
using Canvas;
using Shape.Model;
using Sim.Core;

namespace Pool.Container;

public class ControlsFactory 
	: IOrder<ICanvasVisualControl<IShape>>
{
	private readonly IShapeFactory shapeFactory;
	private readonly ISerializer serializer;
	private readonly IGameData gameData;

	public ControlsFactory(
		IShapeFactory shapeFactory
		, ISerializer serializer
		, IGameData gameData)
	{
		this.shapeFactory = shapeFactory;
		this.serializer = serializer;
		this.gameData = gameData;
	}

	public ICanvasVisualControl<IShape> Order()
	{
		var canvasSerializaton = new CanvasSerializaton(
			OrderBackground()
			, serializer);
		var control = new CanvasVisualControl(canvasSerializaton);
		control?.Canvas?.Shapes?.AddRange(gameData.Shapes);
        ArgumentNullException.ThrowIfNull(control);
		return control;
	}

	private IRectangle OrderBackground()
	{
        var rectangle = shapeFactory.GetShape(ShapeTypes.Rectangle, Colors.White) as IRectangle;
        ArgumentNullException.ThrowIfNull(rectangle);
		return rectangle;
	}
}