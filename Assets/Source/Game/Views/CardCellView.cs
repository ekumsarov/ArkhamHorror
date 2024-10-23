using System.Collections;
using System.Collections.Generic;
using EVI.DDSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace EVI.Game
{
    [RequireComponent(typeof(Slot))]
    public class CardCellView : TypedView<CardCell>
    {
        [Inject] private readonly DragDropSystem _dragDropSystem;

        [SerializeField, OnInspectorInit("UpdateComponents")] private Slot _slot;
        [SerializeField, OnInspectorInit("UpdateComponents")] private SceneLayout _layout;

        public string Identifier => this.name;

        private List<GameCardView> _cards = new List<GameCardView>();

        protected override void InitializeBaseInternal()
        {
            base.InitializeBaseInternal();
            Model.SetupView(this);

            _slot.ItemPlaced += PlaceInCell;
            _slot.ItemRemoved += RemoveFromCell;
            _slot.ItemCovered += CoveredByCard;

            _layout.Rebuild();
            _dragDropSystem.RegisterSlot(_slot);

            foreach(var card in Model.Cards)
            {
                _slot.TryPlaceItem(card.View);
            }
        }

        private void UpdateComponents()
        {
            if(_slot == null)
                _slot = GetComponent<Slot>();

            if(_layout == null)
                _layout = GetComponent<SceneLayout>();
        }

        public void RebuildLayout()
        {
            _layout.Rebuild();
        }

        public bool PlaceInCell(BaseView view)
        {
            GameCardView gameCard = view as GameCardView;
            if(gameCard == null)
                return false;

            if(_cards.Contains(gameCard))
                return false;

            if(_slot.AllowMultipleItems == false && _cards.Count > 0)
            {
                GameCardView currentGameCardView = _cards[0];
                _cards.Remove(currentGameCardView);
                gameCard.CurrentSlot.TryPlaceItem(currentGameCardView);
            }
                

            _cards.Add(gameCard);
            gameCard.SetParentSlot(_slot);
            gameCard.transform.SetParent(transform);
            _layout.Rebuild();      

            return true;
        }

        public void RemoveFromCell(BaseView view)
        {
            GameCardView gameCard = view as GameCardView;
            if(gameCard == null)
                return;

            if(_cards.Contains(gameCard) == false)
                return;

            _cards.Remove(gameCard);
            gameCard.transform.SetParent(null);
        }

        public void CoveredByCard(BaseView view)
        {
            GameCardView gameCard = view as GameCardView;
            if(gameCard == null)
                return;

            
        }
    }
}