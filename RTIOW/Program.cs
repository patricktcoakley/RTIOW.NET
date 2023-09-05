using System.Numerics;
using RTIOW.Canvas;
using RTIOW.Hittable;
using RTIOW.Material;
using RTIOW.Math;

namespace RTIOW;

internal static class Program
{
    public static void Main(string[] args)
    {
        const float aspectRatio = 3.0f / 2.0f;
        const int imageWidth = 400;
        const int imageHeight = (int)(imageWidth / aspectRatio);
        const int samplesPerPixel = 100;
        const int maxDepth = 50;

        var world = RandomScene();

        var lookFrom = new Vector3(13.0f, 2.0f, 3.0f);
        var lookAt = new Vector3(0.0f, 0.0f, 0.0f);
        var verticalUp = new Vector3(0.0f, 1.0f, 0.0f);
        const float focusDistance = 10.0f;
        const float aperture = 0.1f;
        var camera = new Camera(
            lookFrom,
            lookAt,
            verticalUp,
            20.0f,
            aspectRatio,
            aperture,
            focusDistance);

        using var ppmFile = new PpmCanvas(imageWidth, imageHeight);

        for (var y = imageHeight - 1; y >= 0; --y)
        {
            for (var x = 0; x < imageWidth; ++x)
            {
                var pixel = new Vector3();
                var hr = new HitRecord();
                for (var s = samplesPerPixel; s > 0; s--)
                {
                    var u = (x + Random.Shared.NextSingle()) / (imageWidth - 1);
                    var v = (y + Random.Shared.NextSingle()) / (imageHeight - 1);
                    var ray = camera.Ray(u, v);
                    pixel += HitColor(ray, world, maxDepth, ref hr);
                }

                ppmFile.WriteColor(pixel.ToColor(samplesPerPixel));
            }
        }

        return;

        Vector3 HitColor(Ray ray, IHittable hittable, int depth, ref HitRecord hr)
        {
            if (depth <= 0)
            {
                return Vector3.Zero;
            }

            if (hittable.Hit(ray, 0.001f, float.PositiveInfinity, ref hr))
            {
                var scattered = new Ray();
                var attenuation = Vector3.Zero;
                if (hr.Material!.Scatter(ray, hr, ref attenuation, ref scattered))
                {
                    return attenuation * HitColor(scattered, world, depth - 1, ref hr);
                }

                return Vector3.Zero;
            }

            var unitDirection = Vector3.Normalize(ray.Direction);
            var t = 0.5f * (unitDirection.Y + 1.0f);
            return (1.0f - t) * Vector3.One + t * new Vector3(0.5f, 0.7f, 1.0f);
        }
    }

    private static HittableList RandomScene()
    {
        var world = new HittableList();
        var ground = new Lambertian(new Vector3(0.5f, 0.5f, 0.5f));
        world.Add(new Sphere(new Vector3(0.0f, -1000.0f, 0.0f), 1000.0f, ground));


        for (var a = -11; a < 11; a++)
        {
            for (var b = -11; b < 11; b++)
            {
                var chooseMat = Random.Shared.NextSingle();
                var center = new Vector3((float)(a + 0.9 * Random.Shared.NextSingle()), 0.2f, b + 0.9f * Random.Shared.NextSingle());

                if ((center - new Vector3(4.0f, 0.2f, 0.0f)).Length() > 0.9f)
                {
                    IMaterial sphereMaterial;

                    if (chooseMat < 0.8f)
                    {
                        var albedo = Vector3Extensions.Random() * Vector3Extensions.Random();
                        sphereMaterial = new Lambertian(albedo);
                        world.Add(new Sphere(center, 0.2f, sphereMaterial));
                    }
                    else if (chooseMat < 0.95)
                    {
                        var albedo = Vector3Extensions.Random(0.5f, 1.0f);
                        var fuzz = Random.Shared.NextSingle(0.0f, 0.5f);
                        sphereMaterial = new Metal(albedo, fuzz);
                        world.Add(new Sphere(center, 0.2f, sphereMaterial));
                    }
                    else
                    {
                        sphereMaterial = new Dielectric(1.5f);
                        world.Add(new Sphere(center, 0.2f, sphereMaterial));
                    }
                }
            }
        }

        var material1 = new Dielectric(1.5f);
        world.Add(new Sphere(new Vector3(0.0f, 1.0f, 0.0f), 1.0f, material1));

        var material2 = new Lambertian(new Vector3(0.4f, 0.2f, 0.1f));
        world.Add(new Sphere(new Vector3(-4.0f, 1.0f, 0.0f), 1.0f, material2));

        var material3 = new Metal(new Vector3(0.7f, 0.6f, 0.5f), 0.0f);
        world.Add(new Sphere(new Vector3(4.0f, 1.0f, 0.0f), 1.0f, material3));


        return world;
    }
}
