
using UnityEngine;
using UnityEngine.AI;

namespace EVI
{

    public class TwoPoints
    {
        private Vector3 _start;
        private Vector3 _end;

        public Vector3 Start => _start;
        public Vector3 End => _end;

        public TwoPoints(Vector3 start, Vector3 end)
        {
            _start = start;
            _end = end;
        }

        public static TwoPoints Create(Vector3 start, Vector3 end)
        {
            return new TwoPoints(start, end);
        }

        public void RandomizeEndPoint(float spread)
        {
            _end += new Vector3(
                UnityEngine.Random.Range(-spread, spread),
                UnityEngine.Random.Range(-spread, spread),
                UnityEngine.Random.Range(-spread, spread)
            );
        }

        public void MaximizeEndPoint(float spread)
        {
            _end += new Vector3(spread * 2, spread * 2, spread * 2);
        }

        public Ray Ray => new Ray(_start, (_end - _start).normalized);

        public Vector3 Direction => _end - _start;

        public Vector3 NormalizedDirection => (_end - _start).normalized;

        public float Distance => Vector3.Distance(_start, _end);

        public float NavMeshDistance
        {
            get
            {
                NavMeshPath path = new NavMeshPath();
                NavMesh.CalculatePath(_start, _end, NavMesh.AllAreas, path);
                return path.Distance();
            }
        }

        public NavMeshPath GetPath()
        {
            NavMeshPath path = new NavMeshPath();
            NavMesh.CalculatePath(_start, _end, NavMesh.AllAreas, path);
            return path;
        }

        public NavMeshPath GetPath(float maxDistance)
        {
            NavMeshPath path = new NavMeshPath();
            _end = Helper.GetPointWithMaxDistance(this, maxDistance);
            NavMesh.CalculatePath(_start, _end, NavMesh.AllAreas, path);
            return path;
        }

        public Vector3 LerpToEndPoint(float t)
        {
            t = Mathf.Clamp01(t);
            return Vector3.Lerp(_start, _end, t);
        }

        public Quaternion GetLookAtRotation()
        {
            return Quaternion.LookRotation(Direction, Vector3.up);
        }

        public Vector3 GetHorizontalLookRotation()
        {
            Quaternion quaternion = Quaternion.LookRotation(Direction, Vector3.up);
            Vector3 rotation = quaternion.eulerAngles;
            rotation.x = 0;
            rotation.z = 0;
            return rotation;
        }
    }
}