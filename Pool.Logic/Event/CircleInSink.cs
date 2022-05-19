using Sim.Core;

namespace Pool.Logic;

public class CircleInSink : ICircleInSink
	{
		public event Action<IShape> Scored;

		public void BinaryLogic(IShape Circle, IShape Sink, double dt, IGameData data)
		{
			var ball = Circle as ICircle;
			var sink = Sink as ICircle;

			if (ball.MassCenter.X + ball.Radius + sink.Radius > sink.MassCenter.X
				&& ball.MassCenter.X < sink.MassCenter.X + ball.Radius + sink.Radius
				&& ball.MassCenter.Y + ball.Radius + sink.Radius > sink.MassCenter.Y
				&& ball.MassCenter.Y < sink.MassCenter.Y + ball.Radius + sink.Radius)
			{
				double distance = Math.Sqrt(((ball.MassCenter.X - sink.MassCenter.X) * (ball.MassCenter.X - sink.MassCenter.X))
										  + ((ball.MassCenter.Y - sink.MassCenter.Y) * (ball.MassCenter.Y - sink.MassCenter.Y)));
				if (distance <= sink.Radius - ball.Radius)
				{
					//balls have collided
					if (data.Shapes.Contains(ball))
					{
						Scored(ball);
						data.Shapes.Remove(ball);
					}
				}
			}
		}
	}
