using Sim.Core;

namespace Pool.Engine;

public class TestGameLoop
    : GameLoop
{
    private readonly LoggerToFile logger = new LoggerToFile();

    public TestGameLoop(IBilliardGameContext gameState,
        ITimer timer)
            : base(gameState, timer)
    {
    }

    public override void ProcessInput() { }

    public override void Update(double ms) { }

    public override void Render() { }

    public override void Log()
    {
        logger.Log($"{ToString()}{Environment.NewLine}");
        if (TimeTotal <= 5.0) return;
        IsGameLoopOn = false;
        logger.Dispose();
    }
}