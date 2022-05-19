using Sim.Core;

namespace Pool.Logic;

public class WhiteScored
    : FaulState
{
    public WhiteScored(
        IFaulState state)
            : this(state?.Game)
    {

    }

    public WhiteScored(
        IBilliardGameMasterContext? game) => Game = game;

    public override void FixFaul()
    {
        ArgumentNullException.ThrowIfNull(Game);
        var player = Game.PlayerMenager.GetOther();
        Game.Logger.Log($"{player.Name} restores white ball");
        if (player == Game.PlayerMenager.PlayerOne)
            Game.SetPlayer1Stats(new PlayerStats { Faul = "Restores white ball" });
        else
            Game.SetPlayer2Stats(new PlayerStats { Faul = "Restores white ball" });
        Game.LastFaulState = Game.FaulState;
        Game.GameCanvas.GameInputEvent += Game.RestoreWhiteBallAfterScoringIt;
    }

    public override bool IsFaulConditionMeet() => IsWhiteBallScored();
}