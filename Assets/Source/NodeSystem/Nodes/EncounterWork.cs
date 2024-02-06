using EVI.Game;
using SimpleJSON;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EVI
{
    public class EncounterWork : LogicNode
    {
        [SerializeField, InlineEditor]
        private Encounter _encounter;

        [SerializeField]
        private Location _location;
    }
}