using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using Zenject;
using Sirenix.OdinInspector;

namespace EVI
{
    public partial class EncounterNode : LogicNode
    {
        [SerializeField]
        private LocalizedString _text;

        public string Text => _text.GetLocalizedString();

        [SerializeField]    
        public List<DialogueChoice> Choices;

        [SerializeField, ReadOnly] private List<LogicNode> _subNodes;

        #region Logic

        [Inject] private DialoguePanel _ui;

        protected override void EnterExternal()
        {
            _ui.OpenDialogue(this);
        }

        #endregion
    }
}

