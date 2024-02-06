using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EVI
{
    public struct Direction
    {
        public enum DirectionIntensivity
        {
            Zero = 0,
            Low = 1,
            Medium = 2,
            High = 3
        }

        public enum DirectionXAxis
        {
            Straight = 0,
            Left = 1,
            Right = 2
        }

        private float _angle;
        public float Rotation => _angle;

        private float _parentAngle;

        private Vector3 _vectorNormalized;

        private DirectionIntensivity _intense;
        public DirectionIntensivity Intense => _intense;

        private DirectionXAxis _xAxisDirection;
        public DirectionXAxis XAxisDirection => _xAxisDirection;

        public static Direction Create(float angle)
        {
            Direction temp = new Direction();
            temp._parentAngle = angle;
            temp._angle = angle;
            temp._vectorNormalized = Quaternion.Euler(0f, 0f, angle) * Vector3.right;
            temp._intense = temp.CalculateIntensivity();
            temp._xAxisDirection = temp.CalculateDirection();

            return temp;
        }

        public void SetDirection(DirectionXAxis xAxis, DirectionIntensivity intense)
        {
            _intense = intense;
            _xAxisDirection = xAxis;
        }

        public void SetParentAngle(float angle)
        {
            _parentAngle = angle;
            _angle = angle;
            _vectorNormalized = Quaternion.Euler(0f, 0f, angle) * Vector3.right;
            _intense = CalculateIntensivity();
            _xAxisDirection = CalculateDirection();
        }

        public void SetDirection(Vector2 position, Vector2 trget)
        {

            Vector2 difference = trget - position;
            float angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            _angle = angle;
            _vectorNormalized = Quaternion.Euler(0f, 0f, angle) * Vector3.right;
            if (Mathf.Approximately(0f, difference.x) && Mathf.Approximately(0f, difference.y))
            {
                _intense = DirectionIntensivity.Zero;
                _xAxisDirection = DirectionXAxis.Straight;

                return;
            }

            _intense = CalculateIntensivity();
            _xAxisDirection = CalculateDirection();
        }

        public void AddAngle(float angle)
        {
            _angle += angle;
            _vectorNormalized = Quaternion.Euler(0f, 0f, angle) * Vector3.right;
            _intense = CalculateIntensivity();
            _xAxisDirection = CalculateDirection();
        }

        public static DirectionXAxis CalculateDirection(Direction direction)
        {
            Vector3 vector = direction._vectorNormalized.Absolute();

            if (vector.x < vector.y)
            {
                if (direction._vectorNormalized.y > 0 && direction._vectorNormalized.x > 0)
                {
                    return DirectionXAxis.Right;
                }

                if (direction._vectorNormalized.y < 0 && direction._vectorNormalized.x < 0)
                {
                    return DirectionXAxis.Right;
                }
            }
            else
            {
                if (direction._vectorNormalized.x > 0 && direction._vectorNormalized.y < 0)
                {
                    return DirectionXAxis.Right;
                }

                if (direction._vectorNormalized.y > 0 && direction._vectorNormalized.x < 0)
                {
                    return DirectionXAxis.Right;
                }
            }

            return DirectionXAxis.Left;
        }

        private DirectionXAxis CalculateDirection()
        {
            Vector3 vector = _vectorNormalized.Absolute();

            if (vector.x < vector.y)
            {
                if (_vectorNormalized.y > 0 && _vectorNormalized.x > 0)
                {
                    return DirectionXAxis.Right;
                }

                if (_vectorNormalized.y < 0 && _vectorNormalized.x < 0)
                {
                    return DirectionXAxis.Right;
                }
            }
            else
            {
                if (_vectorNormalized.x < 0 && _vectorNormalized.y < 0)
                {
                    return DirectionXAxis.Right;
                }

                if (_vectorNormalized.y > 0 && _vectorNormalized.x < 0)
                {
                    return DirectionXAxis.Right;
                }
            }


            return DirectionXAxis.Left;
        }

        public static DirectionIntensivity CalculateIntensivity(Direction direction)
        {
            Vector3 vector = direction._vectorNormalized.Absolute();
            if (vector.x < 0.03f || vector.y < 0.03f)
                return DirectionIntensivity.Zero;

            float lowest = vector.x < vector.y ? vector.x : vector.y;

            if (lowest > 0 && (lowest <= 0.2f || lowest >= 0.8f))
                return DirectionIntensivity.Low;

            if (lowest > 0 && (lowest <= 0.4 || lowest >= 0.6f))
                return DirectionIntensivity.Medium;

            return DirectionIntensivity.High;
        }

        private DirectionIntensivity CalculateIntensivity()
        {
            /*Vector3 vector = _vectorNormalized.Absolute();
            if (vector.x < 0.03f || vector.y < 0.03f)
                return DirectionIntensivity.Zero;

            float lowest = vector.x > vector.y ? vector.x : vector.y;

            if (lowest <= 0.2f || lowest >= 0.8f)
                return DirectionIntensivity.Low;

            if (lowest <= 0.4 || lowest >= 0.6f)
                return DirectionIntensivity.Medium;

            return DirectionIntensivity.High;*/

            float difference = Mathf.Repeat(_angle, 360f) - Mathf.Repeat(_parentAngle, 360f);
            float absDiff = Mathf.Abs(difference);

            if (absDiff < 5)
                return DirectionIntensivity.Zero;
            else if (Helper.IsValueInRange(5, 25, absDiff) == true)
                return DirectionIntensivity.Low;
            else if (Helper.IsValueInRange(25, 50, absDiff) == true)
                return DirectionIntensivity.Medium;

            return DirectionIntensivity.High;
        }
    }
}