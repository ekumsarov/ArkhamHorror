using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EVI.Game
{
    [JSONSerializable, CreateAssetMenu(menuName = "Models/Card")]
    public class GameCard : TypedModel<GameCardView>
    {
        [SerializeField, JSONConvert] private GameObject _prefab;
        public GameObject Prefab => _prefab;

        [SerializeField, JSONConvert]
        private CardType _cardType; 

        [SerializeField, JSONConvert]
        private SkillGroup _skills;

        [SerializeField, JSONConvert]
        private int _healthPoints;

        [SerializeField, JSONConvert]
        private int _mindPoints;

        public CardType CardType => _cardType;

        public bool CheckSkill(SkillType skill)
        {
            return _skills.CheckSkill(skill);
        }
    }
}

