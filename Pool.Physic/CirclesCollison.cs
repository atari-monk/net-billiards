using Sim.Core;
using Vector.Lib;

namespace Pool.Physic;

public class CirclesCollison
    : IBinaryCirclePhysicsStrategy
	{
		private IGameData? gameData;
		private readonly IShapeFactory shapeFactory;

		private const double Zero = 0.0;
		private double collisionPointX;
		private double collisionPointY;
		private double distance;
		private double radiusSum;

		public CirclesCollison(IShapeFactory shapeFactory) =>
			this.shapeFactory = shapeFactory;

		public void CirclePhysis(
			double dt,
			IGameData gameData,
			params IShape[] circles)
		{
			this.gameData = gameData;
			var ball1 = circles[0] as ICircle;
			var ball2 = circles[1] as ICircle;
            ArgumentNullException.ThrowIfNull(ball1);
            ArgumentNullException.ThrowIfNull(ball2);
			if (IsCollision(ball1, ball2))
			{
				distance = GetDistance(ball1, ball2);
				radiusSum = SumRadius(ball1, ball2);
				if (distance < radiusSum)
				{
					GetCollisionPoint(ball1, ball2);
					SetCollisionPointGraphicMarker();
					CalculateVelocityAfterCollision(ball1, ball2);
					EulerMoveBallsAfterCollision(dt, ball1, ball2);
				}
			}
		}

		private bool IsCollision(ICircle ball1, ICircle ball2) =>
			SubX(ball1, ball2) + SumRadius(ball1, ball2) > Zero &&
			SubX(ball2, ball1) + SumRadius(ball1, ball2) > Zero &&
			SubY(ball1, ball2) + SumRadius(ball1, ball2) > Zero &&
			SubY(ball2, ball1) + SumRadius(ball1, ball2) > Zero;

		private double GetDistance(ICircle ball1, ICircle ball2)
        {
            var x = SubX(ball1, ball2);
            var y = SubY(ball1, ball2);
			return Math.Sqrt(x * x + y * y);
        }

		private void EulerMoveBallsAfterCollision(double dt, ICircle ball1, ICircle ball2)
		{
			ball1.MassCenter += new Vector2(ball1.Velocity.X * dt, ball1.Velocity.Y * dt);
			ball2.MassCenter += new Vector2(ball2.Velocity.X * dt, ball2.Velocity.Y * dt);
		}

		private void GetCollisionPoint(ICircle ball1, ICircle ball2)
		{
			collisionPointX =
				MultiplayXR(ball1, ball2) +
				MultiplayXR(ball2, ball1) / radiusSum;
			collisionPointY =
				MultiplayYR(ball1, ball2) +
				MultiplayYR(ball2, ball1) / radiusSum;
		}

		private double MultiplayXR(ICircle ball1, ICircle ball2) =>
			ball1.MassCenter.X * ball2.Radius;

		private double MultiplayYR(ICircle ball1, ICircle ball2) =>
			ball1.MassCenter.Y * ball2.Radius;

		private void CalculateVelocityAfterCollision(ICircle ball1, ICircle ball2)
		{
			double newV1X = (ball1.Velocity.X * MassDiff(ball1, ball2) + MassTimesVelocityX1(ball1, ball2)) / MassSum(ball1, ball2);
			double newV1Y = (ball1.Velocity.Y * MassDiff(ball1, ball2) + MassTimesVelocityY2(ball1, ball2)) / MassSum(ball1, ball2);
			double newV2X = (ball2.Velocity.X * MassDiff(ball1, ball2) + MassTimesVelocityX2(ball1, ball2)) / MassSum(ball1, ball2);
			double newV2Y = (ball2.Velocity.Y * MassDiff(ball1, ball2) + MassTimesVelocityY1(ball1, ball2)) / MassSum(ball1, ball2);
			ball1.Velocity = new Vector2(newV1X, newV1Y);
			ball2.Velocity = new Vector2(newV2X, newV2Y);
		}

		private double MassTimesVelocityX1(ICircle ball1, ICircle ball2) => 2 * ball1.Mass * ball1.Velocity.X;

		private double MassTimesVelocityX2(ICircle ball1, ICircle ball2) => 2 * ball2.Mass * ball2.Velocity.X;

		private double MassTimesVelocityY1(ICircle ball1, ICircle ball2) => 2 * ball1.Mass * ball1.Velocity.Y;

		private double MassTimesVelocityY2(ICircle ball1, ICircle ball2) => 2 * ball2.Mass * ball2.Velocity.Y;

		private double MassDiff(ICircle ball1, ICircle ball2) =>
			ball1.Mass - ball2.Mass;

		private double MassSum(ICircle ball1, ICircle ball2) =>
			ball1.Mass + ball2.Mass;

		private void SetCollisionPointGraphicMarker()
		{
			var circle = shapeFactory.GetCollisionPointMarker(collisionPointX, collisionPointY);
			gameData?.VectorAnalitics?.Add(circle);
		}

		private static double SubX(ICircle ball1, ICircle ball2) =>
			ball1.MassCenter.X - ball2.MassCenter.X;

		private static double SubY(ICircle ball1, ICircle ball2) =>
			ball1.MassCenter.Y - ball2.MassCenter.Y;

		private double SumRadius(ICircle ball1, ICircle ball2) =>
			ball1.Radius + ball2.Radius;
	}