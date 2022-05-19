using System.Windows;
using System.Windows.Input;
using Sim.Core;

namespace Pool.Logic;

public class BilliardGameMasterContext : BilliardGameContext, IBilliardGameMasterContext
{
    private readonly IGameStateFactory _gameStateFactory;
    private readonly IFaulStateFactory _faulStateFactory;
    private readonly IShapeFactory _shapeFactory;

    private const string WhiteBallTextFlag = "white";
    private const string BlackBallTextFlag = "black";

    public IGameState State { get; set; }

    public IFaulState FaulState { get; set; }

    public IFaulState LastFaulState { get; set; } = null;

    public IBilliardPlayerManager PlayerMenager { get; set; }

    public List<IFaulState> Fauls { get; set; }

    public BilliardGameMasterContext(IGameData data
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
        _gameStateFactory = gameStateFactory;
        _faulStateFactory = faulStateFactory;
        _shapeFactory = shapeFactory;
        PlayerMenager = billiardPlayerManager;

        State = _gameStateFactory.GetBreak(this);
        EndTurnEvent += HandleAfterTurn;
        SetInitialFaulState();
    }

    public void SetScoreInfo()
    {
        SetPlayer1Stats(new PlayerStats { Score = ScoredBilliardBalls.Count(ball => ball.TextFlag == PlayerMenager.PlayerOne.Color).ToString() });
        SetPlayer2Stats(new PlayerStats { Score = ScoredBilliardBalls.Count(ball => ball.TextFlag == PlayerMenager.PlayerTwo.Color).ToString() });
    }

    public void RestoreBlackBallAfterScoringIt(object sender, MouseButtonEventArgs mouseArgs)
    {
        var massCenter = mouseArgs.GetPosition((UIElement)sender);
        var newBlackBall = _shapeFactory.GetBlackBall(massCenter);
        GameData.Shapes.Add(newBlackBall);
        var black = ScoredBilliardBalls.First(ball => ball.TextFlag == BlackBallTextFlag);
        ScoredBilliardBalls.Remove(black);
        GameCanvas.GameInputEvent -= RestoreBlackBallAfterScoringIt;
    }

    public void RestoreWhiteBallAfterScoringIt(object sender, MouseButtonEventArgs mouseArgs)
    {
        var massCenter = mouseArgs.GetPosition((UIElement)sender);
        var newWhiteBall = _shapeFactory.GetWhiteBall(massCenter);
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
        return player;
    }

    public override void StartGame()
    {
        SetBreakState(State);
    }

    public void FixFaul()
    {
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
        FaulState.FixFaul();
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
            FaulState = _faulStateFactory.GetNoFaul(this);
        else FaulState = _faulStateFactory.GetFixedFaul(this);
    }

    private void SetInitialFaulState()
    {
        LastFaulState = null;
        FaulState = _faulStateFactory.GetNoFaul(this);
    }

    public void HandleAfterTurn() => State.DoAfterTurn();

    public void SetBreakState(IGameState gameState) =>
        State = _gameStateFactory.GetBreak(gameState);

    public void SetOpenTableState(IGameState gameState) =>
        State = _gameStateFactory.GetOpenTable(gameState);

    public void SetTurnState(IGameState gameState) =>
        State = _gameStateFactory.GetTurn(gameState);

    public void SetWonState(IGameState gameState) =>
        State = _gameStateFactory.GetWonGame(gameState);
}