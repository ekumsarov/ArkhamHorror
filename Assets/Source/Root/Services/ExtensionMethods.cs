using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.AI;
using NUnit.Framework;

public static class ExtensionMethods
{
    #region Bounds
    public static float Right(this Bounds bounds)
    {
        return bounds.center.x + bounds.size.x / 2;
    }

    public static float Left(this Bounds bounds)
    {
        return bounds.center.x - bounds.size.x / 2;
    }

    public static float Bottom(this Bounds bounds)
    {
        return bounds.center.y - bounds.size.y / 2;
    }

    public static float Top(this Bounds bounds)
    {
        return bounds.center.y + bounds.size.y / 2;
    }

    public static bool OutLeft(this Bounds bounds, Bounds other)
    {
        return bounds.center.x + bounds.size.x / 2 < other.center.x - other.size.x / 2;
    }

    public static int OverlapPercent(this Bounds A, Bounds B)
    {
        float result = 0f;
        //trivial cases
        if (!A.Intersects(B)) return 0;


        //# overlap between A and B
        float SA = A.size.x * A.size.y;
        float SB = B.size.x * B.size.y;
        float leftX = Mathf.Max(A.Left(), B.Left());
        float rightX = Mathf.Min(A.Right(), B.Right());
        float rightY = Mathf.Max(A.Top(), B.Top());
        float leftY = Mathf.Min(A.Bottom(), B.Bottom());
        float SI = Mathf.Max(0, rightX - leftX) *
                    Mathf.Max(0, rightY - leftY);
        float SU = SA + SB - SI;
        result = SI / SU; //ratio
        result *= 100f; //percentage
        return (int)result;
    }

    public static Bounds bounds(this Image image)
    {
        return new Bounds(new Vector2(image.rectTransform.position.x, image.rectTransform.position.y), image.rectTransform.rect.size);
    }
    #endregion

    #region Vectors
    public static void ToLeft(ref this Vector3 vector, float delta)
    {
        vector = new Vector3(vector.x - delta, vector.y, vector.z);
    }

    public static void ToRight(ref this Vector3 vector, float delta)
    {
        vector = new Vector3(vector.x + delta, vector.y, vector.z);
    }

    public static void ToTop(ref this Vector3 vector, float delta)
    {
        vector = new Vector3(vector.x, vector.y + delta, vector.z);
    }

    public static void ToBottom(ref this Vector3 vector, float delta)
    {
        vector = new Vector3(vector.x, vector.y - delta, vector.z);
    }

    public static void SetX(ref this Vector3 vector, float x)
    {
        vector = new Vector3(x, vector.y, vector.z);
    }

    public static void AddX(ref this Vector3 vector, float x)
    {
        vector = new Vector3(vector.x + x, vector.y, vector.z);
    }

    public static Vector3 SetY(this Vector3 vector, float y)
    {
        return new Vector3(vector.x, y, vector.z);
    }

    public static void AddY(ref this Vector3 vector, float y)
    {
        vector = new Vector3(vector.x, vector.y + y, vector.z);
    }

    public static Vector3 SetZ(this Vector3 vector, float z)
    {
        return new Vector3(vector.x, vector.y, z);
    }

    public static void AddZ(ref this Vector3 vector, float z)
    {
        vector = new Vector3(vector.x, vector.y, vector.z + z);
    }

    public static Vector3 Absolute(this Vector3 vector)
    {
        return new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
    }

    public static Vector2 ToVector2(this Vector3 vector)
    {
        return new Vector2(vector.x, vector.y);
    }

    public static T Last<T>(this List<T> list)
    {
        return list[list.Count - 1];
    }

    public static List<T> RemoveLast<T>(this List<T> list)
    {
        list.RemoveAt(list.Count - 1);
        return list;
    }

    public static int LastIndex<T>(this List<T> list)
    {
        return list.Count - 1;
    }
    #endregion

    #region Transform
    public static void SetX(this Transform vector, float x)
    {
        Vector3 v = vector.position;
        v.x = x;
        vector.position = v;
    }

    public static Vector2 Position(this Transform transform)
    {
        return new Vector2(transform.position.x, transform.position.y);
    }
    #endregion

    public static float Distance(this NavMeshPath path)
    {
        float lengthSoFar = 0.0f;
        for (int i = 1; i < path.corners.Length; i++) {
            lengthSoFar += Vector3.Distance(path.corners[i - 1], path.corners[i]);
        }
        return lengthSoFar;
    }

    #region RectTransform
    public static void SetX(this RectTransform vector, float x)
    {
        Vector3 v = vector.position;
        v.x = x;
        vector.position = v;
    }

    public static void SetLocalX(this RectTransform vector, float x)
    {
        Vector3 v = vector.localPosition;
        v.x = x;
        vector.localPosition = v;
    }

    public static bool IsInRange(this IList list, int value)
    {
        return value < list.Count && value >= 0;
    }

    public static void SetAlpha(this Color color, float a)
    {
        color = new Color(color.r, color.g, color.b, a);
    }

    public static Color InstallAlpha(this Color color, float a)
    {
        return new Color(color.r, color.g, color.b, a);
    }
    #endregion
}