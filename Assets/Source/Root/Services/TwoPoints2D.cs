using UnityEngine;

namespace EVI
{
    public class TwoPoints2D
    {
        public Vector2 Start { get; set; }
        public Vector2 Direction { get; set; }
        public float Distance { get; set; }

        public static TwoPoints2D Create(Vector2 start, Vector2 end)
        {
            return new TwoPoints2D
            {
                Start = start,
                Direction = (end - start).normalized,
                Distance = Vector2.Distance(start, end)
            };
        }
    }
}