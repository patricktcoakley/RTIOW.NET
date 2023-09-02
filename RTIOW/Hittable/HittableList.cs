using RTIOW.Math;

namespace RTIOW.Hittable;

public class HittableList : IHittable
{
    private readonly List<IHittable> _objects;

    public HittableList(params IHittable[] objects)
    {
        _objects = new List<IHittable>(objects);
    }

    public bool Hit(Ray ray, float tMin, float tMax, ref HitRecord hitRecord)
    {
        var tempHitRecord = new HitRecord();
        var hitAnything = false;
        var closestSoFar = tMax;

        foreach (var obj in _objects)
        {
            if (!obj.Hit(ray, tMin, closestSoFar, ref tempHitRecord))
            {
                continue;
            }

            hitAnything = true;
            closestSoFar = tempHitRecord.T;
            hitRecord = tempHitRecord;
        }

        return hitAnything;
    }

    public void Clear() => _objects.Clear();
    public void Add(IHittable hittable) => _objects.Add(hittable);
    public void AddRange(ICollection<IHittable> hittables) => _objects.AddRange(hittables);
}
