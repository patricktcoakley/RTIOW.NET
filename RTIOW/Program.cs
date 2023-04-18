﻿using System.Numerics;
using RTIOW;

const float aspectRatio = 16.0f / 9.0f;
const int imageWidth = 400;
const int imageHeight = (int)(imageWidth / aspectRatio);
const int samplesPerPixel = 100;
const int maxDepth = 50;

var world = new HittableList();

var ground = new Lambertian(new Vector3(0.8f, 0.8f, 0.0f));
var center = new Lambertian(new Vector3(0.7f, 0.3f, 0.3f));
var left = new Metal(new Vector3(0.8f), 0.3f);
var right = new Metal(new Vector3(0.8f, 0.6f, 0.0f), 1.0f);

world.Add(new Sphere(0.0f, -100.5f, -1.0f, 100.0f, ground));
world.Add(new Sphere(0.0f, 0f, -1.0f, 0.5f, center));
world.Add(new Sphere(-1.0f, 0f, -1.0f, 0.5f, left));
world.Add(new Sphere(1.0f, 0f, -1.0f, 0.5f, right));

var camera = new Camera();

using var ppmFile = new PpmCanvas(imageWidth, imageHeight);

for (var y = imageHeight - 1; y >= 0; --y)
{
    for (var x = 0; x < imageWidth; ++x)
    {
        var pixel = new Vector3();
        for (var s = 0; s < samplesPerPixel; ++s)
        {
            var u = (x + Random.Shared.NextSingle()) / (imageWidth - 1);
            var v = (y + Random.Shared.NextSingle()) / (imageHeight - 1);
            var ray = camera.Ray(u, v);
            pixel += HitColor(ray, world, maxDepth);
        }

        ppmFile.WriteColor(pixel.ToColor(samplesPerPixel));
    }
}

Vector3 HitColor(Ray ray, IHittable hittable, int depth)
{
    if (depth <= 0)
    {
        return Vector3.Zero;
    }

    var hr = new HitRecord();
    if (hittable.Hit(ray, 0.001f, float.PositiveInfinity, ref hr))
    {
        var scattered = new Ray();
        var attenuation = Vector3.Zero;
        if (hr.Material.Scatter(ray, hr, ref attenuation, ref scattered))
        {
            return attenuation * HitColor(scattered, world, depth - 1);
        }

        return Vector3.Zero;
    }

    var unitDirection = Vector3.Normalize(ray.Direction);
    var t = 0.5f * (unitDirection.Y + 1.0f);
    return (1.0f - t) * Vector3.One + t * new Vector3(0.5f, 0.7f, 1.0f);
}
