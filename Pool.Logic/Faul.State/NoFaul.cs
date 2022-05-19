using Sim.Core;

namespace Pool.Logic;

public class NoFaul
    : FaulState
{
    public NoFaul(
        IFaulState state)
        : this(state?.Game)
    {

    }

    public NoFaul(
        IBilliardGameMasterContext? game)
    {
        Game = game;
    }

    public override void FixFaul() => Game?.State.DoAfterFoul();

    public override bool IsFaulConditionMeet() => true;
}