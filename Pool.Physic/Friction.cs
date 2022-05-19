using Sim.Core;

namespace Pool.Physic;

public class Friction
    : IUnaryCirclePhysicsStrategy
{
    private readonly IShapeFactory shapeFactory;
    private const double FrictionConstant = 40;

    public Friction(IShapeFactory shapeFactory) =>
        this.shapeFactory = shapeFactory;

    public void CirclePhysis(
        IShape circle,
        double frameDeltaTime,
        IGameData gameData)
    {
        if (!IsMoving(circle)) return;
        var velocityNormal = circle.Velocity.Normalize();
        var velocityFriction = -velocityNormal * FrictionConstant * frameDeltaTime;
        circle.Velocity += velocityFriction;

        gameData.VectorAnalitics?.Add(
            shapeFactory.GetVelocityVector(
                circle.MassCenter,
                circle.MassCenter + velocityFriction));
    }

    private static bool IsMoving(IShape circle) =>
        circle.Velocity.X != 0 || circle.Velocity.Y != 0;
}