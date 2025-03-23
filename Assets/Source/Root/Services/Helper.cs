using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SimpleJSON;
using System.Globalization;
using UnityEngine.AI;
using Unity.VisualScripting;

namespace EVI
{
    public static class Helper
    {
        public static JSONNode GetNodeFrom<T>(IJsonProcessing<T> arg)
        {
            if (arg == null)
                return null;

            return arg.GetJSON();
        }

        public static JSONArray GetArrayTypeOf<T>(IJsonProcessing<T>[] arg)
        {
            JSONArray array = new JSONArray();
            foreach (var argument in arg)
            {
                array.Add(argument.GetJSON());
            }

            return array;
        }

        public static T RecogniseEnum<T>(Type enumType, string arg)
        {
            return (T)Enum.Parse(enumType, arg);
        }

        public static JSONNode Vector2ToJSON(Vector2 value)
        {
            JSONNode node = new JSONObject();

            node.Add("x", value.x);
            node.Add("y", value.y);

            return node;
        }

        public static JSONNode Vector3ToJSON(Vector3 value)
        {
            JSONNode node = new JSONObject();

            node.Add("x", value.x.ToString("0.00", CultureInfo.InvariantCulture));
            node.Add("y", value.y.ToString("0.00", CultureInfo.InvariantCulture));
            node.Add("z", value.z.ToString("0.00", CultureInfo.InvariantCulture));

            return node;
        }

        public static Vector2 JsonToVecotr2(JSONNode node)
        {
            return new Vector2(node["x"].AsFloat, node["y"].AsFloat);
        }

        public static Vector3 JsonToVecotr3(JSONNode node)
        {
            return new Vector3(node["x"].AsFloat, node["y"].AsFloat, node["z"].AsFloat);
        }

        public static void LogVector(Vector3 vector)
        {
            Debug.Log("x:" + vector.x.ToString() + "  y:" + vector.y.ToString() + "  z:" + vector.z.ToString());
        }
        public static bool IsValueInRange(float minInclude, float maxExclude, float value)
        {
            return value >= minInclude && value < maxExclude;
        }

        public static bool IsDistanceIsNear(Vector3 start, Vector3 end)
        {
            //Debug.LogError("Dis: " + Vector3.Distance(start, end).ToString("0.00"));
            return Vector3.Distance(start, end) <= 0.9f;
        }

        public static Vector3 GetPointWithMaxDistance(TwoPoints point, float maxDistance)
        {
            NavMeshPath path = point.GetPath();
            return GetPointWithMaxDistance(path, maxDistance);
        }

        public static Vector3 GetPointWithMaxDistance(NavMeshPath path, float maxDistance)
        {

            if (path.corners.Length == 0)
                return Vector3.zero;

            if (path.corners.Length == 1)
                return path.corners[0];

            float len = 0f;

            for (int i = 1; i < path.corners.Length; i++)
                len += Vector3.Distance(path.corners[i], path.corners[i - 1]);


            float d = 0f;
            for (int i = 1; i < path.corners.Length; i++)
            {
                float distanceBetweenPoints = d + Vector3.Distance(path.corners[i], path.corners[i - 1]);
                if (maxDistance < d + distanceBetweenPoints)
                {
                    float t = (maxDistance - d) / distanceBetweenPoints;
                    return Vector3.Lerp(path.corners[i - 1], path.corners[i], t);
                }
                d += distanceBetweenPoints;
            }

            return path.corners[path.corners.Length - 1];
        }

        public static class BezierCurveHelper
        {
            public static Vector3 Interpolate(Vector3 start, Vector3 control, Vector3 end, float t)
            {
                Vector3 point = (((1 - t) * (1 - t)) * start) + ((2 * (1 - t) * t) * control) + (t * t * end);
                return point;
            }

            public static List<Vector3> GetCurvePoints(Vector3 start, Vector3 control, Vector3 end, int resolution)
            {
                if (resolution < 5)
                    throw new System.ArgumentException("Please provide a positive, non-zero resolution.");

                List<Vector3> tempList = new List<Vector3>();

                float timeStep = 1.0f / resolution;
                for (int i = 0; i <= resolution; i++)
                {
                    tempList.Add(Interpolate(start, control, end, timeStep * i));
                }

                return tempList;
            }
        }

        public static string MarkText(string text, Color color)
        {
            return string.Format("<color=" + color.ToHexString() + ">" + text + "</color>");
        }

        public static class CatmullRomCurveHelper
        {
            public static Vector3 Interpolate(Vector3 start, Vector3 end, Vector3 tanPoint1, Vector3 tanPoint2, float t)
            {
                // Catmull-Rom splines are Hermite curves with special tangent values.
                // Hermite curve formula:
                // (2t^3 - 3t^2 + 1) * p0 + (t^3 - 2t^2 + t) * m0 + (-2t^3 + 3t^2) * p1 + (t^3 - t^2) * m1
                // For points p0 and p1 passing through points m0 and m1 interpolated over t = [0, 1]
                // Tangent M[k] = (P[k+1] - P[k-1]) / 2
                // With [] indicating subscript
                Vector3 position = (2.0f * t * t * t - 3.0f * t * t + 1.0f) * start
                    + (t * t * t - 2.0f * t * t + t) * tanPoint1
                    + (-2.0f * t * t * t + 3.0f * t * t) * end
                    + (t * t * t - t * t) * tanPoint2;

                return position;
            }

            public static Vector3 Interpolate(Vector3 start, Vector3 end, Vector3 tanPoint1, Vector3 tanPoint2, float t, out Vector3 tangent)
            {
                // Calculate tangents
                // p'(t) = (6t� - 6t)p0 + (3t� - 4t + 1)m0 + (-6t� + 6t)p1 + (3t� - 2t)m1
                tangent = (6 * t * t - 6 * t) * start
                    + (3 * t * t - 4 * t + 1) * tanPoint1
                    + (-6 * t * t + 6 * t) * end
                    + (3 * t * t - 2 * t) * tanPoint2;
                return Interpolate(start, end, tanPoint1, tanPoint2, t);
            }

            public static Vector3 Interpolate(Vector3 start, Vector3 end, Vector3 tanPoint1, Vector3 tanPoint2, float t, out Vector3 tangent, out Vector3 curvature)
            {
                // Calculate second derivative (curvature)
                // p''(t) = (12t - 6)p0 + (6t - 4)m0 + (-12t + 6)p1 + (6t - 2)m1
                curvature = (12 * t - 6) * start
                    + (6 * t - 4) * tanPoint1
                    + (-12 * t + 6) * end
                    + (6 * t - 2) * tanPoint2;
                return Interpolate(start, end, tanPoint1, tanPoint2, t, out tangent);

            }

            public static List<Vector3> GetCoordinates(List<Vector3> points, int curveResolution, bool ClosedLoop)
            {
                if (points == null || points.Count < 2 || curveResolution < 5)
                    return null;

                Vector3[] temp;

                Vector3 p0;
                Vector3 p1;
                Vector3 m0;
                Vector3 m1;
                int pointsToMake;

                if (ClosedLoop == true)
                {
                    pointsToMake = (curveResolution) * (points.Count);
                }
                else
                {
                    pointsToMake = (curveResolution) * (points.Count - 1);
                }

                temp = new Vector3[pointsToMake];

                int closedAdjustment = ClosedLoop ? 0 : 1;

                for (int i = 0; i < points.Count - closedAdjustment; i++)
                {

                    p0 = points[i];
                    p1 = (ClosedLoop == true && i == points.Count - 1) ? points[0] : points[i + 1];

                    if (i == 0)
                    {
                        m0 = ClosedLoop ? 0.5f * (p1 - points[points.Count - 1]) : p1 - p0;
                    }
                    else
                    {
                        m0 = 0.5f * (p1 - points[i - 1]);
                    }

                    if (ClosedLoop)
                    {
                        if (i == points.Count - 1)
                        {
                            m1 = 0.5f * (points[(i + 2) % points.Count] - p0);
                        }
                        else if (i == 0)
                        {
                            m1 = 0.5f * (points[i + 2] - p0);
                        }
                        else
                        {
                            m1 = 0.5f * (points[(i + 2) % points.Count] - p0);
                        }
                    }
                    else
                    {
                        if (i < points.Count - 2)
                        {
                            m1 = 0.5f * (points[(i + 2) % points.Count] - p0);
                        }
                        else
                        {
                            m1 = p1 - p0;
                        }
                    }

                    Vector3 position;
                    float t;
                    float pointStep = 1.0f / curveResolution;

                    if ((i == points.Count - 2 && ClosedLoop == false) || (i == points.Count - 1 && ClosedLoop))
                    {
                        pointStep = 1.0f / (curveResolution - 1);
                    }

                    for (int j = 0; j < curveResolution; j++)
                    {
                        t = j * pointStep;
                        position = Helper.CatmullRomCurveHelper.Interpolate(p0, p1, m0, m1, t);
                        temp[i * curveResolution + j] = position;
                    }
                }
                return temp.ToList();
            }

            
        }
    
        #region Raycast

            public static bool RaycastObject2D<T>(Vector2 start, out RaycastResult2D<T> result)
            {
                result = default;

                RaycastHit2D hit = Physics2D.Raycast(start, Vector2.zero);
                if (hit.collider != null)
                {
                    if (hit.transform.gameObject.TryGetComponent(out T character))
                    {
                        result = RaycastResult2D<T>.Create(character, hit);
                        return true;
                    }
                }

                return false;
            }

            public static bool RaycastObject2D<T>(TwoPoints2D point, out RaycastResult2D<T> result)
            {
                result = default;

                RaycastHit2D[] hits = Physics2D.RaycastAll(point.Start, point.Direction, point.Distance);
                foreach (var hit in hits)
                {
                    if (hit.transform.gameObject.TryGetComponent(out T character))
                    {
                        result = RaycastResult2D<T>.Create(character, hit);
                        return true;
                    }
                }

                return false;
            }

            #endregion
        
    }
}
