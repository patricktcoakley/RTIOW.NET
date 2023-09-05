using System.Numerics;
using RTIOW.Canvas;
using RTIOW.Hittable;
using RTIOW.Material;
using RTIOW.Math;

namespace RTIOW;

internal static class Program
{
    private const int ImageWidth = 1200;
    private const float AspectRatio = 3.0f / 2.0f;
    private const int ImageHeight = (int)(ImageWidth / AspectRatio);
    private const int SamplesPerPixel = 500;
    private const int MaxDepth = 50;
    private const float FocusDistance = 10.0f;
    private const float Aperture = 0.1f;
    private static readonly HittableList World = RandomScene();

    public static void Main(string[] args)
    {
        var lookFrom = new Vector3(13.0f, 2.0f, 3.0f);
        var lookAt = new Vector3(0.0f, 0.0f, 0.0f);
        var verticalUp = new Vector3(0.0f, 1.0f, 0.0f);
        var camera = new Camera(
            lookFrom,
            lookAt,
            verticalUp,
            20.0f,
            AspectRatio,
            Aperture,
            FocusDistance);

        using var ppmFile = new PpmCanvas(ImageWidth, ImageHeight);

        var pixels = new Vector3[ImageWidth, ImageHeight];
        Parallel.For(0, ImageWidth, x =>
        {
            for (var y = ImageHeight - 1; y >= 0; --y)
            {
                var pixel = SamplePixel(x, y, camera);
                pixels[x, y] = pixel;
            }
        });

        for (var y = ImageHeight - 1; y >= 0; --y)
        {
            for (var x = 0; x < ImageWidth; ++x)
            {
                ppmFile.WriteColor(pixels[x, y].ToColor(SamplesPerPixel));
            }
        }
    }


    private static Vector3 SamplePixel(
        int x,
        int y,
        Camera camera
    )
    {
        var pixel = new Vector3();
        var hr = new HitRecord();
        for (var s = SamplesPerPixel; s > 0; s--)
        {
            var u = (x + Random.Shared.NextSingle()) / (ImageWidth - 1);
            var v = (y + Random.Shared.NextSingle()) / (ImageHeight - 1);
            var ray = camera.Ray(u, v);
            pixel += HitColor(ray, World, MaxDepth, ref hr);
        }

        return pixel;
    }

    private static Vector3 HitColor(Ray ray, IHittable hittable, int depth, ref HitRecord hr)
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
                return attenuation * HitColor(scattered, World, depth - 1, ref hr);
            }

            return Vector3.Zero;
        }

        var unitDirection = Vector3.Normalize(ray.Direction);
        var t = 0.5f * (unitDirection.Y + 1.0f);
        return (1.0f - t) * Vector3.One + t * new Vector3(0.5f, 0.7f, 1.0f);
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

        var dielectric = new Dielectric(1.5f);
        world.Add(new Sphere(new Vector3(0.0f, 1.0f, 0.0f), 1.0f, dielectric));

        var lambertian = new Lambertian(new Vector3(0.4f, 0.2f, 0.1f));
        world.Add(new Sphere(new Vector3(-4.0f, 1.0f, 0.0f), 1.0f, lambertian));

        var metal = new Metal(new Vector3(0.7f, 0.6f, 0.5f), 0.0f);
        world.Add(new Sphere(new Vector3(4.0f, 1.0f, 0.0f), 1.0f, metal));

        return world;
    }
}
