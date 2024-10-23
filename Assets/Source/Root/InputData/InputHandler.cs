using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using EVI.DDSystem;
using EVI.Inputs;

namespace EVI
{
    public class InputHandler : MonoBehaviour, IInitializable
    {
        [Inject] private CameraHandler _mainCamera;
        [Inject] private MainInput _mainInput;
        private IInteractable _currentInteractable;
        private bool _isDragging = false;
        private float _clickTimeThreshold = 0.2f; // Время, определяющее клик или перетаскивание
        private float _dragThreshold = 0.1f; // Минимальное перемещение для начала перетаскивания
        private Vector2 _initialPointerPosition;
        private float _pressStartTime;

        public void Initialize()
        {
            _mainInput.MainMap.ClickLeft.started += OnPressStarted;
            _mainInput.MainMap.ClickLeft.canceled += OnPressReleased;
            _mainInput.MainMap.PointerPosition.performed += OnPointerMove;
        }

        private void OnEnable()
        {
            _mainInput.Enable();
        }

        private void OnDisable()
        {
            _mainInput.Disable();
        }

        private void OnPressStarted(InputAction.CallbackContext context)
        {
            _pressStartTime = Time.time;
            _initialPointerPosition = _mainInput.MainMap.PointerPosition.ReadValue<Vector2>();

            // Raycast для поиска IInteractable объекта
            Vector3 worldPosition = _mainCamera.Camera.ScreenToWorldPoint(new Vector3(_initialPointerPosition.x, _initialPointerPosition.y, 0));
            worldPosition.z = 0; // Устанавливаем Z в 0 для 2D мира

            if (Helper.RaycastObject2D<IInteractable>(worldPosition, out RaycastResult2D<IInteractable> result))
            {
                _currentInteractable = result.Object;
            }
        }

        private void OnPressReleased(InputAction.CallbackContext context)
        {
            if (_currentInteractable == null)
            {
                return;
            }

            float pressDuration = Time.time - _pressStartTime;
            Vector2 pointerPosition = _mainInput.MainMap.PointerPosition.ReadValue<Vector2>();

            if (_isDragging)
            {
                _currentInteractable.HandleEndDrag();
                _isDragging = false;
            }
            else if (pressDuration < _clickTimeThreshold && _currentInteractable.IsButton)
            {
                _currentInteractable.HandleClick();
            }

            _currentInteractable = null; // Сброс текущего взаимодействия
        }

        private void OnPointerMove(InputAction.CallbackContext context)
        {
            if (_currentInteractable == null)
            {
                return;
            }

            Vector2 pointerPosition = _mainInput.MainMap.PointerPosition.ReadValue<Vector2>();
            Vector3 worldPosition = _mainCamera.Camera.ScreenToWorldPoint(new Vector3(pointerPosition.x, pointerPosition.y, 0));
            worldPosition.z = 0; // Устанавливаем Z в 0 для 2D мира

            // Проверяем, нужно ли начать перетаскивание
            if (!_isDragging && _currentInteractable.IsDraggable &&
                Vector2.Distance(pointerPosition, _initialPointerPosition) > _dragThreshold)
            {
                _isDragging = true;
                _currentInteractable.HandleBeginDrag();
            }

            if (_isDragging)
            {
                // Передаем мировые координаты для перетаскивания
                _currentInteractable.HandleDrag(worldPosition);
            }
        }

        private void Update()
        {
            // Если пользователь отпустил мышь, завершить перетаскивание
            if (Mouse.current.leftButton.wasReleasedThisFrame && _isDragging)
            {
                _currentInteractable?.HandleEndDrag();
                _isDragging = false;
            }
        }
    }
}
