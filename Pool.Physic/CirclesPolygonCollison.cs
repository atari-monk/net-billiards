using System.Windows.Media;
using Sim.Core;
using Vector.Lib;

namespace Pool.Physic;

public class CirclesPolygonCollison
    : IBinaryCirclePhysicsStrategy
{
    private readonly IShapeFactory shapeFactory;

    public bool VectorInfoSwitch { get; set; } = false;

    public CirclesPolygonCollison(IShapeFactory shapeFactory) =>
        this.shapeFactory = shapeFactory;

    public void CirclePhysis(
        double dt
        , IGameData data
        , params IShape[] circles)
    {
        var ball = circles[0] as ICircle;
        var poly = circles[1] as ILine;
        ArgumentNullException.ThrowIfNull(ball);
        ArgumentNullException.ThrowIfNull(poly);

        var segV = poly.SecondPoint - poly.MassCenter;
        var segVn = segV.Normalize();
        var ptV = ball.MassCenter - poly.MassCenter;

        var projV = Vector2.DotProduct(ptV, segVn);

        var closest = VectorCompute.ClosesestPoint(poly.MassCenter, poly.SecondPoint, segV, projV);

        var distV = ball.MassCenter - closest;

        var distVn = distV.Normalize();

        var vn = Vector2.DotProduct(ball.Velocity, distVn);
        var vs = Vector2.DotProduct(ball.Velocity, segVn);
        var Vn = vn * distVn;
        var Vs = vs * segVn;

        if (distV.Magnitude < ball.Radius)
        {
            var offset = (ball.Radius - distV.Magnitude) * distVn;

            ball.MassCenter += offset;

            ball.Velocity = Vs - Vn;
        }

        if (VectorInfoSwitch
            && data.VectorAnalitics != null
            && data.VectorAnalitics.IsAddingCompleted == false)
        {
            VectorInfo(data, ball, poly, ref closest, ref distVn, ref Vn, ref Vs);
        }
    }

    private void VectorInfo(
        IGameData data, ICircle ball, ILine poly,
        ref Vector2 closest, ref Vector2 distVn, ref Vector2 Vn,
        ref Vector2 Vs)
    {
        var line1 = shapeFactory.GetVelocityVector(
            ball.MassCenter,
            ball.MassCenter + ball.Velocity);
        data.VectorAnalitics?.Add(line1);
        var line2 = shapeFactory.GetVelocityVector(
            ball.MassCenter,
            poly.MassCenter,
            Colors.Purple);
        data.VectorAnalitics?.Add(line2);
        var pointMarker1 = shapeFactory.GetCircleMarker(
            closest,
            Colors.Red);
        data.VectorAnalitics?.Add(pointMarker1);
        var line3 = shapeFactory.GetVelocityVector(
            ball.MassCenter,
            closest,
            Colors.GreenYellow);
        data.VectorAnalitics?.Add(line3);
        var line4 = shapeFactory.GetVelocityVector(
            ball.MassCenter,
            poly.MassCenter + distVn * 100,
            Colors.HotPink);
        data.VectorAnalitics?.Add(line4);
        var line5 = shapeFactory.GetVelocityVector(
            ball.MassCenter,
            ball.MassCenter + Vn,
            Colors.Red);
        data.VectorAnalitics?.Add(line5);
        var line6 = shapeFactory.GetVelocityVector(
            ball.MassCenter,
            ball.MassCenter + Vs,
            Colors.Gold);
        data.VectorAnalitics?.Add(line6);
    }
}