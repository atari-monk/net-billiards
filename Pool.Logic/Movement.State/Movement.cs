using Sim.Core;

namespace Pool.Logic;

public class Movement
    : MovementState
{
    public Movement(IMovementState state)
        : this(state?.Game)
    {
        ArgumentNullException.ThrowIfNull(state?.Game);
        ArgumentNullException.ThrowIfNull(Game);
        Game.GameCanvas.GameInputEvent -= Game.PlayerMove;
    }

    public Movement(IBilliardGameContext? game)
    {
        Game = game;
    }

    public override string ToString() => nameof(Movement);
}