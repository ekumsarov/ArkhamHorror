using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Sirenix.OdinInspector;

namespace EVI
{
    public class NodeSystem : MonoBehaviour, IUpdatable, IRegistrator
    {
        [Inject] private LevelDataHandler _reader;
        [Inject] private Instatinator _instatinator;
        [Inject] private IRegystryRoot _registrator;
        

        private List<LogicNode> _nodes;

        private LogicNode _currentNode;

        public void Initialize()
        {
            _nodes = _reader.Data.Data.Nodes;

            foreach (var node in _nodes)
            {
                _instatinator.QueueForInject(node);
            }

            _currentNode = _reader.Data.Data.RootNode;
            _currentNode.Enter(this);
        }

        public void NextNode(LogicNode node)
        {
            if(_currentNode != null)
            {
                _currentNode = null;
            }

            if(node != null)
            {
                _currentNode = node;
                _currentNode.Enter(this);
            }
            else
            {
                Debug.LogError("No entry other node");
            }
        }

        public void Tick(float deltaTime)
        {
            if (_currentNode != null && _currentNode)
                _currentNode.Tick(deltaTime);
        }


        public void Registry()
        {
            _registrator.Registry(this);
        }

        public void Unregistry()
        {
            _registrator.Unregisrty(this);
        }

        
    }
}

