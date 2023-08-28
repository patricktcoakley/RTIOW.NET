using System.Numerics;

namespace RTIOW;

public interface IMaterial
{
    public bool Scatter(Ray ray, HitRecord hr, ref Vector3 attenuation, ref Ray scattered);
}
