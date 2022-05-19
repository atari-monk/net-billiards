using Sim.Core;

namespace Pool.Logic;

public class MovementFactory
    : IMovementFactory
{
    public IMovementState GetMovement(
        IMovementState movementState) =>
            new Movement(movementState);

    public IMovementState GetNoMovement(
        IMovementState movementState) =>
            new NoMovement(movementState);

    public IMovementState GetNoMovement(
        IBilliardGameContext gameContext) =>
            new NoMovement(gameContext);
}