using Sim.Core;

namespace Pool.Engine;

public abstract class GameLoop
    : IGameLoop
{
    public event Action<GameStats>? SetLabelEvent;

    private readonly IBilliardGameContext gameContext;
    private readonly ITimer timer;

    protected double TimeTotal;
    protected double FramesPerSecond;
    int loopPauseTime;
    int frameCount;
    double previousFrameTime;
    double elapsedSeconds;

    protected bool IsGameLoopOn { get; set; } = true;

    public GameLoop(
        IBilliardGameContext gameContext,
        ITimer timer)
    {
        this.gameContext = gameContext;
        this.timer = timer;
    }

    public void RunGameLoop()
    {
        gameContext.StartGame();
        InitilaizeLoopState();
        while (IsGameLoopOn)
        {
            CalculateFrame();
            if (IsSecondPassed())
            {
                OnSecondPassed();
                Log();
            }
        }
    }

    private void InitilaizeLoopState()
    {
        loopPauseTime = 0;
        frameCount = 0;
        previousFrameTime = 0;
        elapsedSeconds = 0;
    }

    private void CalculateFrame()
    {
        timer.Start();
        ProcessInput();
        Update(elapsedSeconds);
        Render();
        Thread.Sleep(loopPauseTime);
        frameCount++;
        timer.Stop();
        elapsedSeconds = timer.ElapsedSeconds;
        timer.Reset();
        TimeTotal += elapsedSeconds;
    }

    public abstract void ProcessInput();

    public abstract void Update(double frameDeltaTime);

    public abstract void Render();

    private bool IsSecondPassed() =>
        TimeTotal - previousFrameTime >= 1.0;

    private void OnSecondPassed()
    {
        FramesPerSecond = frameCount;
        SetPauseVale();
        previousFrameTime = TimeTotal;
        frameCount = 0;
        SetLabelEvent?.Invoke(new GameStats { Fps = $"{FramesPerSecond}" });
        SetLabelEvent?.Invoke(new GameStats { Time = $"{string.Format("{0:0}", TimeTotal)}" });
    }

    private void SetPauseVale()
    {
        if (FramesPerSecond > 50)
            loopPauseTime += 1;
    }

    public abstract void Log();
}