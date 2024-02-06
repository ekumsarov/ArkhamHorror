using EVI.Game;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using System.Linq;
using Sirenix.Serialization;

namespace EVI
{
    public class Encounter : SerializedScriptableObject
    {
        [SerializeField]
        private List<CardCell> _cells;

        [SerializeField]
        private List<EncounterAction> _actions;

        [SerializeField]
        private EncounterActionType _encounterType = EncounterActionType.Dialogue;

        [SerializeField, ShowIf("@_encounterType == EncounterActionType.Dialogue")]
        private LocalizedString _mainText;

        [SerializeField, ShowIf("@_encounterType == EncounterActionType.Dialogue")]
        private List<DialogueChoice> _choices;

        public List<GameCard> DetectiveCards()
        {
            return null;
        }    

        public static Encounter Create()
        {
            Encounter temp = ScriptableObject.CreateInstance<Encounter>();

            temp._cells = new List<CardCell>();

            return temp;
        }


        private Action _callback;
        public void StartEvent(Action callback)
        {
            _callback = callback;
            EndEvent();
        }

        public void EndEvent()
        {
            foreach(var cell in _cells)
            {

            }
            _callback?.Invoke();
        }

        public EncounterAction GetEncounter()
        {
            if (_encounterType == EncounterActionType.Dialogue)
                return DialogueEncounter.Create(_mainText, _choices);

            return null;
        }
    }
}

