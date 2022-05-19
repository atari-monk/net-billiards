using Sim.Core;

namespace Pool.Logic;

public class NoMovement
    : MovementState
{
    public NoMovement(IMovementState state)
        : this(state?.Game)
    {

    }

    public NoMovement(IBilliardGameContext? game)
    {
        Game = game;
    }

    public override string ToString() => nameof(NoMovement);
}