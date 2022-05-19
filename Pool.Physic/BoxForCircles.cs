using Sim.Core;
using Vector.Lib;

namespace Pool.Physic;

public class BoxForCircles
    : IUnaryCirclePhysicsStrategy
{
    private const double WallX = 900.0;

    private const double WallY = WallX;

    private const double Zero = 0.0;

    private const double Inverse = -1.0;

    private ICircle? circle;

    public void CirclePhysis(
        IShape circle
        , double frameDeltaTime
        , IGameData gameData)
    {
        this.circle = (ICircle)circle;
        if (IsVelocityXPositive() &&
            IsWallHitFromRight())
            InvertVelocityX();
        if (IsVelocityXNegative() &&
            IsWallHitFromLeft())
            InvertVelocityX();
        if (IsVelocityYPositive() &&
            IsWallHitFromBottom())
            InvertVelocityY();
        if (IsVelocityYNegative() &&
            IsWallHitFromTop())
            InvertVelocityY();
    }

    private bool IsVelocityXPositive() => circle?.Velocity.X > Zero;

    private bool IsWallHitFromRight() => circle?.MassCenter.X + circle?.Radius >= WallX;

    private void InvertVelocityX()
    {
        ArgumentNullException.ThrowIfNull(circle);
        circle.Velocity =
            new Vector2(circle.Velocity.X * Inverse
                , circle.Velocity.Y * Inverse);
    }

    private bool IsVelocityXNegative() => circle?.Velocity.X < Zero;

    private bool IsWallHitFromLeft() => circle?.MassCenter.X <= 2 * circle?.Radius;

    private bool IsVelocityYPositive() => circle?.Velocity.Y > Zero;

    private bool IsWallHitFromBottom() => circle?.MassCenter.Y + circle?.Radius >= WallY;

    private void InvertVelocityY()
    {
        ArgumentNullException.ThrowIfNull(circle);
        circle.Velocity = new Vector2(
            circle.Velocity.X
            , circle.Velocity.Y * Inverse);
    }

    private bool IsVelocityYNegative()
    {
        return circle?.Velocity.Y < Zero;
    }

    private bool IsWallHitFromTop()
    {
        return circle?.MassCenter.Y <= 2 * circle?.Radius;
    }
}