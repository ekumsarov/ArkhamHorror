using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EVI.Game
{
    [JSONSerializable]
    public class GameCard : BaseModel
    {
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

