﻿using System.Numerics;

namespace RTIOW;

public readonly record struct Ray(Vector3 Origin = new(), Vector3 Direction = new())
{
    public Vector3 At(float t) => Origin + t * Direction;
}
