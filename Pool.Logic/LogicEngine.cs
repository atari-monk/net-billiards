using Sim.Core;

namespace Pool.Logic;

public class LogicEngine : ILogics
{
    private readonly IGameData gameData;
    private readonly List<IBinaryLogic> binaryLogics;
    private readonly List<IShapesLogic> listLogics;

    public LogicEngine(IGameData data,
        List<IBinaryLogic> binaryLogics,
        List<IShapesLogic> listLogics)
    {
        gameData = data;
        this.binaryLogics = binaryLogics;
        this.listLogics = listLogics;
    }

    public void RunLogics(
        double frameDeltaTime)
    {
        CalculateBinaryLogic(frameDeltaTime);
        CalculateListLogic(frameDeltaTime);
    }

    private void CalculateListLogic(
        double frameDeltaTime)
    {
        foreach (var logic in listLogics)
        {
            logic.ShapesLogic(
                gameData.Circles,
                frameDeltaTime,
                gameData);
        }
    }

    private void CalculateBinaryLogic(
        double frameDeltaTime)
    {
        foreach (var strategy in binaryLogics)
        {
            if (!(strategy is CircleInSink)) continue;
            CalculateCircleInSinkLogic(strategy, frameDeltaTime);
        }
    }

    private void CalculateCircleInSinkLogic(
        IBinaryLogic circleInSinkLogic,
        double frameDeltaTime)
    {
        foreach (var circle in gameData.Circles)
        {
            foreach (var sink in gameData.Sinks)
            {
                circleInSinkLogic.BinaryLogic(circle, sink, frameDeltaTime, gameData);
            }
        }
    }
}