namespace RTIOW;

public static class RandomExtensions
{
    public static float NextSingle(this Random r, float min, float max) => min + (max - min) * r.NextSingle();
}
