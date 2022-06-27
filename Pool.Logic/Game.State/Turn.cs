using Sim.Core;

namespace Pool.Logic;

public class Turn
    : GameState
{
    private const string WhiteBallTextFlag = "white";
    private int beforeTurnScore = 0;
    private int turnScore = 0;

    public Turn(IGameState state)
        : this(state.Game) => DoBeforeTurn();

    public Turn(IBilliardGameMasterContext? game)
        => Game = game;

    public override string ToString() => nameof(Turn);

    public override void DoBeforeTurn()
    {
        ArgumentNullException.ThrowIfNull(Game);
        if (Game.PlayerMenager.PlayerInCurrentRound == Game.PlayerMenager.PlayerOne)
        {
            Game.SetPlayer1Stats(new PlayerStats { State = "Turn" });
            Game.SetPlayer1Stats(new PlayerStats { Color = Game.PlayerMenager.PlayerOne?.Color });

        }
        else
        {
            Game.SetPlayer2Stats(new PlayerStats { State = "Turn" });
            Game.SetPlayer2Stats(new PlayerStats { Color = Game.PlayerMenager.PlayerTwo?.Color });

        }
        Game.SetScoreInfo();
        Game.GameCanvas.GameInputEvent += Game.PlayerMove;
        Game.Physics.BallCollisions.SetWhiteBallFirstCollisionHandler(WhiteBallFirstCollisionHandler);
        beforeTurnScore = Game.ScoredBilliardBalls.Count(ball => ball.TextFlag == Game.PlayerMenager.PlayerInCurrentRound?.Color);
    }

    private void WhiteBallFirstCollisionHandler(string textFlag)
    {
        ArgumentNullException.ThrowIfNull(Game);
        if (textFlag != Game.PlayerMenager.PlayerInCurrentRound?.Color &&
                Game.ScoredBilliardBalls.Count(ball => ball.TextFlag == Game.PlayerMenager.PlayerInCurrentRound?.Color) < 2)
        {
            var white = Game.GameData.Shapes.Find(ball => ball.TextFlag == WhiteBallTextFlag);
            ArgumentNullException.ThrowIfNull(white);
            Game.ScoredBilliardBalls.Add(white);
            Game.GameData.Shapes.Remove(white);
            if (Game.PlayerMenager.PlayerInCurrentRound == Game.PlayerMenager.PlayerOne)
                Game.SetPlayer1Stats(new PlayerStats { Faul = "Hit wrong color" });
            else
                Game.SetPlayer2Stats(new PlayerStats { Faul = "Hit wrong color" });
        }
    }

    public override void DoAfterTurn()
    {
        ArgumentNullException.ThrowIfNull(Game);
        if (Game.PlayerMenager.PlayerInCurrentRound == Game.PlayerMenager.PlayerOne)
            Game.SetPlayer1Stats(new PlayerStats { State = "" });
        else
            Game.SetPlayer2Stats(new PlayerStats { State = "" });
        Game.FixFaul();
    }

    public override void DoAfterFoul()
    {
        ArgumentNullException.ThrowIfNull(Game);
        turnScore = Game.ScoredBilliardBalls.Count(ball => ball.TextFlag == Game.PlayerMenager.PlayerInCurrentRound?.Color) - beforeTurnScore;
        if (Game.FaulState is FixedFaul)
        {
            Game.PlayerMenager.SwitchCurrentRoundPlayer();
            Game.SetTurnState(this);
            return;
        }
        else if (Game.FaulState is NoFaul)
        {
            if (turnScore > 0)
            {
                Game.SetScoreInfo();
                Game.SetTurnState(this);
            }
            else
            {
                Game.PlayerMenager.SwitchCurrentRoundPlayer();
                Game.SetTurnState(this);
            }
        }
    }
}