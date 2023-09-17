namespace RTIOW.Hittable;

public class HittableList(IEnumerable<IHittable> objects) : IHittable
{
    private readonly ReadOnlyMemory<IHittable> _objects = objects.ToArray();

    public bool Hit(Ray ray, float tMin, float tMax, ref HitRecord hitRecord)
    {
        var tempHitRecord = new HitRecord();
        var hitAnything = false;
        var closestSoFar = tMax;

        foreach (var obj in _objects.Span)
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
}
