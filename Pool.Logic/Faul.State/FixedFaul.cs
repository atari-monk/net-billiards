using Sim.Core;

namespace Pool.Logic;

public class FixedFaul
    : FaulState
{
    public FixedFaul(
        IFaulState state)
        : this(state?.Game)
    {

    }

    public FixedFaul(
        IBilliardGameMasterContext? game) => Game = game;

    public override void FixFaul() => Game?.State.DoAfterFoul();

    public override bool IsFaulConditionMeet() => true;
}