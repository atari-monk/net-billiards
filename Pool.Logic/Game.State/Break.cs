using Sim.Core;

namespace Pool.Logic;

public class Break
    : GameState
{
    public Break(IGameState state)
        : this(state?.Game) => DoBeforeTurn();

    public Break(IBilliardGameMasterContext? game) => Game = game;

    public override string ToString() => nameof(Break);

    public override void DoBeforeTurn()
    {
        ArgumentNullException.ThrowIfNull(Game);
        Game.SetScoreInfo();
        SetState(nameof(Break));
        Game.GameCanvas.GameInputEvent += Game.PlayerMove;
    }

    private void SetState(string text)
    {
        ArgumentNullException.ThrowIfNull(Game);
        if (Game.PlayerMenager.PlayerInCurrentRound == Game.PlayerMenager.PlayerOne)
            Game.SetPlayer1Stats(new PlayerStats { State = text });
        else
            Game.SetPlayer2Stats(new PlayerStats { State = text });
    }

    public override void DoAfterTurn()
    {
        ArgumentNullException.ThrowIfNull(Game);
        SetState(string.Empty);
        Game.FixFaul();
    }

    public override void DoAfterFoul()
    {
        ArgumentNullException.ThrowIfNull(Game);
        if (Game.FaulState is FixedFaul)
        {
            Game.PlayerMenager.SwitchCurrentRoundPlayer();
            Game.SetOpenTableState(this);
        }
        else if (Game.FaulState is NoFaul)
            Game.SetOpenTableState(this);
    }
}