using System.Windows.Media;
using Canvas;
using Shape.Model;
using Sim.Core;
using Unity;
using Unity.Injection;

namespace Pool.Container;

public class ControlsContainer
	: ContainerBase
{
	public ControlsContainer(IUnityContainer unityContainer)
		: base(unityContainer)
	{
	}

	public override void Register()
	{
		Container.RegisterType<ICanvasSerializaton<IShape>, CanvasSerializaton>(
			TypeLifetime.Singleton
			, new InjectionConstructor(
				Container.Resolve<IShapeFactory>().GetShape(ShapeTypes.Rectangle, Colors.White) as IRectangle
				, Container.Resolve<ISerializer>()));

		Container.RegisterType<ICanvasVisualControl<IShape>, CanvasVisualControl>(
			TypeLifetime.Singleton
			, new InjectionConstructor(
				Container.Resolve<ICanvasSerializaton<IShape>>()));
	}

	public override void Initialize()
	{
		var control = Container.Resolve<ICanvasVisualControl<IShape>>();
		var gameDate = Container.Resolve<IGameData>();
		control?.Canvas?.Shapes?.AddRange(gameDate.Shapes);
	}
}