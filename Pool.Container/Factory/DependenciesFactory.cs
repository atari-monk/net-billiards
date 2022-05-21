using Pool.Engine;
using Shape.Model;
using Sim.Core;

namespace Pool.Container;

public class DependenciesFactory
	: IOrder<IGameDependencies>
{
	private readonly string _filePath;

	public DependenciesFactory(
		string filePath)
	{
		_filePath = filePath;
	}

	public IGameDependencies Order()
	{
		var serializer = new SerializerXml();
		var dependencies = new GameDependencies(
			new LoggerToMemory()
			, new ShapeFactory()
			, serializer
			, new GameData(serializer, _filePath));
		return dependencies;
	}
}