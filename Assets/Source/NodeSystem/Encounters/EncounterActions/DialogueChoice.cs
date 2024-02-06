using EVI.Game;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;

namespace EVI
{
    [Serializable]
    public class DialogueChoice 
    {
        public enum ChoiceCheckType
        {
            Simple,
            SkillCheck,
            Condition
        }

        [SerializeField]
        private LocalizedString _text;

        [SerializeField]
        private LogicNode _succesNextNode;

        [SerializeField]
        private ChoiceCheckType _checkType;

        [SerializeField, ShowIf("@_checkType == ChoiceCheckType.SkillCheck")]
        private LogicNode _failNextNode;

        [SerializeField, ShowIf("@_checkType == ChoiceCheckType.SkillCheck")]
        private SkillCheckRule _rule;

        private LogicNode _selectedNode = null;
        public LogicNode NextNode => _selectedNode != null ? _selectedNode : _succesNextNode;

        private Encounter _encounter;
        public void BindEncounter(Encounter encounter)
        {
            _encounter = encounter;
        }

        public void Clicked(DialoguePanel panel)
        {
            _selectedNode = null;

            if(_checkType == ChoiceCheckType.SkillCheck)
            {
                List<GameCard> detectives = _encounter.DetectiveCards().Where(card => card.CardType == CardType.Player).ToList();



                return;
            }


        }

        [OnInspectorInit]
        private void OnInspectorInit()
        {
            if (_rule == null)
                _rule = new SkillCheckRule();
        }
    }
}