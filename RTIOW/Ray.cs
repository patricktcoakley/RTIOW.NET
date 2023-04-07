using System.Drawing;
using System.Numerics;

namespace RTIOW;

public readonly record struct Ray(Vector3 Origin = new(), Vector3 Direction = new())
{
    public Color Color
    {
        get
        {
            if (HitSphere(new Vector3 { Z = -1.0f }, 0.5f))
            {
                return Color.Red;
            }

            var unitDirection = Vector3.Normalize(Direction);
            var t = 0.5f * (unitDirection.Y + 1.0f);
            var color = (1.0f - t) * new Vector3(1.0f, 1.0f, 1.0f) + t * new Vector3(0.5f, 0.7f, 1.0f);
            return color.ToColor();
        }
    }

    public Vector3 At(float t) => Origin + t * Direction;

    private bool HitSphere(Vector3 center, float radius)
    {
        var originCenter = Origin - center;
        var a = Vector3.Dot(Direction, Direction);
        var b = 2.0f * Vector3.Dot(originCenter, Direction);
        var c = Vector3.Dot(originCenter, originCenter) - radius * radius;
        var discriminant = b * b - 4 * a * c;
        return discriminant > 0;
    }
}
