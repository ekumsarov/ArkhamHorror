using UnityEngine;

public interface IInteractable
{
    void HandleClick();
    void HandleBeginDrag();
    void HandleDrag(Vector2 mousePosition);
    void HandleEndDrag();
    bool IsDraggable { get; }
    bool IsButton { get; }
}
