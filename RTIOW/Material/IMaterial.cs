using System.Numerics;
using RTIOW.Hittable;

namespace RTIOW.Material;

public interface IMaterial
{
    public bool Scatter(in Ray ray, in HitRecord hr, ref Vector3 attenuation, ref Ray scattered);
}
