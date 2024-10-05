namespace EVI
{
    using UnityEngine;
    using UnityEngine.InputSystem;
    using EVI.Inputs;
    using Zenject;
    using UnityEngine.EventSystems;

    [ExecuteInEditMode]
    public class CameraHandler : MonoBehaviour, IInitializable
    {
        public enum Constraint { Landscape, Portrait }

        #region FIELDS
        public Color wireColor = Color.white;
        public float UnitsSize = 1; // size of your scene in unity units
        public Constraint constraint = Constraint.Portrait;
        public new Camera camera;

        public bool executeInUpdate;

        private float _width;
        private float _height;
        //*** bottom screen
        private Vector3 _bl;
        private Vector3 _bc;
        private Vector3 _br;
        //*** middle screen
        private Vector3 _ml;
        private Vector3 _mc;
        private Vector3 _mr;
        //*** top screen
        private Vector3 _tl;
        private Vector3 _tc;
        private Vector3 _tr;

        private Bounds _bounds;
        public Bounds Bounds => _bounds;
        #endregion

        #region PROPERTIES
        public float Width
        {
            get
            {
                return _width;
            }
        }
        public float Height
        {
            get
            {
                return _height;
            }
        }

        // helper points:
        public Vector3 BottomLeft
        {
            get
            {
                return _bl;
            }
        }
        public Vector3 BottomCenter
        {
            get
            {
                return _bc;
            }
        }
        public Vector3 BottomRight
        {
            get
            {
                return _br;
            }
        }
        public Vector3 MiddleLeft
        {
            get
            {
                return _ml;
            }
        }
        public Vector3 MiddleCenter
        {
            get
            {
                return _mc;
            }
        }
        public Vector3 MiddleRight
        {
            get
            {
                return _mr;
            }
        }
        public Vector3 TopLeft
        {
            get
            {
                return _tl;
            }
        }
        public Vector3 TopCenter
        {
            get
            {
                return _tc;
            }
        }
        public Vector3 TopRight
        {
            get
            {
                return _tr;
            }
        }
        #endregion

        #region METHODS

        public void Init()
        {
            ComputeResolution();
            _bounds = new Bounds();
            _bounds.size = new Vector2(Width, Height);
            _bounds.center = new Vector2(0, 0);
        }

        private void ComputeResolution()
        {
            float leftX, rightX, topY, bottomY;

            if (constraint == Constraint.Landscape)
            {
                camera.orthographicSize = 1f / camera.aspect * UnitsSize / 2f;
            }
            else
            {
                camera.orthographicSize = UnitsSize / 2f;
            }

            _height = 2f * camera.orthographicSize;
            _width = _height * camera.aspect;

            float cameraX, cameraY;
            cameraX = camera.transform.position.x;
            cameraY = camera.transform.position.y;

            leftX = cameraX - _width / 2;
            rightX = cameraX + _width / 2;
            topY = cameraY + _height / 2;
            bottomY = cameraY - _height / 2;

            //*** bottom
            _bl = new Vector3(leftX, bottomY, 0);
            _bc = new Vector3(cameraX, bottomY, 0);
            _br = new Vector3(rightX, bottomY, 0);
            //*** middle
            _ml = new Vector3(leftX, cameraY, 0);
            _mc = new Vector3(cameraX, cameraY, 0);
            _mr = new Vector3(rightX, cameraY, 0);
            //*** top
            _tl = new Vector3(leftX, topY, 0);
            _tc = new Vector3(cameraX, topY, 0);
            _tr = new Vector3(rightX, topY, 0);
        }

        private void Update()
        {
#if UNITY_EDITOR

            if (executeInUpdate)
                ComputeResolution();

#endif
        }

#region  Contoll

        [Inject] private MainInput _mainInput;
        private InputAction _moveAction;
        private InputAction _rotateAction;

        private void OnEnable()
        {
            if(_mainInput == null)
                return;

            
            _moveAction = _mainInput.CameraMap.CameraContol;
            _moveAction.performed += MovePerfomed;
            _moveAction.started += MoveCanceled;
            _moveAction.Enable();

            _rotateAction = _mainInput.CameraMap.CameraRotate;
            _rotateAction.Enable();
        }

        private bool _isMoveingCamera = false;
        private Vector3 _cameraVector = Vector3.zero;
        private void MoveStarted(InputAction.CallbackContext inputAction)
        {
            _isMoveingCamera = true;
        }


        private void MovePerfomed(InputAction.CallbackContext inputAction)
        {
            _cameraVector = inputAction.ReadValue<Vector2>();
        }

        private void MoveCanceled(InputAction.CallbackContext inputAction)
        {
            _isMoveingCamera = false;
            _cameraVector = Vector3.zero;
        }

        private void OnDisable()
        {
            if(_mainInput == null)
                return;

            _moveAction.performed -= MovePerfomed;
            _moveAction.Disable();
            _moveAction = null;
        }

        private Vector3 _roundAxis = new Vector3(0, -1f, 0);
        private void FixedUpdate() 
        {
            if(_moveAction == null)
                return;


            
            if(_rotateAction.inProgress)
            {
                Vector3 lookPoint = Vector3.zero;
                Ray ray = camera.ScreenPointToRay(new Vector2(Screen.width/2, Screen.height/2));
                if (Physics.Raycast(ray: ray, hitInfo: out RaycastHit hit))
                {
                    lookPoint = hit.point;
                }

                float axis = _rotateAction.ReadValue<float>();
                if(axis > 0)
                {
                    camera.transform.RotateAround(lookPoint, Vector3.up, 20*axis*Time.fixedDeltaTime);
                }
                else if(axis < 0)
                {
                    camera.transform.RotateAround(lookPoint, Vector3.up, 20*axis*Time.fixedDeltaTime);
                }
            }
        
            _cameraVector = Vector3.zero;
            if(_moveAction.inProgress)
                _cameraVector = _moveAction.ReadValue<Vector2>();
                
            //Vector3 newPos = new Vector3(_cameraVector.x * Time.fixedDeltaTime * 20f, 0f, _cameraVector.y * Time.fixedDeltaTime * 20f);
            Vector3 newPos = camera.transform.up * _cameraVector.y + transform.right * _cameraVector.x;
            camera.transform.position += newPos * 20f * Time.deltaTime;
        }
#endregion
        void OnDrawGizmos()
        {
            Gizmos.color = wireColor;

            Matrix4x4 temp = Gizmos.matrix;
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
            if (camera.orthographic)
            {
                float spread = camera.farClipPlane - camera.nearClipPlane;
                float center = (camera.farClipPlane + camera.nearClipPlane) * 0.5f;
                Gizmos.DrawWireCube(new Vector3(0, 0, center), new Vector3(camera.orthographicSize * 2 * camera.aspect, camera.orthographicSize * 2, spread));
            }
            else
            {
                Gizmos.DrawFrustum(Vector3.zero, camera.fieldOfView, camera.farClipPlane, camera.nearClipPlane, camera.aspect);
            }
            Gizmos.matrix = temp;
        }

        public void Initialize()
        {
            _moveAction = _mainInput.CameraMap.CameraContol;
            _moveAction.performed += MovePerfomed;
            _moveAction.started += MoveCanceled;
            _moveAction.Enable();

            _rotateAction = _mainInput.CameraMap.CameraRotate;
            _rotateAction.Enable();
        }
        #endregion

    }
}
