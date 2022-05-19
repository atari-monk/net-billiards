using Sim.Core;

namespace Pool.Logic;

public class WonGame : GameState
{
    public WonGame(IGameState state)
        : this(state.Game) => DoBeforeTurn();

    public WonGame(IBilliardGameMasterContext game)
        => Game = game;

    public override void DoBeforeTurn()
    {
        if (Game.PlayerMenager.PlayerInCurrentRound == Game.PlayerMenager.PlayerOne)
            Game.SetPlayer1Stats(new PlayerStats { State = "Won Game :) Hug your opponent" });
        else
            Game.SetPlayer2Stats(new PlayerStats { State = "Try next time :) Hug your opponent" });
    }
}