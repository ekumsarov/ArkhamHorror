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
        [SerializeField, OnInspectorInit("BaseViewComponents")] private SpriteRenderer _sprite;

        public Bounds Bounds => _sprite.bounds;

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

        private void BaseViewComponents()
        {
            if(_sprite == null)
                _sprite = GetComponent<SpriteRenderer>();
        }

        public Vector3 Position => transform.position;


    }
}