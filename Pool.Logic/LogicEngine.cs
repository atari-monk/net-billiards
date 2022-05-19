using Sim.Core;

namespace Pool.Logic;

public class LogicEngine : ILogics
{
    private readonly IGameData _gameData;
    private readonly List<IBinaryLogic> _binaryLogics;
    private readonly List<IShapesLogic> _listLogics;

    public LogicEngine(IGameData data,
        List<IBinaryLogic> binaryLogics,
        List<IShapesLogic> listLogics)
    {
        _gameData = data;
        _binaryLogics = binaryLogics;
        _listLogics = listLogics;
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
        foreach (var logic in _listLogics)
        {
            logic.ShapesLogic(
                _gameData.Circles,
                frameDeltaTime,
                _gameData);
        }
    }

    private void CalculateBinaryLogic(
        double frameDeltaTime)
    {
        foreach (var strategy in _binaryLogics)
        {
            if (!(strategy is CircleInSink)) continue;
            CalculateCircleInSinkLogic(strategy, frameDeltaTime);
        }
    }

    private void CalculateCircleInSinkLogic(
        IBinaryLogic circleInSinkLogic,
        double frameDeltaTime)
    {
        foreach (var circle in _gameData.Circles)
        {
            foreach (var sink in _gameData.Sinks)
            {
                circleInSinkLogic.BinaryLogic(circle, sink, frameDeltaTime, _gameData);
            }
        }
    }
}