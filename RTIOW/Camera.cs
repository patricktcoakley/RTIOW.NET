using System.Numerics;
using RTIOW.Math;

namespace RTIOW;

public class Camera
{
    private readonly Vector3 _horizontal;
    private readonly Vector3 _lowerLeftCorner;
    private readonly Vector3 _origin;
    private readonly Vector3 _vertical;

    public Camera()
    {
        const float aspectRatio = 16.0f / 9.0f;
        const float viewportHeight = 2.0f;
        const float viewportWidth = aspectRatio * viewportHeight;
        const float focalLength = 1.0f;

        _origin = Vector3.Zero;
        _horizontal = new Vector3 { X = viewportWidth };
        _vertical = new Vector3 { Y = viewportHeight };
        _lowerLeftCorner = _origin - _horizontal / 2 - _vertical / 2 - new Vector3 { Z = focalLength };
    }

    public Ray Ray(float u, float v) =>
        new(_origin, _lowerLeftCorner + u * _horizontal + v * _vertical - _origin);
}
