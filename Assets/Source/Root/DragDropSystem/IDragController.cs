using UnityEngine;

namespace EVI.DDSystem
{
    public interface IDragController
    {
        void RegisterSlot(Slot container);
        void UnregisterSlot(Slot container);
        bool TryFindSlotForItem(BaseView draggableItem, out Slot foundContainer);
    }
}