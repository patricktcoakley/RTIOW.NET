using System.Numerics;

namespace RTIOW;

public class HitRecord
{
    private bool _frontFace;

    public Vector3 Point { get; set; }
    public Vector3 Normal { get; set; }
    public float T { get; set; }
    public IMaterial? Material { get; set; }

    public void SetFaceNormal(Ray ray, Vector3 outwardNormal)
    {
        _frontFace = Vector3.Dot(ray.Direction, outwardNormal) < 0;
        Normal = _frontFace ? outwardNormal : -outwardNormal;
    }
}
