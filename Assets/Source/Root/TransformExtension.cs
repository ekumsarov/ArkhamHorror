using UnityEngine;
using System.Collections;
using Zenject;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections.Generic;
using System.Reflection;
using System;

namespace EVI
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Transform))]
    public class TransformExtension : MonoBehaviour, IInitializable
    {
        [Inject] private CameraHandler _cameraHandler;

        public enum AnchorType
        {
            BottomLeft,
            BottomCenter,
            BottomRight,
            MiddleLeft,
            MiddleCenter,
            MiddleRight,
            TopLeft,
            TopCenter,
            TopRight,
        };

        [SerializeField] private bool _manualControl = true;
        [SerializeField, OnValueChanged("UpdateAnchor")] private AnchorType anchorType;
        [SerializeField, OnValueChanged("UpdateAnchor")] private Vector3 anchorOffset;
        [SerializeField, OnValueChanged("RotationUpdate")] private Vector3 _rotation;
        [SerializeField, OnValueChanged("ScaleUpdate")] private Vector3 _scale;

        private void RotationUpdate()
        {
            transform.rotation = Quaternion.Euler(_rotation);
        }

        private void ScaleUpdate()
        {
            transform.localScale = _scale;
        }

        public void Initialize()
        {
            UpdateAnchor();
        }

        void UpdateAnchor()
        {
            switch (anchorType)
            {
                case AnchorType.BottomLeft:
                    SetAnchor(_cameraHandler.BottomLeft);
                    break;
                case AnchorType.BottomCenter:
                    SetAnchor(_cameraHandler.BottomCenter);
                    break;
                case AnchorType.BottomRight:
                    SetAnchor(_cameraHandler.BottomRight);
                    break;
                case AnchorType.MiddleLeft:
                    SetAnchor(_cameraHandler.MiddleLeft);
                    break;
                case AnchorType.MiddleCenter:
                    SetAnchor(_cameraHandler.MiddleCenter);
                    break;
                case AnchorType.MiddleRight:
                    SetAnchor(_cameraHandler.MiddleRight);
                    break;
                case AnchorType.TopLeft:
                    SetAnchor(_cameraHandler.TopLeft);
                    break;
                case AnchorType.TopCenter:
                    SetAnchor(_cameraHandler.TopCenter);
                    break;
                case AnchorType.TopRight:
                    SetAnchor(_cameraHandler.TopRight);
                    break;
            }
        }

        private Vector3 GetAnchor()
        {
            switch (anchorType)
            {
                case AnchorType.BottomLeft:
                    return _cameraHandler.BottomLeft;
                case AnchorType.BottomCenter:
                    return _cameraHandler.BottomCenter;
                case AnchorType.BottomRight:
                    return _cameraHandler.BottomRight;
                case AnchorType.MiddleLeft:
                    return _cameraHandler.MiddleLeft;
                case AnchorType.MiddleCenter:
                    return _cameraHandler.MiddleCenter;
                case AnchorType.MiddleRight:
                    return _cameraHandler.MiddleRight;
                case AnchorType.TopLeft:
                    return _cameraHandler.TopLeft;
                case AnchorType.TopCenter:
                    return _cameraHandler.TopCenter;
                case AnchorType.TopRight:
                    return _cameraHandler.TopRight;
                default:
                    return _cameraHandler.MiddleCenter;
            }
        }

        private void SetAnchor(Vector3 anchor)
        {
            Vector3 newPos = anchor + anchorOffset;
            if (!transform.position.Equals(newPos))
            {
                transform.position = newPos;
            }
        }

        public Vector3 Position => anchorOffset;
        public void SetPosition(Vector3 position)
        {
            anchorOffset = position;
            transform.position = GetAnchor() + anchorOffset;
        }
        public void SetPosition(Vector2 position)
        {
            anchorOffset = new Vector3(position.x, position.y, anchorOffset.z);
            transform.position = GetAnchor() + anchorOffset;
        }

        public void SetPositionFromWorld(Vector2 position)
        {
            anchorOffset = position - GetAnchor().ToVector2();
            transform.position = GetAnchor() + anchorOffset;
        }

        public void SetPositionFromWorld(Vector3 position)
        {
            anchorOffset = GetAnchor() - position;
            transform.position = GetAnchor() + anchorOffset;
        }

        public void SetScale(Vector3 scale)
        {
            transform.localScale = scale;
        }

        public void SetRotation(float angle)
        {
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

#if UNITY_EDITOR

        // Update is called once per frame
        void Update()
        {
            if (_cameraHandler == null)
                _cameraHandler = FindFirstObjectByType<CameraHandler>().GetComponent<CameraHandler>();

            if (_manualControl)
            {
                anchorOffset = GetAnchor() + transform.position;
            }
            else
            {
                UpdateAnchor();
            }
        }


#endif
    }
}