using System.Numerics;
using RTIOW.Hittable;

namespace RTIOW.Material;

public record Metal(Vector3 Albedo, float Fuzziness) : IMaterial
{
    public bool Scatter(in Ray ray, in HitRecord hr, ref Vector3 attenuation, ref Ray scattered)
    {
        var reflected = Vector3Extensions.Reflect(
            Vector3.Normalize(ray.Direction),
            hr.Normal
        );

        scattered = new Ray(hr.Point, reflected + Fuzziness * Vector3Extensions.RandomInUnitSphere());
        attenuation = Albedo;
        return Vector3.Dot(scattered.Direction, hr.Normal) > 0;
    }
}
