using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace EVI
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class DraggableView : BaseView
    {
        [Inject] private readonly CameraHandler _camera;

        [OdinSerialize, ReadOnly, OnInspectorInit("OnInspector")] private BoxCollider2D _collider;
        private void OnInspector()
        {
            if (_collider == null)
                _collider = GetComponent<BoxCollider2D>();
        }

        public void OnMouseDown()
        {
            _isDraging = true;
            StartCoroutine(Draging());
        }

        public void OnMouseUp()
        {
            _isDraging = false;
        }

        private bool _isDraging = false;
        private IEnumerator Draging()
        {
            yield return null;

            while(_isDraging)
            { 
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - PositionVector3;
                SetPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));

                yield return null;
            }

            yield return null;
        }
    }
}

