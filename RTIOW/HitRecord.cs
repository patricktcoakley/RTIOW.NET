using System.Numerics;

namespace RTIOW;

public class HitRecord
{
    public Vector3 Point { get; set; }
    public Vector3 Normal { get; set; }
    public float T { get; set; }
    public bool FrontFace { get; set; }

    public void SetFaceNormal(Ray ray, Vector3 outwardNormal)
    {
        FrontFace = Vector3.Dot(ray.Direction, outwardNormal) < 0;
        Normal = FrontFace ? outwardNormal : -outwardNormal;
    }
}
