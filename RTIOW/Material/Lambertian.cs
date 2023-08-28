using System.Numerics;

namespace RTIOW;

public record Lambertian(Vector3 Albedo) : IMaterial
{
    public bool Scatter(Ray ray, HitRecord hr, ref Vector3 attenuation, ref Ray scattered)
    {
        var direction = hr.Normal + Vector3Extensions.RandomUnitVector();
        if (direction.NearZero())
        {
            direction = hr.Normal;
        }

        scattered = new Ray(hr.Point, direction);
        attenuation = Albedo;
        return true;
    }
}