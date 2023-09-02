using System.Numerics;
using RTIOW.Hittable;
using RTIOW.Math;

namespace RTIOW.Material;

public interface IMaterial
{
    public bool Scatter(Ray ray, HitRecord hr, ref Vector3 attenuation, ref Ray scattered);
}
