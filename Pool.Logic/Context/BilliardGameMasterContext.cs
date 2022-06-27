using System.Windows;
using System.Windows.Input;
using Sim.Core;

namespace Pool.Logic;

public class BilliardGameMasterContext
    : BilliardGameContext
        , IBilliardGameMasterContext
{
    private const string WhiteBallTextFlag = "white";
    private const string BlackBallTextFlag = "black";    

    private readonly IGameStateFactory gameStateFactory;
    private readonly IFaulStateFactory faulStateFactory;
    private readonly IShapeFactory shapeFactory;

    public IGameState State { get; set; }

    public IFaulState? FaulState { get; set; }

    public IFaulState? LastFaulState { get; set; } = null;

    public IBilliardPlayerManager PlayerMenager { get; set; }

    public List<IFaulState>? Fauls { get; set; }

    public BilliardGameMasterContext(
        IGameData data
        , IPhysics physic
        , ICanvasSerializaton<IShape> canvas
        , IBilliardPlayerManager billiardPlayerManager
        , IMovementFactory movementFactory
        , IGameStateFactory gameStateFactory
        , IShapeFactory shapeFactory
        , IFaulStateFactory faulStateFactory
        , ILoggerToMemory logger)
            : base(data, physic, canvas, movementFactory, logger)
    {
        this.gameStateFactory = gameStateFactory;
        this.faulStateFactory = faulStateFactory;
        this.shapeFactory = shapeFactory;
        PlayerMenager = billiardPlayerManager;
        State = this.gameStateFactory.GetBreak(this);
        EndTurnEvent += HandleAfterTurn;
        SetInitialFaulState();
    }

    public void SetScoreInfo()
    {
        SetPlayer1Stats(
            new PlayerStats 
                { 
                    Score = ScoredBilliardBalls.Count(
                        ball => ball.TextFlag == PlayerMenager?.PlayerOne?.Color).ToString() 
                });
        SetPlayer2Stats(
            new PlayerStats 
                { 
                    Score = ScoredBilliardBalls.Count(
                        ball => ball.TextFlag == PlayerMenager?.PlayerTwo?.Color).ToString()
                });
    }

    public void RestoreBlackBallAfterScoringIt(object? sender, MouseButtonEventArgs mouseArgs)
    {
        ArgumentNullException.ThrowIfNull(sender);
        var uiElement = (UIElement)sender;
        var massCenter = mouseArgs.GetPosition(uiElement);
        var newBlackBall = shapeFactory.GetBlackBall(massCenter);
        GameData.Shapes.Add(newBlackBall);
        var black = ScoredBilliardBalls.First(ball => ball.TextFlag == BlackBallTextFlag);
        ScoredBilliardBalls.Remove(black);
        GameCanvas.GameInputEvent -= RestoreBlackBallAfterScoringIt;
    }

    public void RestoreWhiteBallAfterScoringIt(object? sender, MouseButtonEventArgs mouseArgs)
    {
        ArgumentNullException.ThrowIfNull(sender);
        var uiElement = (UIElement)sender;
        var massCenter = mouseArgs.GetPosition(uiElement);
        var newWhiteBall = shapeFactory.GetWhiteBall(massCenter);
        GameData.Shapes.Add(newWhiteBall);
        var white = ScoredBilliardBalls.First(ball => ball.TextFlag == WhiteBallTextFlag);
        ScoredBilliardBalls.Remove(white);
        GameCanvas.RemoveShape(white);
        GameCanvas.GameInputEvent -= RestoreWhiteBallAfterScoringIt;
        GameCanvas.AddShape(newWhiteBall);
        RestoreFaulState();
        State.DoAfterFoul();
    }

    public override IShape GetPlayer()
    {
        var player = GameData.Circles.Find(ball => ball.TextFlag == WhiteBallTextFlag);
        ArgumentNullException.ThrowIfNull(player);
        return player;
    }

    public override void StartGame()
    {
        SetBreakState(State);
    }

    public void FixFaul()
    {
        ArgumentNullException.ThrowIfNull(Fauls);
        var isFaul = false;
        foreach (var faul in Fauls)
        {
            if (faul.IsFaulConditionMeet())
            {
                FaulState = faul;
                isFaul = true;
                break;
            }
        }
        FaulState?.FixFaul();
        if (!isFaul)
        {
            RestoreFaulState();
        }
    }

    private void RestoreFaulState()
    {
        SetPlayer1Stats(new PlayerStats { Faul = "" });
        SetPlayer2Stats(new PlayerStats { Faul = "" });
        if (LastFaulState == null)
            FaulState = faulStateFactory.GetNoFaul(this);
        else FaulState = faulStateFactory.GetFixedFaul(this);
    }

    private void SetInitialFaulState()
    {
        LastFaulState = null;
        FaulState = faulStateFactory.GetNoFaul(this);
    }

    public void HandleAfterTurn() => State.DoAfterTurn();

    public void SetBreakState(IGameState gameState) =>
        State = gameStateFactory.GetBreak(gameState);

    public void SetOpenTableState(IGameState gameState) =>
        State = gameStateFactory.GetOpenTable(gameState);

    public void SetTurnState(IGameState gameState) =>
        State = gameStateFactory.GetTurn(gameState);

    public void SetWonState(IGameState gameState) =>
        State = gameStateFactory.GetWonGame(gameState);
}