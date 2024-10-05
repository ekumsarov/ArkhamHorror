using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EVI
{
    [JSONSerializable]
    public class BaseView : BindableView
    {
        [BindTo]
        public void SetRotation(float angle)
        {
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        [BindTo]
        public void SetPosition(Vector3 pos)
        {
            SetPositionInternal(pos);
        }

        public void SetupPosition(Vector3 pos)
        {
            transform.position = pos;
        }

        protected virtual void SetPositionInternal(Vector3 pos)
        {
            transform.position = pos;
        }

        public Vector3 Position => transform.position;
    }
}