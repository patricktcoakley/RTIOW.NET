using System.Numerics;
using RTIOW.Math;

namespace RTIOW;

public record Camera
{
    private readonly Vector3 _horizontal;
    private readonly Vector3 _lowerLeftCorner;
    private readonly Vector3 _origin;
    private readonly Vector3 _vertical;

    public Camera(
        Vector3 lookFrom,
        Vector3 lookAt,
        Vector3 verticalUp,
        float verticalFieldOfView,
        float aspectRatio)
    {
        const float focalLength = 1.0f;
        var theta = DegreesToRadians(verticalFieldOfView);
        var h = MathF.Tan(theta / 2.0f);
        var viewportHeight = 2.0f * h;
        var viewportWidth = aspectRatio * viewportHeight;
        var w = Vector3.Normalize(lookFrom - lookAt);
        var u = Vector3.Normalize(Vector3.Cross(verticalUp, w));
        var v = Vector3.Cross(w, u);

        _origin = lookFrom;
        _horizontal = viewportWidth * u;
        _vertical = viewportHeight * v;
        _lowerLeftCorner = _origin - _horizontal / 2 - _vertical / 2 - w;
    }

    public Ray Ray(float s, float t) =>
        new(_origin, _lowerLeftCorner + s * _horizontal + t * _vertical - _origin);

    private static float DegreesToRadians(float degrees) => degrees * MathF.PI / 180.0f;
}
