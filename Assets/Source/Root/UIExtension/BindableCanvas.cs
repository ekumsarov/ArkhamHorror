using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EVI
{
    public class BindableCanvas : MonoBehaviour
    {
        [SerializeField, ReadOnly, OnInspectorInit("OnInspectorInit")]
        private List<BindableUI> _allItems;
        
        private void OnInspectorInit()
        {
            if (_allItems == null)
                _allItems = new List<BindableUI>();

            _allItems.Clear();
            foreach(var childs in GetComponentsInChildren<BindableUI>())
            {
                _allItems.Add(childs);
                childs.PrepareBindings();
            }
        }
    }
}