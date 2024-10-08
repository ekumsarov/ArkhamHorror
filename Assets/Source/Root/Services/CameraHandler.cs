using UnityEngine;
using UnityEngine.InputSystem;
using EVI.Inputs;
using Zenject;
using Sirenix.OdinInspector;

namespace EVI
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class CameraHandler : MonoBehaviour, IInitializable
    {
        public enum Constraint { Landscape, Portrait }

        #region FIELDS
        [Header("Camera Settings")]
        public Color wireColor = Color.white;
        public float UnitsSize = 1; 
        public Constraint constraint = Constraint.Portrait;

        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 20f;
        [SerializeField] private float rotationSpeed = 20f;

        public bool executeInUpdate;

        private float _width;
        private float _height;

        private Bounds _bounds;
        public Bounds Bounds => _bounds;

        [SerializeField, ReadOnly, OnInspectorInit("UpdateComponents")] private Camera _camera;
        [Inject] private MainInput _mainInput;

        #endregion

        #region PROPERTIES
        public float Width => _width;
        public float Height => _height;

        // Public property to access the injected camera
        public Camera Camera => _camera;

        // Helper points
        public Vector3 BottomLeft { get; private set; }
        public Vector3 BottomCenter { get; private set; }
        public Vector3 BottomRight { get; private set; }
        public Vector3 MiddleLeft { get; private set; }
        public Vector3 MiddleCenter { get; private set; }
        public Vector3 MiddleRight { get; private set; }
        public Vector3 TopLeft { get; private set; }
        public Vector3 TopCenter { get; private set; }
        public Vector3 TopRight { get; private set; }
        #endregion

        #region DEPENDENCY INJECTION AND INITIALIZATION

        public void Initialize()
        {
            OnEnable();
        }

        private void OnEnable()
        {
            if (_mainInput == null) return;

            var moveAction = _mainInput.CameraMap.CameraContol;
            moveAction.performed += OnMovePerformed;
            moveAction.canceled += OnMoveCanceled;
            moveAction.Enable();

            var rotateAction = _mainInput.CameraMap.CameraRotate;
            rotateAction.performed += OnRotatePerformed;
            rotateAction.canceled += OnRotateCanceled;
            rotateAction.Enable();
        }

        private void OnDisable()
        {
            if (_mainInput == null) return;

            var moveAction = _mainInput.CameraMap.CameraContol;
            moveAction.performed -= OnMovePerformed;
            moveAction.canceled -= OnMoveCanceled;
            moveAction.Disable();

            var rotateAction = _mainInput.CameraMap.CameraRotate;
            rotateAction.performed -= OnRotatePerformed;
            rotateAction.canceled -= OnRotateCanceled;
            rotateAction.Disable();
        }

        #endregion

        #region METHODS FOR CAMERA SETUP

        private void ComputeResolution()
        {
            if (_camera == null)
            {
                Debug.LogError("Camera is not assigned.");
                return;
            }

            if (constraint == Constraint.Landscape)
            {
                _camera.orthographicSize = UnitsSize / (2 * _camera.aspect);
            }
            else
            {
                _camera.orthographicSize = UnitsSize / 2f;
            }

            _height = 2f * _camera.orthographicSize;
            _width = _height * _camera.aspect;

            float cameraX = _camera.transform.position.x;
            float cameraY = _camera.transform.position.y;

            // Calculate screen points
            BottomLeft = new Vector3(cameraX - _width / 2, cameraY - _height / 2, 0);
            BottomCenter = new Vector3(cameraX, cameraY - _height / 2, 0);
            BottomRight = new Vector3(cameraX + _width / 2, cameraY - _height / 2, 0);
            MiddleLeft = new Vector3(cameraX - _width / 2, cameraY, 0);
            MiddleCenter = new Vector3(cameraX, cameraY, 0);
            MiddleRight = new Vector3(cameraX + _width / 2, cameraY, 0);
            TopLeft = new Vector3(cameraX - _width / 2, cameraY + _height / 2, 0);
            TopCenter = new Vector3(cameraX, cameraY + _height / 2, 0);
            TopRight = new Vector3(cameraX + _width / 2, cameraY + _height / 2, 0);

            _bounds = new Bounds(Vector3.zero, new Vector3(Width, Height, 0));
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (executeInUpdate)
                ComputeResolution();
#endif
        }

        #endregion

        #region CAMERA MOVEMENT AND ROTATION

        private Vector3 _cameraMovementVector = Vector3.zero;
        private float _rotationInput = 0f;

        private void OnMovePerformed(InputAction.CallbackContext context)
        {
            _cameraMovementVector = context.ReadValue<Vector2>();
        }

        private void OnMoveCanceled(InputAction.CallbackContext context)
        {
            _cameraMovementVector = Vector3.zero;
        }

        private void OnRotatePerformed(InputAction.CallbackContext context)
        {
            _rotationInput = context.ReadValue<float>();
        }

        private void OnRotateCanceled(InputAction.CallbackContext context)
        {
            _rotationInput = 0f;
        }

        private void FixedUpdate()
        {
            if (_camera == null) return;

            HandleRotation();
            HandleMovement();
        }

        private void HandleMovement()
        {
            if (_cameraMovementVector == Vector3.zero) return;

            Vector3 movement = _camera.transform.up * _cameraMovementVector.y + _camera.transform.right * _cameraMovementVector.x;
            _camera.transform.position += movement * moveSpeed * Time.fixedDeltaTime;
        }

        private void HandleRotation()
        {
            if (_rotationInput == 0f) return;

            Vector3 lookPoint = Vector3.zero;
            Ray ray = _camera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                lookPoint = hit.point;
            }

            _camera.transform.RotateAround(lookPoint, Vector3.up, rotationSpeed * _rotationInput * Time.fixedDeltaTime);
        }

        #endregion

        private void UpdateComponents()
        {
            if(_camera == null)
            {
                _camera = GetComponent<Camera>();
            }
        }

        #region GIZMOS

        private void OnDrawGizmos()
        {
            if (_camera == null) return;

            Gizmos.color = wireColor;
            Matrix4x4 temp = Gizmos.matrix;
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);

            if (_camera.orthographic)
            {
                float spread = _camera.farClipPlane - _camera.nearClipPlane;
                float center = (_camera.farClipPlane + _camera.nearClipPlane) * 0.5f;
                Gizmos.DrawWireCube(new Vector3(0, 0, center), new Vector3(_camera.orthographicSize * 2 * _camera.aspect, _camera.orthographicSize * 2, spread));
            }
            else
            {
                Gizmos.DrawFrustum(Vector3.zero, _camera.fieldOfView, _camera.farClipPlane, _camera.nearClipPlane, _camera.aspect);
            }

            Gizmos.matrix = temp;
        }

        #endregion
    }
}
