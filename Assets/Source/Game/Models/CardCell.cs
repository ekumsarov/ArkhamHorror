using EVI.Game;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace EVI
{
    [Serializable, JSONSerializable, CreateAssetMenu(menuName = "Models/CardCell")]
    public class CardCell : TypedModel<CardCellView>
    {
        [SerializeField, JSONConvert]
        private GameObject _prefab;

        [SerializeField, JSONConvert]
        private CardType _cellType;

        [SerializeField, JSONConvert, ValueDropdown("@EVI.GameCardSelector.GetAllGameCardIDs()")]
        private List<string> _cards;

        [SerializeField, JSONConvert]
        private bool _isAllowedMultiple = false;

        private List<GameCard> _actualCards;

        public CardType CellType => _cellType;
        public GameObject Prefab => _prefab;
        public List<GameCard> Cards => _actualCards;

        [Inject] private readonly ContainerSystem _containerSystem;

        protected override void InitializeBaseExternal()
        {
            base.InitializeBaseExternal();

            _actualCards = new List<GameCard>();

            foreach(var cardID in _cards)
            {
                GameCard actualCard = _containerSystem.GetObject<GameCard>(cardID);
                if(actualCard != null)
                {
                    _actualCards.Add(actualCard);
                }
            }
        }
    }
}
