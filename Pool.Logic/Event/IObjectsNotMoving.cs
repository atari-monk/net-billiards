using Sim.Core;

namespace Pool.Logic;

public interface IObjectsNotMoving
    : IShapesLogic
{
    event Action SignalNoMovementEvent;

    event Func<bool> IsItMovingEvent;
}