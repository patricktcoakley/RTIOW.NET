using System.Numerics;

namespace RTIOW;

public class HitRecord
{
    public Vector3 Point { get; set; }
    public Vector3 Normal { get; set; }
    public float T { get; set; }
    public bool FrontFace { get; set; }

    public void SetFaceNormal(Ray ray, Vector3 OutwardNormal)
    {
        FrontFace = Vector3.Dot(ray.Direction, OutwardNormal) < 0;
        Normal = FrontFace ? OutwardNormal : -OutwardNormal;
    }
}
