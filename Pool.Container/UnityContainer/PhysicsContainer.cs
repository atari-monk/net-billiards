using Pool.Physic;
using Sim.Core;
using Unity;
using Unity.Injection;

namespace Pool.Container;

public class PhysicsContainer
	: ContainerBase
{
	public PhysicsContainer(IUnityContainer unityContainer)
        : base(unityContainer)
	{
	}

	public override void Register()
	{
		var circlesCollisonDuringFrame = Container.Resolve<CirclesCollisonDuringFrame>();
		Container.RegisterInstance<IBinaryCirclePhysicsStrategy>(circlesCollisonDuringFrame);
		Container.RegisterInstance<ICirclesCollisonDuringFrame>(circlesCollisonDuringFrame);

		Container.RegisterType<IBinaryCirclePhysicsStrategy, CirclesPolygonCollison>(
			nameof(CirclesPolygonCollison)
			, TypeLifetime.Singleton
			, new InjectionConstructor(
				Container.Resolve<IShapeFactory>()));

		Container.RegisterType<IUnaryCirclePhysicsStrategy, Euler>(
			nameof(Euler)
			, TypeLifetime.Singleton);

		Container.RegisterType<IUnaryCirclePhysicsStrategy, Friction>(
			nameof(Friction)
			, TypeLifetime.Singleton
			, new InjectionConstructor(
				Container.Resolve<IShapeFactory>()));

		Container.RegisterType<ICirclePhysics, CirclePhysics>(
			TypeLifetime.Singleton
			, new InjectionConstructor(
				Container.Resolve<IGameData>()
				, Container.Resolve<List<IUnaryCirclePhysicsStrategy>>()
				, Container.Resolve<List<IBinaryCirclePhysicsStrategy>>()));

		Container.RegisterType<IPhysics, PhysicsSim>(
			TypeLifetime.Singleton
			, new InjectionConstructor(
				Container.Resolve<ICirclePhysics>()
				, Container.Resolve<ICirclesCollisonDuringFrame>()));
	}
}