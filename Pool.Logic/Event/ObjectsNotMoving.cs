using Sim.Core;

namespace Pool.Logic;

public class ObjectsNotMoving : IObjectsNotMoving
	{
		public event Action SignalNoMovementEvent;

		public event Func<bool> IsItMovingEvent;

		public void ShapesLogic(List<IShape> shapes, double dt, IGameData data)
		{
			if (!IsItMovingEvent()) return;
			var notMovingCount = shapes.Count(IsBallNotMoving());
			if (notMovingCount == shapes.Count)
				SignalNoMovementEvent();
		}

		private static Func<IShape, bool> IsBallNotMoving() =>
			shape => shape.Velocity.X <= 1 &&
			shape.Velocity.X >= -1 &&
			shape.Velocity.Y <= 1 &&
			shape.Velocity.Y >= -1;
	}