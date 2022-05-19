using Sim.Core;

namespace Pool.Physic;

public class CirclePhysics
    : ICirclePhysics
{
    private readonly IGameData gameData;

    private readonly List<IUnaryCirclePhysicsStrategy> unaryCirclePhysicsStrategies;

    private readonly List<IBinaryCirclePhysicsStrategy> binaryCirclePhysicsStrategies;

    public CirclePhysics(
        IGameData gameData
        , List<IUnaryCirclePhysicsStrategy> unaryCirclePhysicsStrategies
        , List<IBinaryCirclePhysicsStrategy> binaryCirclePhysicsStrategies)
    {
        this.unaryCirclePhysicsStrategies = unaryCirclePhysicsStrategies;
        this.binaryCirclePhysicsStrategies = binaryCirclePhysicsStrategies;
        this.gameData = gameData;
    }

    public void DoCirclePhysics(double dt)
    {
        DoUnaryCirclePhysics(dt);
        DoBinaryCirclePhysics(dt);
    }

    private void DoBinaryCirclePhysics(double dt)
    {
        foreach (var binaryStrategy in binaryCirclePhysicsStrategies)
        {
            CircleCircleCollision(dt, binaryStrategy);
            CircleLineCOllision(dt, binaryStrategy);
        }
    }

    private void CircleCircleCollision(double dt, IBinaryCirclePhysicsStrategy binaryStrategy)
    {
        if (binaryStrategy is ICirclesCollisonDuringFrame)
        {
            foreach (var circle1 in gameData.Circles)
            {
                foreach (var circle2 in gameData.Circles)
                {
                    if (circle1 != circle2)
                        binaryStrategy.CirclePhysis(dt, gameData, circle1, circle2);
                }
            }
        }
    }

    private void CircleLineCOllision(double dt, IBinaryCirclePhysicsStrategy binaryStrategy)
    {
        if (binaryStrategy is CirclesPolygonCollison)
        {
            foreach (var circle1 in gameData.Circles)
            {
                foreach (var polygon in gameData.Polygons)
                {
                    binaryStrategy.CirclePhysis(dt, gameData, circle1, polygon);
                }
            }
            if (gameData.VectorAnalitics != null)
            {
                lock (gameData.VectorAnalitics)
                {
                    gameData.VectorAnalitics.CompleteAdding();
                }
            }
        }
    }

    private void DoUnaryCirclePhysics(double dt)
    {
        foreach (var unaryStrategy in unaryCirclePhysicsStrategies)
        {
            foreach (var circle in gameData.Circles)
            {
                unaryStrategy.CirclePhysis(circle, dt, gameData);
            }
        }
    }
}