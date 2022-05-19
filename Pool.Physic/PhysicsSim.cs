using Sim.Core;

namespace Pool.Physic;

public class PhysicsSim
    : IPhysics
{
    private readonly ICirclePhysics circlePhysics;

    public ICirclesCollisonDuringFrame BallCollisions { get; set; }

    public PhysicsSim(
        ICirclePhysics circlePhysics
        , ICirclesCollisonDuringFrame ballCollisions)
    {
        this.circlePhysics = circlePhysics;
        BallCollisions = ballCollisions;
    }

    public void RunPhysics(double frameDeltaTime) =>
        circlePhysics.DoCirclePhysics(frameDeltaTime);
}