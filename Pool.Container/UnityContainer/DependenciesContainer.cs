using Pool.Engine;
using Shape.Model;
using Sim.Core;
using Unity;
using Unity.Injection;

namespace Pool.Container;

public class DependenciesContainer
	: ContainerBase
{
	public DependenciesContainer(IUnityContainer unityContainer) : base(unityContainer)
	{
	}

	public override void Register()
	{
		Container.RegisterType<ILoggerToMemory, LoggerToMemory>(
			TypeLifetime.Singleton);

		Container.RegisterType<IOrder<string>, GameDataPath>(TypeLifetime.Singleton);

		Container.RegisterType<IShapeFactory, ShapeFactory>(TypeLifetime.Singleton);
		Container.RegisterType<ISerializer, SerializerXml>();

		Container.RegisterType<IGameData, GameData>(
			TypeLifetime.Singleton
			, new InjectionConstructor(
				Container.Resolve<ISerializer>()
				, Container.Resolve<IOrder<string>>().Order()));
	}
}