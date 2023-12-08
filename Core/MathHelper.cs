using System.Numerics;

namespace Core;

public static class MathHelper
{
    public static T LowestCommonMultiplier<T>(T a, T b) where T : INumber<T>
    {
        return a / GreatestCommonDivisor(a, b) * b;
    }

    private static T GreatestCommonDivisor<T>(T a, T b) where T : INumber<T>
    {
        while (b != T.Zero)
        {
            T tmp = b;
            b = a % b;
            a = tmp;
        }

        return a;
    }
}