using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtentionMethods
{
    public static void Shuffle<T>(this IList<T> list)
    {
        //https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle
        System.Random rand = new System.Random();

        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rand.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }


    public static Vector2 Copy(this Vector2 vec)
    {
        return new Vector2(vec.x, vec.y);
    }

    public static Vector3 Copy(this Vector3 vec)
    {
        return new Vector3(vec.x, vec.y, vec.z);
    }

    public static Quaternion Copy(this Quaternion quaternion)
    {
        return new Quaternion(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
    }


    public static Vector2 RadianToVector2(this Vector2 vec, float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }

    public static Vector2 DegreeToVector2(this Vector2 vec, float degree)
    {
        return RadianToVector2(vec, degree * Mathf.Deg2Rad);
    }
}
