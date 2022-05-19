using Sim.Core;

namespace Pool.Logic;

public class BlackScoredInBreak
    : FaulState
{
    public BlackScoredInBreak(
        IFaulState state)
            : this(state?.Game)
    {

    }

    public BlackScoredInBreak(
        IBilliardGameMasterContext? game) => Game = game;

    public override void FixFaul()
    {
        ArgumentNullException.ThrowIfNull(Game);
        if (Game.PlayerMenager.GetOther() == Game.PlayerMenager.PlayerOne)
            Game.SetPlayer1Stats(new PlayerStats { Faul = $"{Game.PlayerMenager.GetOther().Name} restores black ball" });
        else
            Game.SetPlayer2Stats(new PlayerStats { Faul = $"{Game.PlayerMenager.GetOther().Name} restores black ball" });
        Game.LastFaulState = Game.FaulState;
        Game.GameCanvas.GameInputEvent += Game.RestoreBlackBallAfterScoringIt;
    }

    public override bool IsFaulConditionMeet()
    {
        ArgumentNullException.ThrowIfNull(Game);
        return IsBlackBallScored() && Game.State is Break;
    }
}