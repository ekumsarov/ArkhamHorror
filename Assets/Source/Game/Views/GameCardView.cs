using System.Collections;
using System.Collections.Generic;
using EVI.DDSystem;
using EVI.Game;
using UnityEngine;
using Zenject;

namespace EVI
{
    public class GameCardView : TypedView<GameCard>
    {
        [Inject] private DragDropSystem _dragDropSystem;
        [Inject] private CameraHandler _camera;

        [SerializeField] private InteractableView _interactableView;

        private Slot _currentSlot;
        public Slot CurrentSlot => _currentSlot;

        protected override void InitializeBaseInternal()
        {
            base.InitializeBaseInternal();

            _interactableView.Initialize(_camera);
            _interactableView.OnEndDrag += OnEndDrag;
            _interactableView.OnBeginDrag += OnBeginDrag;
            Model.SetupView(this);
        }

        private void OnBeginDrag()
        {
            if (_currentSlot != null)
            {
                // Оповещаем ячейку, что объект покинул её
                _currentSlot.RemoveItem(this);
            }
        }

        private void OnEndDrag()
        {
            // Ищем подходящую ячейку для размещения
            if (_dragDropSystem.TryFindSlotForItem(this, out Slot newSlot))
            {
                if (newSlot.TryPlaceItem(this))
                {
                    Debug.Log("Card successfully placed in the slot.");
                    return;
                }
                
            }
            _currentSlot.TryPlaceItem(this);
            Debug.Log("Failed to place card. Handling conflict or switch.");
            
        }

        public void SetParentSlot(Slot slot)
        {
            _currentSlot = slot;
        }
    }
}

