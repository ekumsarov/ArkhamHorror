using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EVI
{
    public class BaseUI : MonoBehaviour
    {
        [SerializeField, OnInspectorInit("UpdateComponents")] private RectTransform _rectTransform;
        public RectTransform RectTransform => _rectTransform;

        [SerializeField, OnInspectorInit("UpdateComponents")] private RectTransform _parentRectTransform;
        private Vector2? _cachedParentSize;
        private Vector2? _cachedScreenSize;

        private void UpdateComponents()
        {
            if (_rectTransform == null)
            {
                _rectTransform = GetComponent<RectTransform>();
            }

            if (_rectTransform.parent != null)
            {
                _parentRectTransform = _rectTransform.parent as RectTransform;
            }
        }

        // Добавление свойств для получения углов RectTransform
        public Vector2 TopLeft => GetWorldCornerPosition(1);  // TopLeft is index 1
        public Vector2 TopRight => GetWorldCornerPosition(2); // TopRight is index 2
        public Vector2 BottomLeft => GetWorldCornerPosition(0); // BottomLeft is index 0
        public Vector2 BottomRight => GetWorldCornerPosition(3); // BottomRight is index 3

        // Метод для получения позиции угла
        private Vector2 GetWorldCornerPosition(int index)
        {
            Vector3[] corners = new Vector3[4];
            RectTransform.GetWorldCorners(corners);
            return corners[index];
        }

        public Vector2 GetCenterToWorldPosition()
        {
            Vector3 worldPosition = _rectTransform.TransformPoint(_rectTransform.rect.center);

            return worldPosition;
        }

        // Преобразование позиции между двумя BaseUI
        public Vector2 TransformPositionTo(BaseUI targetBaseUI, Vector2 position)
        {
            Vector2 worldPosition = RectTransform.TransformPoint(position);
            return targetBaseUI.RectTransform.InverseTransformPoint(worldPosition);
        }

        public Vector2 TransformPositionFrom(BaseUI sourceBaseUI, Vector2 position)
        {
            Vector2 worldPosition = sourceBaseUI.RectTransform.TransformPoint(position);
            return RectTransform.InverseTransformPoint(worldPosition);
        }

        // Получение позиции угла относительно другого BaseUI
        public Vector2 GetCornerPositionIn(BaseUI targetBaseUI, int cornerIndex)
        {
            Vector2 cornerPosition = GetWorldCornerPosition(cornerIndex);
            return targetBaseUI.RectTransform.InverseTransformPoint(cornerPosition);
        }

        // Пример использования углов для установки позиции
        public void SetAnchoredPositionUsingCorner(int cornerIndex, Vector2 offset = default)
        {
            Vector2 cornerPosition = GetWorldCornerPosition(cornerIndex);
            Vector3 adjustedCornerPosition = new Vector3(cornerPosition.x + offset.x, cornerPosition.y + offset.y, 0f);
            RectTransform.anchoredPosition = RectTransform.InverseTransformPoint(adjustedCornerPosition);
        }

        public Vector2 GetWorldPosition(Vector2 normalizedPosition)
        {
            if (_rectTransform == null)
            {
                Debug.LogError("RectTransform должен быть установлен.");
                return Vector2.zero;
            }

            // Получаем размер RectTransform в пикселях
            Vector2 rectSize = _rectTransform.rect.size;

            // Рассчитываем абсолютную позицию внутри RectTransform на основе нормализованной позиции
            Vector2 localPosition = new Vector2(normalizedPosition.x * rectSize.x, normalizedPosition.y * rectSize.y);

            // Преобразуем локальную позицию в мировые координаты
            Vector3 worldPosition = _rectTransform.TransformPoint(localPosition);

            return worldPosition;
        }

        public void AddUI(BaseUI uiTransform)
        {
            uiTransform.SetParent(this);
        }

        public void SetParent(BaseUI baseUI)
        {
            RectTransform.SetParent(baseUI.RectTransform);
            _parentRectTransform = baseUI.RectTransform;
        }

        public void SetRelativePosition(Vector2 normalizedPosition)
        {
            if (_parentRectTransform == null)
            {
                Debug.LogError("RectTransform должен иметь родителя.");
                return;
            }

            if (!_cachedParentSize.HasValue)
            {
                _cachedParentSize = _parentRectTransform.rect.size;
            }

            Vector2 absolutePosition = new Vector2(normalizedPosition.x * _cachedParentSize.Value.x, normalizedPosition.y * _cachedParentSize.Value.y);

            Vector2 pivotOffset = new Vector2(RectTransform.rect.width * RectTransform.pivot.x, RectTransform.rect.height * RectTransform.pivot.y);
            Vector2 adjustedPosition = absolutePosition - pivotOffset;

            RectTransform.localPosition = adjustedPosition;
        }

        public void SetPositionOnScreen(Vector2 normalizedPosition)
        {
            if (!_cachedScreenSize.HasValue)
            {
                _cachedScreenSize = new Vector2(Screen.width, Screen.height);
            }

            Vector2 absolutePosition = new Vector2(normalizedPosition.x * _cachedScreenSize.Value.x, normalizedPosition.y * _cachedScreenSize.Value.y);

            Vector2 pivotOffset = new Vector2(RectTransform.rect.width * RectTransform.pivot.x, RectTransform.rect.height * RectTransform.pivot.y);
            Vector2 adjustedPosition = absolutePosition - pivotOffset;

            RectTransform.position = adjustedPosition;
        }

        public void CenterToClick(PointerEventData eventData)
        {
            Vector2 clickPosition = eventData.position;

            Vector2 pivotOffset = new Vector2(RectTransform.rect.width * RectTransform.pivot.x, RectTransform.rect.height * RectTransform.pivot.y);
            Vector2 adjustedPosition = clickPosition - pivotOffset;

            RectTransform.position = adjustedPosition;
        }

        public void SetPositionAtWorldPoint(Vector3 worldPoint)
        {
            if (_rectTransform == null)
            {
                Debug.LogError("RectTransform должен быть установлен.");
                return;
            }

            Vector3 localPoint = _rectTransform.InverseTransformPoint(worldPoint);
            RectTransform.anchoredPosition = localPoint;
        }

        public void SetWorldPosition(Vector3 worldPosition)
        {
            if (_rectTransform == null)
            {
                Debug.LogError("RectTransform должен быть установлен.");
                return;
            }

            _rectTransform.position = worldPosition;
        }

        public Vector3 GetWorldPositionFromAnchoredPosition()
        {
            if (_rectTransform == null)
            {
                Debug.LogError("RectTransform должен быть установлен.");
                return Vector3.zero;
            }

            return _rectTransform.TransformPoint(_rectTransform.anchoredPosition);
        }

        public void SetAnchoredPositionAtScreenPoint(Vector2 screenPoint)
        {
            if (_rectTransform == null)
            {
                Debug.LogError("RectTransform должен быть установлен.");
                return;
            }

            Vector2 pivotOffset = new Vector2(_rectTransform.rect.width * _rectTransform.pivot.x, _rectTransform.rect.height * _rectTransform.pivot.y);
            Vector2 adjustedPosition = screenPoint - pivotOffset;

            _rectTransform.position = adjustedPosition;
        }

        public void SetCenteredAnchoredPosition(Vector2 targetPosition)
        {
            if (_rectTransform == null)
            {
                Debug.LogError("RectTransform должен быть установлен.");
                return;
            }

            Vector2 size = new Vector2(_rectTransform.rect.width, _rectTransform.rect.height);
            Vector2 pivotOffset = new Vector2(size.x * _rectTransform.pivot.x, size.y * _rectTransform.pivot.y);

            _rectTransform.anchoredPosition = targetPosition - pivotOffset;
        }

        public Vector2 GetLocalPositionFromWorld(Vector3 worldPosition)
        {
            if (_rectTransform == null)
            {
                Debug.LogError("RectTransform должен быть установлен.");
                return Vector2.zero;
            }

            return _rectTransform.InverseTransformPoint(worldPosition);
        }

        public void InvalidateCache()
        {
            _cachedParentSize = null;
            _cachedScreenSize = null;
        }

        private void OnRectTransformDimensionsChange()
        {
            InvalidateCache();
        }

        private void OnEnable()
        {
            UpdateComponents();
            InvalidateCache();
        }
    }
}
