using UnityEngine;

namespace EVI
{
    public class TwoPointsTransform
    {
        private Transform _startTransform;
        private Transform _endTransform;
        public Vector3 End => _endTransform.position + _endOffset;
        public Vector3 Start => _startTransform.position;

        private Vector3 _endOffset = Vector3.zero;

        public TwoPointsTransform(Transform start, Transform end)
        {
            _startTransform = start;
            _endTransform = end;
        }

        public static TwoPointsTransform Create(Transform start, Transform end)
        {
            return new TwoPointsTransform(start, end);
        }

        public void RandomEndPoint(float spread)
        {
            _endOffset += new Vector3(
                        UnityEngine.Random.Range(-spread, spread),
                        UnityEngine.Random.Range(-spread, spread),
                        UnityEngine.Random.Range(-spread, spread)
                    );
        }

        public Ray Ray
        {
            get
            {
                return new Ray(Start, End - Start);
            }
            
        }

        public Vector3 Direction
        {
            get
            {
                return End - Start;
            }
        }

        public Vector3 DirectionNormalized
        {
            get
            {
                return (End - Start).normalized;
            }
        }

        public float Distance
        {
            get
            {
                return Vector3.Distance(Start, End);
            }
        }

        public bool IsDistanceLower(float distance)
        {
            return Vector3.Distance(Start, End) <= distance;
        }
    }
}