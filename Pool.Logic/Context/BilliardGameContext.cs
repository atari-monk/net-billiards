using System.Windows;
using System.Windows.Input;
using Sim.Core;
using Vector.Lib;

namespace Pool.Logic;

public abstract class BilliardGameContext
    : IBilliardGameContext
{
    private const string WhiteBilliardBallFlag = "white";

    public event Action? EndTurnEvent;
    public event Action<PlayerStats>? SetPlayer1StatsEvent;
    public event Action<PlayerStats>? SetPlayer2StatsEvent;

    private readonly IMovementFactory movementFactory;

    private Point mousePoint;
    private Vector2 mousePoint3D;
    private IShape? player;

    public ILoggerToMemory Logger { get; private set; }
    public IGameData GameData { get; private set; }
    public IPhysics Physics { get; private set; }
    public ICanvasSerializaton<IShape> GameCanvas { get; private set; }
    public IMovementState MovementState { get; private set; }
    public List<IShape> ScoredBilliardBalls { get; private set; }

    public BilliardGameContext(
        IGameData gameData
        , IPhysics physic
        , ICanvasSerializaton<IShape> gameCanvas
        , IMovementFactory movementFactory
        , ILoggerToMemory logger)
    {
        GameData = gameData;
        Physics = physic;
        GameCanvas = gameCanvas;
        this.movementFactory = movementFactory;
        Logger = logger;
        MovementState = this.movementFactory.GetNoMovement(this);
        ScoredBilliardBalls = new List<IShape>();
        mousePoint3D = new Vector2();
    }

    public void SetPlayer1Stats(PlayerStats playerStats) =>
        SetPlayer1StatsEvent?.Invoke(playerStats);

    public void SetPlayer2Stats(PlayerStats playerStats) =>
        SetPlayer2StatsEvent?.Invoke(playerStats);

    public void SetNoMovementState()
    {
        MovementState = movementFactory.GetNoMovement(MovementState);
        EndTurnEvent?.Invoke();
    }

    public void PlayerMove(object? sender, MouseButtonEventArgs mouseButtonEventArgs)
    {
        ArgumentNullException.ThrowIfNull(sender);
        var uiElement = (UIElement)sender;
        mousePoint = mouseButtonEventArgs.GetPosition(uiElement);
        mousePoint3D = new Vector2(mousePoint.X, mousePoint.Y);
        player = GetPlayer();
        player.Velocity = mousePoint3D - player.MassCenter;
        MovementState = movementFactory.GetMovement(MovementState);
    }

    public virtual IShape GetPlayer()
    {
        player = GameData.Circles.Find(shape => shape.TextFlag == WhiteBilliardBallFlag);
        ArgumentNullException.ThrowIfNull(player);
        return player;
    }

    public void ScoreBilliardBall(IShape shape)
    {
        ScoredBilliardBalls.Add(shape);
        GameCanvas.RemoveShape(shape);
    }

    public bool IsMovementOnBilliardTable() =>
        MovementState is Movement;

    public virtual void StartGame() { }
}