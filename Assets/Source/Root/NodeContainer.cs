using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace EVI
{
    [Serializable]
    public abstract class NodeContainer
    {
        [Inject] protected NodeSystem _nodeSystem;
        [SerializeField] private LogicNode _entryNode;

        private LogicNode _poppedUpNode = null;

        public void Init()
        {
            
        }

        public void Execute()
        {
            if(_entryNode == null)
            {
                return;
            }

            _nodeSystem.NextNode(_entryNode);
        }

        public void SetEntryNode(LogicNode node)
        {
            _entryNode = node;
        }

        public void PopupNode(LogicNode node)
        {

        }
    }
}