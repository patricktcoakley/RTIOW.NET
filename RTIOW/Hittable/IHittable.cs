using RTIOW.Math;

namespace RTIOW.Hittable;

public interface IHittable
{
    bool Hit(Ray ray, float tMin, float tMax, ref HitRecord hitRecord);
}
