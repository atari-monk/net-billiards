using System.Windows;
using System.Windows.Input;
using Sim.Core;
using Vector.Lib;

namespace Pool.Logic;

public abstract class BilliardGameContext : IBilliardGameContext
{
    public event Action EndTurnEvent;

    public event Action<PlayerStats> SetPlayer1StatsEvent;
    public event Action<PlayerStats> SetPlayer2StatsEvent;

    private readonly IMovementFactory _movementFactory;

    private Point _mousePoint;
    private Vector2 _mousePoint3D;
    private IShape _player;
    private const string WhiteBilliardBallFlag = "white";

    public ILoggerToMemory Logger { get; private set; }

    public IGameData GameData { get; private set; }

    public IPhysics Physics { get; private set; }

    public ICanvasSerializaton<IShape> GameCanvas { get; private set; }

    public IMovementState MovementState { get; private set; }

    public List<IShape> ScoredBilliardBalls { get; private set; }

    public BilliardGameContext(IGameData gameData
        , IPhysics physic
        , ICanvasSerializaton<IShape> gameCanvas
        , IMovementFactory movementFactory
        , ILoggerToMemory logger)
    {
        GameData = gameData;
        Physics = physic;
        GameCanvas = gameCanvas;
        _movementFactory = movementFactory;
        Logger = logger;

        MovementState = _movementFactory.GetNoMovement(this);
        ScoredBilliardBalls = new List<IShape>();
        _mousePoint3D = new Vector2();
    }

    public void SetPlayer1Stats(PlayerStats playerStats) =>
        SetPlayer1StatsEvent?.Invoke(playerStats);

    public void SetPlayer2Stats(PlayerStats playerStats) =>
        SetPlayer2StatsEvent?.Invoke(playerStats);

    public void SetNoMovementState()
    {
        MovementState = _movementFactory.GetNoMovement(MovementState);
        EndTurnEvent?.Invoke();
    }

    public void PlayerMove(object? sender, MouseButtonEventArgs mouseButtonEventArgs)
    {
        _mousePoint = mouseButtonEventArgs.GetPosition((UIElement)sender);
        _mousePoint3D = new Vector2(_mousePoint.X, _mousePoint.Y);
        _player = GetPlayer();
        _player.Velocity = _mousePoint3D - _player.MassCenter;
        MovementState = _movementFactory.GetMovement(MovementState);
    }

    public virtual IShape GetPlayer()
    {
        _player = GameData.Circles.Find(shape => shape.TextFlag == WhiteBilliardBallFlag);
        return _player;
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