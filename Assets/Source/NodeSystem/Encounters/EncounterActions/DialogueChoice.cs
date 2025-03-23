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
        public string DisplayedText => _text.GetLocalizedString();

        public void Evaluate(DialoguePanel panel)
        {
            // Для Simple и Condition можно сразу set
            // Для SkillCheck может быть "отложенно"
            _selectedNode = null;

            if (_checkType == ChoiceCheckType.Simple)
            {
                _selectedNode = _succesNextNode;
            }
            else if (_checkType == ChoiceCheckType.Condition)
            {
                bool conditionResult = true; // Допустим
                _selectedNode = conditionResult ? _succesNextNode : _failNextNode;
            }
        }

        public LogicNode FinalizeChoice(bool success)
        {
            _selectedNode = success ? _succesNextNode : _failNextNode;
            return _selectedNode;
        }

        [OnInspectorInit]
        private void OnInspectorInit()
        {
            if (_rule == null)
                _rule = new SkillCheckRule();
        }
    }
}