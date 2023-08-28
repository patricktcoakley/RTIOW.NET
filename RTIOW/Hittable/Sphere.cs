using System.Numerics;

namespace RTIOW;

public record Sphere(Vector3 Center, float Radius, IMaterial Material) : IHittable
{
    public Sphere(float centerX, float centerY, float centerZ, float radius, IMaterial material) : this(
        new Vector3(centerX, centerY, centerZ), radius, material)
    {
    }

    public bool Hit(Ray ray, float tMin, float tMax, ref HitRecord hitRecord)
    {
        var originCenter = ray.Origin - Center;
        var a = ray.Direction.LengthSquared();
        var halfB = Vector3.Dot(originCenter, ray.Direction);
        var c = originCenter.LengthSquared() - Radius * Radius;
        var discriminant = halfB * halfB - a * c;

        if (discriminant < 0)
        {
            return false;
        }

        var sqrtD = float.Sqrt(discriminant);
        var root = (-halfB - sqrtD) / a;
        if (root < tMin || tMax < root)
        {
            root = (-halfB + sqrtD) / a;
            if (root < tMin || tMax < root)
            {
                return false;
            }
        }

        hitRecord.T = root;
        hitRecord.Point = ray.At(hitRecord.T);
        var outwardNormal = (hitRecord.Point - Center) / Radius;
        hitRecord.SetFaceNormal(ray, outwardNormal);
        hitRecord.Material = Material;

        return true;
    }
}
