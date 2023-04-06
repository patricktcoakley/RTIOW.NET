using System.Drawing;
using System.Numerics;

namespace RTIOW;

public readonly record struct Ray(Vector3 Origin = new(), Vector3 Direction = new())
{
    public Color Color
    {
        get
        {
            var unitDirection = Vector3.Normalize(Direction);
            var t = 0.5f * (unitDirection.Y + 1.0f);
            var color = (1.0f - t) * new Vector3(1.0f, 1.0f, 1.0f) + t * new Vector3(0.5f, 0.7f, 0.9f);
            return color.ToColor();
        }
    }

    public Vector3 At(float t) => Origin + t * Direction;
}
