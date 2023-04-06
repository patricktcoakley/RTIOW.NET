using System.Drawing;
using System.Numerics;

namespace RTIOW;

public static class VectorExtensions
{
    public static Color ToColor(this Vector3 v)
    {
        return Color.FromArgb((int)(255.999f * v.X), (int)(255.999f * v.Y), (int)(255.999f * v.Z));
    }
}