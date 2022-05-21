using Shape.Model;
using Sim.Core;

namespace Pool.Container;

public class GameDependencies
	: IGameDependencies
{
	public GameDependencies(
		ILoggerToMemory logger
		, IShapeFactory shapeFactory
		, ISerializer serializerXml
		, IGameData gameData)
	{
		Logger = logger;
		ShapeFactory = shapeFactory;
		SerializerXml = serializerXml;
		GameData = gameData;
	}

	public ILoggerToMemory Logger { get; }

	public IShapeFactory ShapeFactory { get; }

	public ISerializer SerializerXml { get; }

	public IGameData GameData { get; }
}