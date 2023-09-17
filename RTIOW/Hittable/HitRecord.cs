using System.Numerics;
using RTIOW.Material;

namespace RTIOW.Hittable;

public ref struct HitRecord
{
    public bool FrontFace { get; private set; }
    public Vector3 Point { get; set; }
    public Vector3 Normal { get; private set; }
    public float T { get; set; }
    public IMaterial? Material { get; set; }

    public void SetFaceNormal(Ray ray, Vector3 outwardNormal)
    {
        FrontFace = Vector3.Dot(ray.Direction, outwardNormal) < 0;
        Normal = FrontFace ? outwardNormal : -outwardNormal;
    }
}
