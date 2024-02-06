using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace EVI
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class InterectableView : BaseView
    {
        [OdinSerialize, FoldoutGroup("Interactive")] private bool _isInteractable;
        [OdinSerialize, FoldoutGroup("Interactive")] private bool _isButton;
        [OdinSerialize, FoldoutGroup("Interactive")] private bool _isDraggable;
        
        [OdinSerialize, ReadOnly, OnInspectorInit("OnInspector")] private BoxCollider2D _collider;
        private void OnInspector()
        {
            if (_collider == null)
                _collider = GetComponent<BoxCollider2D>();
        }

        private bool _isDraging = false;
        private float _timeStamp = 0f;

        public void OnMouseDown()
        {
            if (_isInteractable == false)
                return;

            _timeStamp = Time.time;
        }

        public void OnMouseUp()
        {
            if (_isInteractable == false)
                return;

            if(_isDraging)
            {
                _isDraging = false;
                _timeStamp = 0f;
                StopDrag?.Invoke(this);
                return;
            }

            if (Time.time - _timeStamp < 0.2f)
            {
                StopCoroutine("OnMouseDrag");
                if (_isButton)
                {
                    OnClick?.Invoke();
                }
            }

            _isDraging = false;
            _timeStamp = 0f;
        }

        public IEnumerator OnMouseDrag()
        {
            if (_isInteractable == false || _isDraggable == false)
                yield break;

            float delta = Time.time - _timeStamp;
            while (delta < 0.2f)
            {
                delta = Time.time - _timeStamp;
                yield return null;
            }

            _isDraging = true;
            yield return null;

            while (_isDraging)
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - PositionVector3;
                SetPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));

                yield return null;
            }
        }

        public Action<InterectableView> StopDrag;
        public Action OnClick;
    }
}