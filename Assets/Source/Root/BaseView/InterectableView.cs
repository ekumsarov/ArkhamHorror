using EVI.DDSystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace EVI
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class InteractableView : BaseView, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [Inject] private readonly CameraHandler _camera;

        [SerializeField, FoldoutGroup("Interactive")] private bool _isInteractable;
        [SerializeField, FoldoutGroup("Interactive")] private bool _isButton;
        [SerializeField, FoldoutGroup("Interactive")] private bool _isDraggable;

        [SerializeField, ReadOnly, OnInspectorInit("InitializeCollider")] private BoxCollider2D _collider;

        private bool _isDragging = false;
        private float _timeStamp = 0f;

        public event Action<InteractableView> StopDrag;
        public event Action OnClick;

        private void InitializeCollider()
        {
            if (_collider == null)
                _collider = GetComponent<BoxCollider2D>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_isInteractable)
                return;

            _timeStamp = Time.time;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!_isInteractable)
                return;

            if (_isDragging)
            {
                _isDragging = false;
                StopDrag?.Invoke(this);
            }
            else if (Time.time - _timeStamp < 0.2f && _isButton)
            {
                OnClick?.Invoke();
            }

            _timeStamp = 0f;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_isInteractable || !_isDraggable)
                return;

            if (Time.time - _timeStamp >= 0.2f)
            {
                _isDragging = true;
                DragItem(eventData);
            }
        }

        private void DragItem(PointerEventData eventData)
        {
            Vector3 mousePosition = _camera.Camera.ScreenToWorldPoint(eventData.position);
            SetPosition(mousePosition.SetZ(0));
        }
    }
}
