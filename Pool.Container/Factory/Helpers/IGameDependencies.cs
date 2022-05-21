using Shape.Model;
using Sim.Core;

namespace Pool.Container;

public interface IGameDependencies
{
	ILoggerToMemory Logger { get; }
	IShapeFactory ShapeFactory { get; }
	ISerializer SerializerXml { get; }
	IGameData GameData { get; }
}