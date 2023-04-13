using System.Drawing;
using System.Numerics;

namespace RTIOW;

public static class VectorExtensions
{
    public static Color ToColor(this Vector3 v, int samplesPerPixel = 100)
    {
        var scale = 1.0f / samplesPerPixel;
        return Color.FromArgb(
            (int)(256 * Single.Clamp(v.X * scale, 0.0f, 0.999f)),
            (int)(256 * Single.Clamp(v.Y * scale, 0.0f, 0.999f)),
            (int)(256 * Single.Clamp(v.Z * scale, 0.0f, 0.999f)));
    }
}
