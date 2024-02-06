using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EVI.Game
{
    public class GameCard : BaseModel
    {
        private GameCardData _data; 

        [BindableProperty]
        public int HealthPoints { get; private set; }

        [BindableProperty]
        public int MindPoints { get; private set; }

        public CardType CardType  => _data.CardType; 

        private SkillGroup _skills;

        private void Initialize(GameCardData data)
        {
            _data = data;
            HealthPoints = _data.HealthPoints;
            MindPoints = _data.MindPoints;
            _skills = data.Skills;
        }

        protected override void InitializeParameter()
        {
            InitParameter<GameCardData>(Initialize);
        }

        public bool CheckSkill(SkillType skill)
        {
            return _skills.CheckSkill(skill);
        }
    }
}

