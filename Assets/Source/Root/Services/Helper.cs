using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;
using SimpleJSON;

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

            node.Add("x", value.x);
            node.Add("y", value.y);
            node.Add("z", value.z);

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

        public static bool IsValueInRange(float minInclude, float maxExclude, float value)
        {
            return value >= minInclude && value < maxExclude;
        }

        public static class BezierCurveHelper
        {
            public static Vector2 Interpolate(Vector2 start, Vector2 control, Vector2 end, float t)
            {
                Vector2 point = (((1 - t) * (1 - t)) * start) + ((2 * (1 - t) * t) * control) + (t * t * end);
                return point;
            }

            public static List<Vector2> GetCurvePoints(Vector2 start, Vector2 control, Vector2 end, int resolution)
            {
                if (resolution < 5)
                    throw new System.ArgumentException("Please provide a positive, non-zero resolution.");

                List<Vector2> tempList = new List<Vector2>();

                float timeStep = 1.0f / resolution;
                for (int i = 0; i <= resolution; i++)
                {
                    tempList.Add(Interpolate(start, control, end, timeStep * i));
                }

                return tempList;
            }
        }

        public static class CutmullRomCurveHelper
        {
            public static Vector2 Interpolate(Vector2 start, Vector2 end, Vector2 tanPoint1, Vector2 tanPoint2, float t)
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

            public static Vector2 Interpolate(Vector2 start, Vector2 end, Vector2 tanPoint1, Vector2 tanPoint2, float t, out Vector3 tangent)
            {
                // Calculate tangents
                // p'(t) = (6t² - 6t)p0 + (3t² - 4t + 1)m0 + (-6t² + 6t)p1 + (3t² - 2t)m1
                tangent = (6 * t * t - 6 * t) * start
                    + (3 * t * t - 4 * t + 1) * tanPoint1
                    + (-6 * t * t + 6 * t) * end
                    + (3 * t * t - 2 * t) * tanPoint2;
                return Interpolate(start, end, tanPoint1, tanPoint2, t);
            }

            public static Vector2 Interpolate(Vector2 start, Vector2 end, Vector2 tanPoint1, Vector2 tanPoint2, float t, out Vector3 tangent, out Vector2 curvature)
            {
                // Calculate second derivative (curvature)
                // p''(t) = (12t - 6)p0 + (6t - 4)m0 + (-12t + 6)p1 + (6t - 2)m1
                curvature = (12 * t - 6) * start
                    + (6 * t - 4) * tanPoint1
                    + (-12 * t + 6) * end
                    + (6 * t - 2) * tanPoint2;
                return Interpolate(start, end, tanPoint1, tanPoint2, t, out tangent);

            }

            public static List<Vector2> GetCoordinates(List<Vector2> points, int curveResolution, bool ClosedLoop)
            {
                if (points == null || points.Count < 2 || curveResolution < 5)
                    return null;

                Vector2[] temp;

                Vector2 p0;
                Vector2 p1;
                Vector2 m0;
                Vector2 m1;
                int pointsToMake;

                if (ClosedLoop == true)
                {
                    pointsToMake = (curveResolution) * (points.Count);
                }
                else
                {
                    pointsToMake = (curveResolution) * (points.Count - 1);
                }

                temp = new Vector2[pointsToMake];

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
                        position = Helper.CutmullRomCurveHelper.Interpolate(p0, p1, m0, m1, t);
                        temp[i * curveResolution + j] = position;
                    }
                }
                return temp.ToList();
            }
        }
    }
}
