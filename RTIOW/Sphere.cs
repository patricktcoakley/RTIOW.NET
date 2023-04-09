using System.Numerics;

namespace RTIOW;

public class Sphere : IHittable
{
    public Sphere(float centerX = 0.0f, float centerY = 0.0f, float centerZ = 0.0f, float radius = 0.0f) : this(
        new Vector3(centerX, centerY, centerZ), radius)
    {
    }

    public Sphere(Vector3 center, float radius)
    {
        Center = center;
        Radius = radius;
    }

    public Vector3 Center { get; set; }
    public float Radius { get; set; }


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

        return true;
    }
}
