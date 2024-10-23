using System;
using Sirenix.OdinInspector;
using UnityEngine;
using EVI.DDSystem;

namespace EVI
{
    public class CardLayout : SceneLayout
    {
        [SerializeField, OnInspectorInit("UpdateComponents")] private Slot _slot;

        private void UpdateComponents()
        {
            if(_slot == null)
                _slot = GetComponent<Slot>();
        }
    }
}