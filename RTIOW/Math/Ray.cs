using System.Numerics;

namespace RTIOW.Math;

public readonly ref struct Ray(Vector3 origin, Vector3 direction)
{
    public Vector3 At(float t) => Origin + t * Direction;
    public Vector3 Direction { get; } = direction;
    public Vector3 Origin { get; } = origin;
}
