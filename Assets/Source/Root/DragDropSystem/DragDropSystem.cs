using UnityEngine;
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Zenject;

namespace EVI.DDSystem
{
    [Serializable]
    public class DragDropSystem : AutoFindMB, IDragController, IInitializable
    {
        [SerializeField, OnInspectorInit("UpdateComponents")] private List<Slot> _slots = new List<Slot>();

        public void Initialize()
        {

        }

        public void RegisterSlot(Slot container)
        {
            _slots.Add(container);
        }

        public bool TryFindSlotForItem(BaseView draggableItem, out Slot foundSlot)
        {
            foundSlot = null;

            // Здесь логика для поиска слота, например по позициям
            foreach (var slot in _slots)
            {
                if (SlotIsValidForItem(draggableItem, slot))
                {
                    foundSlot = slot;
                    return true;
                }
            }

            return false;
        }

        private bool SlotIsValidForItem(BaseView item, Slot slot)
        {
            // Получаем Bounds ячейки и перетаскиваемого объекта
            Bounds itemBounds = item.Bounds;
            Bounds slotBounds = slot.Bounds;

            // Проверяем, пересекается ли карточка с ячейкой
            bool isOverlapping = itemBounds.Intersects(slotBounds);
            return isOverlapping;
        }

        public void UnregisterSlot(Slot container)
        {

        }

        private void UpdateComponents()
        {
            _slots.Clear();

            foreach (var slot in GameObject.FindObjectsByType<Slot>(FindObjectsSortMode.None))
            {
                _slots.Add(slot);
            }
        }
    }
}
