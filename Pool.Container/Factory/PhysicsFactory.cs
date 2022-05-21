using Pool.Physic;
using Sim.Core;

namespace Pool.Container;

public class PhysicsFactory
	: IOrder<IPhysics>
{
	private readonly IShapeFactory _shapeFactory;
	private readonly IGameData _gameData;

	public PhysicsFactory(
		IShapeFactory shapeFactory
		, IGameData gameData)
	{
		_shapeFactory = shapeFactory;
		_gameData = gameData;
	}

	public IPhysics Order()
	{
		ICirclesCollisonDuringFrame ballCollisions = new CirclesCollisonDuringFrame();

		var unaryCircle = new List<IUnaryCirclePhysicsStrategy>
			{
				new Euler(),
				new Friction(_shapeFactory)
			};

		var binaryCircle = new List<IBinaryCirclePhysicsStrategy>
			{
				ballCollisions,
				new CirclesPolygonCollison(_shapeFactory)
			};

		var circlePhysics = new CirclePhysics(
			_gameData
			, unaryCircle
			, binaryCircle);

		return new PhysicsSim(circlePhysics, ballCollisions);
	}
}