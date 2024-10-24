using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace EVI.Game
{
    [JSONSerializable]
    public class GameCard : TypedModel<GameCardView>
    {
        [Inject] private readonly ContainerSystem _containerSystem;

        [SerializeField, JSONConvert] private GameObject _prefab;
        public GameObject Prefab => _prefab;

        [SerializeField, JSONConvert]
        private CardType _cardType; 

        [SerializeField, JSONConvert]
        private SkillGroup _skills;

        [SerializeField, JSONConvert, OnValueChanged("GameCardUpdate"), BindableProperty]
        private int _healthPoints;

        [SerializeField, JSONConvert, OnValueChanged("GameCardUpdate"), BindableProperty]
        private int _mindPoints;

        [SerializeField, JSONConvert, OnInspectorInit("GameCardUpdate")]
        private int _currentHealthPoints;

        [SerializeField, JSONConvert, OnInspectorInit("GameCardUpdate")]
        private int _currentMindPoints;

        [SerializeField, JSONConvert]
        private int _healthDamage;

        [SerializeField, JSONConvert]
        private int _mindDamage;

        public CardType CardType => _cardType;

        public int HealthPoints => _currentHealthPoints;
        public int MindPoints => _currentMindPoints;

        public int MaxHealthPoints => _healthPoints;
        public int MaxMindPoints => _mindPoints;

        public int HealthDamage => _healthDamage;
        public int MindDamage => _mindDamage;

        public bool CheckSkill(SkillType skill)
        {
            return _skills.CheckSkill(skill);
        }

        public void HealHealth(int amount)
        {
            _currentHealthPoints += amount;
            if(_currentHealthPoints > _healthPoints)
                _currentHealthPoints = _healthPoints;

            InvokeChange<int>(nameof(_healthPoints), _currentHealthPoints);
        }

        public void HealMind(int amount)
        {
            _currentMindPoints += amount;
            if(_currentMindPoints > _mindPoints)
                _currentMindPoints = _mindPoints;

            InvokeChange<int>(nameof(_mindPoints), _currentMindPoints);
        }

        public void DamageHealth(int amount)
        {
            _currentHealthPoints -= amount;
            if(_currentHealthPoints <= 0)
            {
                _currentHealthPoints = 0;
                
                InvokeChange<int>(nameof(_healthPoints), _currentHealthPoints);
                _containerSystem.RemoveObject<GameCard>(this.ID);
                DestroyModel();
                return;
            }
                
            InvokeChange<int>(nameof(_healthPoints), _currentHealthPoints);
        }

        public void DamageMind(int amount)
        {
            _currentMindPoints -= amount;
            if(_currentMindPoints < 0)
                _currentMindPoints = 0;

            InvokeChange<int>(nameof(_mindPoints), _currentMindPoints);
        }

        private void GameCardUpdate()
        {
            _currentHealthPoints = _healthPoints;
            _currentMindPoints = _mindPoints;
        }
    }
}

