using System.Numerics;
using RTIOW.Math;
using static RTIOW.Math.Vector3Extensions;

namespace RTIOW;

public record Camera
{
    private readonly Vector3 _horizontal;
    private readonly float _lensRadius;
    private readonly Vector3 _lowerLeftCorner;
    private readonly Vector3 _origin;
    private readonly Vector3 _u;
    private readonly Vector3 _v;
    private readonly Vector3 _vertical;
    private readonly Vector3 _w;


    public Camera(
        Vector3 lookFrom,
        Vector3 lookAt,
        Vector3 verticalUp,
        float verticalFieldOfView,
        float aspectRatio,
        float aperture,
        float focusDistance
    )
    {
        const float focalLength = 1.0f;
        var theta = DegreesToRadians(verticalFieldOfView);
        var h = MathF.Tan(theta / 2.0f);
        var viewportHeight = 2.0f * h;
        var viewportWidth = aspectRatio * viewportHeight;

        _w = Vector3.Normalize(lookFrom - lookAt);
        _u = Vector3.Normalize(Vector3.Cross(verticalUp, _w));
        _v = Vector3.Cross(_w, _u);
        _origin = lookFrom;
        _horizontal = focusDistance * viewportWidth * _u;
        _vertical = focusDistance * viewportHeight * _v;
        _lowerLeftCorner = _origin - _horizontal / 2 - _vertical / 2 - focusDistance * _w;
        _lensRadius = aperture / 2.0f;
    }

    public Ray Ray(float s, float t)
    {
        var rd = _lensRadius * RandomInUnitDisk();
        var offset = _u * rd.X + _v * rd.Y;
        return new Ray(_origin + offset, _lowerLeftCorner + s * _horizontal + t * _vertical - _origin - offset);
    }

    private static float DegreesToRadians(float degrees) => degrees * MathF.PI / 180.0f;
}
