using UnityEngine;
using UnityEngine.EventSystems;

namespace EVI
{
    public class DraggableWindow : BaseUI, IPointerDownHandler, IDragHandler
    {
        private Vector2 _lastMousePosition;

        public void OnPointerDown(PointerEventData eventData)
        {
            _lastMousePosition = eventData.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (RectTransform == null)
                return;

            Vector2 currentMousePosition = eventData.position;
            Vector2 diff = currentMousePosition - _lastMousePosition;
            RectTransform.anchoredPosition += diff;

            _lastMousePosition = currentMousePosition;
        }
    }
}
