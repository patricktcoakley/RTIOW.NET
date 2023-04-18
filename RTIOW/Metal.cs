using System.Numerics;
using static RTIOW.Vector3Extensions;

namespace RTIOW;

public record Metal(Vector3 Albedo, float Fuzziness) : IMaterial
{
    public bool Scatter(Ray ray, HitRecord hr, ref Vector3 attenuation, ref Ray scattered)
    {
        var reflected = Reflect(
            Vector3.Normalize(ray.Direction),
            hr.Normal
        );

        scattered = new Ray(hr.Point, reflected + Fuzziness * RandomInUnitSphere());
        attenuation = Albedo;
        return Vector3.Dot(scattered.Direction, hr.Normal) > 0;
    }
}
