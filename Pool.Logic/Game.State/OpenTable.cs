using Sim.Core;

namespace Pool.Logic;

public class OpenTable
    : GameState
{
    private int beforeTurnScore = 0;
    private int turnScore = 0;

    public OpenTable(IGameState state)
        : this(state.Game) => DoBeforeTurn();

    public OpenTable(IBilliardGameMasterContext? game) => Game = game;

    public override string ToString() => nameof(OpenTable);

    public override void DoBeforeTurn()
    {
        ArgumentNullException.ThrowIfNull(Game);
        if (Game.PlayerMenager.PlayerInCurrentRound == Game.PlayerMenager.PlayerOne)
            Game.SetPlayer1Stats(new PlayerStats { State = "Open table" });
        else
            Game.SetPlayer2Stats(new PlayerStats { State = "Open table" });
        Game.GameCanvas.GameInputEvent += Game.PlayerMove;
        beforeTurnScore =
            Game.ScoredBilliardBalls.Count(ball => ball.TextFlag == "full") +
            Game.ScoredBilliardBalls.Count(ball => ball.TextFlag == "stripe");
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
        turnScore =
            Game.ScoredBilliardBalls.Count(a => a.TextFlag == "full") +
            Game.ScoredBilliardBalls.Count(a => a.TextFlag == "stripe")
            - beforeTurnScore;
        if (Game.FaulState is FixedFaul)
        {
            Game.PlayerMenager.SwitchCurrentRoundPlayer();
            Game.SetOpenTableState(this);
            return;
        }
        else if (Game.FaulState is NoFaul)
        {
            if (turnScore > 0)
            {
                var flag = Game!.ScoredBilliardBalls[Game.ScoredBilliardBalls.Count - turnScore].TextFlag;
                ArgumentNullException.ThrowIfNull(flag);
                Game.PlayerMenager.OpenTable(flag);
                Game.SetTurnState(this);
            }
            else
            {
                Game.PlayerMenager.SwitchCurrentRoundPlayer();
                Game.SetOpenTableState(this);
            }
        }
    }
}