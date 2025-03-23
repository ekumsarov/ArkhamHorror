using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using UnityEditor;
using System.IO;

namespace EVI
{
    [Serializable]
    public class LogicNode : ScriptableObject
    {
        [SerializeField, HideIf("@this.GetType().IsSubclassOf(typeof(LogicNode))"), HideInTables] private NodeType _type = NodeType.Empty;
        [SerializeField] protected LogicNode _nextNode;
        public NodeType NodeType => _type;
        protected NodeSystem _parentSystem;

        private INodeContainer _parent;

        public void Initialize(INodeContainer nodeContainer, NodeSystem parent)
        {
            _parentSystem = parent;
            _parent = nodeContainer;
        }

        public T GetParentAs<T>() where T : class, INodeContainer
        {
            var typed = _parent as T;
            if (typed == null)
            {
                throw new InvalidOperationException(
                    $"{GetType().Name} требует {typeof(T).Name} в качестве INodeContainer, " +
                    $"но {_parent?.GetType().Name ?? "null"} не подходит.");
            }
            return typed;
        }

        protected virtual void EnterExternal() { Exit(); }

        public void Enter()
        {
            EnterExternal();
        }

        public virtual void Tick(float deltaTime)
        {

        }

        protected virtual void Exit()
        {
            _parentSystem.NextNode(_nextNode);
        }

    }
}

