using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EVI
{
    public class BaseView : BindableView
    {
        [BindTo]
        public void SetRotation(float angle)
        {
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        [BindTo]
        public void SetPosition(Vector2 pos) => transform.position = pos;
        public void SetPosition(float x, float y)
        {
            transform.position = new Vector2(x, y);
        }
        public Vector2 Position => transform.position;
        public Vector3 PositionVector3 => transform.position;
    }
}