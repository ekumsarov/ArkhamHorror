using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EVI
{
    [Serializable]
    public class EncounterAction : LogicNode
    {
        protected Encounter _parentEncounter;
        protected void BindEncounter(Encounter encounter)
        {
            _parentEncounter = encounter;
        }
    }
}

