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
    public class InteractableView : MonoBehaviour, IInteractable
    {
        private CameraHandler _camera;

        [SerializeField, FoldoutGroup("Interactive")] private bool _isInteractable;
        [SerializeField, FoldoutGroup("Interactive")] private bool _isButton;
        [SerializeField, FoldoutGroup("Interactive")] private bool _isDraggable;

        [SerializeField, ReadOnly, OnInspectorInit("InitializeCollider")] private BoxCollider2D _collider;

        private bool _isDragging = false;

        public void SetDraggable(bool isDraggable)
        {
            _isDraggable = isDraggable;
        }

        public bool IsDraggable => _isDraggable;
        public bool IsButton => _isButton;

        private void InitializeCollider()
        {
            if (_collider == null)
                _collider = GetComponent<BoxCollider2D>();
        }

        public void Initialize(CameraHandler camera)
        {
            _camera = camera;
        }

        public void HandleClick() 
        {
            if (!_isInteractable)
                return;

            Debug.LogError("Clicked");

            OnClick?.Invoke();
        }

        public void HandleBeginDrag()
        {
            if (!_isInteractable || !_isDraggable)
                return;

            _isDragging = true;
            OnBeginDrag?.Invoke();
        }

        public void HandleDrag(Vector2 mousePosition)
        {
            if (!_isInteractable || !_isDraggable)
                return;

            Debug.LogError("IsDragging");

            if(OnDrag != null)
            {
                OnDrag.Invoke();
                return;
            }

            DragItem(mousePosition);
        }

        public void HandleEndDrag()
        {
            if (_isDragging)
            {
                _isDragging = false;
                OnEndDrag?.Invoke();
            }
        }

        private void DragItem(Vector2 mouseScreenPosition)
        {
            transform.position = mouseScreenPosition;
        }

        public event Action OnBeginDrag;
        public event Action OnDrag;
        public event Action OnEndDrag;
        public event Action OnClick;
    }
}