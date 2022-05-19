using Sim.Core;
using Vector.Lib;

namespace Pool.Physic;

public class CirclesCollisonDuringFrame
    : ICirclesCollisonDuringFrame
{
    public event Action<string>? WhiteBallFirstCollisionEvent;

    private Action<string>? whiteBallFirstCollisionHandler;

    private ICircle? ball1;
    private ICircle? ball2;
    private Vector2 massCenter12Diff;
    private double distanceAtFrameEnd;
    private double collisionDistance;
    private double dt;
    private double millisecondsAfterCollision;
    private Vector2 collisionPlaneNormal;
    private Vector2 collisionPlane;
    private double ball1VelocityRespectiveToNormal;
    private double ball1VelocityRespectiveToPlane;
    private double ball2VelocityRespectiveToNormal;
    private double ball2VelocityRespectiveToPlane;
    private double ball1ScalarVelocityAfterCollision;
    private double ball2ScalarVelocityAfterCollision;
    private Vector2 ball2VelocityNormal;
    private Vector2 ball2VelovityPlane;
    private Vector2 ball1VelocityNormal;
    private Vector2 ball1VelovityPlane;

    private const string BallWhite = "white";

    public void SetWhiteBallFirstCollisionHandler(
        Action<string> whiteBallFirstCollisionHandler)
    {
        this.whiteBallFirstCollisionHandler = whiteBallFirstCollisionHandler;
        WhiteBallFirstCollisionEvent += this.whiteBallFirstCollisionHandler;
    }

    public void CirclePhysis(
        double dt
        , IGameData gameData
        , params IShape[] circles)
    {
        InitializeFields(circles, dt);
        CalculateCollisionParameters();
        if (IsDistanceLessThenRadiusSum())
        {
            MoveBallsExactlyToCollisionPointInFrame();
            CreateProjectionVectors();
            DotProductVelocityAndProjection();
            CalculateScalarVelocitiesAfterCollision();
            CalculateVectorVelocitiesAfterCollision();
            FinalVelocitiesAfterCollision();
            MoveAfterCollisionWithVelecitiesAfterCollision();
            InvokeWhiteBallFirstCollision();
        }
    }

    private void InitializeFields(IShape[] circles, double dt)
    {
        ball1 = circles[0] as ICircle;
        ball2 = circles[1] as ICircle;
        this.dt = dt;
    }

    private void CalculateCollisionParameters()
    {
        ArgumentNullException.ThrowIfNull(ball1);
        ArgumentNullException.ThrowIfNull(ball2);
        massCenter12Diff = ball1.MassCenter - ball2.MassCenter;
        distanceAtFrameEnd = massCenter12Diff.Magnitude;
        collisionDistance = ball1.Radius + ball2.Radius;
    }

    private bool IsDistanceLessThenRadiusSum() => distanceAtFrameEnd < collisionDistance;

    private void MoveBallsExactlyToCollisionPointInFrame()
    {
        ArgumentNullException.ThrowIfNull(ball1);
        ArgumentNullException.ThrowIfNull(ball2);
        var ball1PositionAtFrameStart = new Vector2(
            ball1.MassCenter.X - ball1.Velocity.X * dt,
            ball1.MassCenter.Y - ball1.Velocity.Y * dt);
        var ball2PositionAtFrameStart = new Vector2(
            ball2.MassCenter.X - ball2.Velocity.X * dt,
            ball2.MassCenter.Y - ball2.Velocity.Y * dt);
        var vectorBalls21AtFrameStart = ball2PositionAtFrameStart - ball1PositionAtFrameStart;
        var ballsDistanceAtFrameStart = vectorBalls21AtFrameStart.Magnitude;
        var distanceFrameDelta = distanceAtFrameEnd - ballsDistanceAtFrameStart;
        var distanceToCollision = collisionDistance - ballsDistanceAtFrameStart;
        var percentageDeltaToCollision = distanceToCollision / distanceFrameDelta;
        var percentageDeltaAfterCollision = 1 - percentageDeltaToCollision;
        var millisecondsToCollision = dt * percentageDeltaToCollision;
        millisecondsAfterCollision = dt * percentageDeltaAfterCollision;
        ball1.MassCenter = new Vector2(ball1PositionAtFrameStart.X + ball1.Velocity.X * millisecondsToCollision,
            ball1PositionAtFrameStart.Y + ball1.Velocity.Y * millisecondsToCollision);
        ball2.MassCenter = new Vector2(ball2PositionAtFrameStart.X + ball2.Velocity.X * millisecondsToCollision,
            ball2PositionAtFrameStart.Y + ball2.Velocity.Y * millisecondsToCollision);
    }

    private void CreateProjectionVectors()
    {
        collisionPlaneNormal = new Vector2(
                                    massCenter12Diff.X,
                                    massCenter12Diff.Y).Normalize();
        collisionPlane = new Vector2(
            -collisionPlaneNormal.Y
            , collisionPlaneNormal.X);
    }

    private void DotProductVelocityAndProjection()
    {
        ArgumentNullException.ThrowIfNull(ball1);
        ArgumentNullException.ThrowIfNull(ball2);
        ball1VelocityRespectiveToNormal = Vector2.DotProduct(collisionPlaneNormal, ball1.Velocity);
        ball1VelocityRespectiveToPlane = Vector2.DotProduct(collisionPlane, ball1.Velocity);
        ball2VelocityRespectiveToNormal = Vector2.DotProduct(collisionPlaneNormal, ball2.Velocity);
        ball2VelocityRespectiveToPlane = Vector2.DotProduct(collisionPlane, ball2.Velocity);
    }

    private void CalculateScalarVelocitiesAfterCollision()
    {
        ArgumentNullException.ThrowIfNull(ball1);
        ArgumentNullException.ThrowIfNull(ball2);
        ball1ScalarVelocityAfterCollision = ball1VelocityRespectiveToNormal * (ball1.Mass - ball2.Mass) +
                            2 * ball2.Mass * ball2VelocityRespectiveToNormal / (ball2.Mass + ball1.Mass);
        ball2ScalarVelocityAfterCollision = ball2VelocityRespectiveToNormal * (ball2.Mass - ball1.Mass) +
            2 * ball1.Mass * ball1VelocityRespectiveToNormal / (ball2.Mass + ball1.Mass);
    }

    private void CalculateVectorVelocitiesAfterCollision()
    {
        ball1VelocityNormal = ball1ScalarVelocityAfterCollision * collisionPlaneNormal;
        ball1VelovityPlane = ball1VelocityRespectiveToPlane * collisionPlane;
        ball2VelocityNormal = ball2ScalarVelocityAfterCollision * collisionPlaneNormal;
        ball2VelovityPlane = ball2VelocityRespectiveToPlane * collisionPlane;
    }

    private void FinalVelocitiesAfterCollision()
    {
        ArgumentNullException.ThrowIfNull(ball1);
        ArgumentNullException.ThrowIfNull(ball2);
        ball1.Velocity = ball1VelocityNormal + ball1VelovityPlane;
        ball2.Velocity = ball2VelocityNormal + ball2VelovityPlane;
    }

    private void MoveAfterCollisionWithVelecitiesAfterCollision()
    {
        ArgumentNullException.ThrowIfNull(ball1);
        ArgumentNullException.ThrowIfNull(ball2);
        ball1.MassCenter += ball1.Velocity * millisecondsAfterCollision;
        ball2.MassCenter += ball2.Velocity * millisecondsAfterCollision;
    }

    private void InvokeWhiteBallFirstCollision()
    {
        ArgumentNullException.ThrowIfNull(ball1);
        ArgumentNullException.ThrowIfNull(ball2);
        ArgumentNullException.ThrowIfNull(ball1.TextFlag);
        ArgumentNullException.ThrowIfNull(ball2.TextFlag);
        if (WhiteBallFirstCollisionEvent == null) return;
        if (ball1.TextFlag == BallWhite)
            WhiteBallFirstCollisionEvent.Invoke(ball2.TextFlag);
        else if (ball2.TextFlag == BallWhite)
            WhiteBallFirstCollisionEvent.Invoke(ball1.TextFlag);
        WhiteBallFirstCollisionEvent -= whiteBallFirstCollisionHandler;
    }
}