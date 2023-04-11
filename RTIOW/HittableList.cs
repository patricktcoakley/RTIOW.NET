namespace RTIOW;

public class HittableList : IHittable
{
    private readonly List<IHittable> _objects = new();

    public HittableList(params IHittable[] objects)
    {
        _objects.AddRange(objects);
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
}
