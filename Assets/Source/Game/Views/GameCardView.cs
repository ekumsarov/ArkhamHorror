using System.Collections;
using System.Collections.Generic;
using EVI.DDSystem;
using EVI.Game;
using TMPro;
using UnityEngine;
using Zenject;

namespace EVI
{
    public class GameCardView : TypedView<GameCard>
    {
        [Inject] private DragDropSystem _dragDropSystem;
        [Inject] private CameraHandler _camera;

        [SerializeField] private InteractableView _interactableView;

        [SerializeField] private TextMeshProUGUI _healthLabel;
        [SerializeField] private TextMeshProUGUI _mindLabel;

        private Slot _currentSlot;
        public Slot CurrentSlot => _currentSlot;

        public void SetDraggable(bool draggable)
        {
            _interactableView.SetDraggable(draggable);
        }

        protected override void InitializeBaseInternal()
        {
            base.InitializeBaseInternal();

            _interactableView.Initialize(_camera);
            _interactableView.OnEndDrag += OnEndDrag;
            _interactableView.OnBeginDrag += OnBeginDrag;

            _healthLabel.text = Model.HealthPoints.ToString();
            _mindLabel.text = Model.MindPoints.ToString();

            Model.SetupView(this);
            Model.OnDestroyed += DestroyView;
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

        [BindTo]
        private void HealthPointsChanged(int currentHealth)
        {
            _healthLabel.text = currentHealth.ToString();
        }

        [BindTo]
        private void MindPointsChanged(int currentHealth)
        {
            _mindLabel.text = currentHealth.ToString();
        }

        protected override void DestroyView(BaseModel model)
        {
            if(_currentSlot != null)
            {
                _currentSlot.RemoveItem(this);
            }

            base.DestroyView(model);
        }
    }
}

