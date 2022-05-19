using Sim.Core;

namespace Pool.Logic;

public class OpenTable : GameState
{
    private int _beforeTurnScore = 0;
    private int _turnScore = 0;

    public OpenTable(IGameState state)
        : this(state.Game) => DoBeforeTurn();

    public OpenTable(IBilliardGameMasterContext? game) => Game = game;

    public override string ToString() => nameof(OpenTable);

    public override void DoBeforeTurn()
    {
        if (Game.PlayerMenager.PlayerInCurrentRound == Game.PlayerMenager.PlayerOne)
            Game.SetPlayer1Stats(new PlayerStats { State = "Open table" });
        else
            Game.SetPlayer2Stats(new PlayerStats { State = "Open table" });
        Game.GameCanvas.GameInputEvent += Game.PlayerMove;
        _beforeTurnScore =
            Game.ScoredBilliardBalls.Count(ball => ball.TextFlag == "full") +
            Game.ScoredBilliardBalls.Count(ball => ball.TextFlag == "stripe");
    }

    public override void DoAfterTurn()
    {
        if (Game.PlayerMenager.PlayerInCurrentRound == Game.PlayerMenager.PlayerOne)
            Game.SetPlayer1Stats(new PlayerStats { State = "" });
        else
            Game.SetPlayer2Stats(new PlayerStats { State = "" });
        Game.FixFaul();
    }

    public override void DoAfterFoul()
    {
        _turnScore =
            Game.ScoredBilliardBalls.Count(a => a.TextFlag == "full") +
            Game.ScoredBilliardBalls.Count(a => a.TextFlag == "stripe")
            - _beforeTurnScore;
        if (Game.FaulState is FixedFaul)
        {
            Game.PlayerMenager.SwitchCurrentRoundPlayer();
            Game.SetOpenTableState(this);
            return;
        }
        else if (Game.FaulState is NoFaul)
        {
            if (_turnScore > 0)
            {
                Game.PlayerMenager.OpenTable(Game.ScoredBilliardBalls[Game.ScoredBilliardBalls.Count - _turnScore].TextFlag);
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