using Sim.Core;

namespace Pool.Physic;

public class Euler
    : IUnaryCirclePhysicsStrategy
{
    public void CirclePhysis(
        IShape circle
        , double frameDeltaTime
        , IGameData gameData)
    {
        circle.MassCenter += circle.Velocity * frameDeltaTime;
    }
}