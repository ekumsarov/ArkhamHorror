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

        protected virtual void EnterExternal() { Exit(); }

        public void Enter(NodeSystem parent)
        {
            _parentSystem = parent;
            EnterExternal();
        }

        public virtual void Tick(float deltaTime)
        {
            
        }

        protected virtual void Exit()
        {
            _parentSystem.NextNode(_nextNode);
        }

        #region Nodes

        public static LogicNode CreateNode(string name, string path, NodeType type, Action updateCallback)
        {
            if (type == NodeType.EncounterWork)
            {
                EncounterWork temp = ScriptableObject.CreateInstance<EncounterWork>();
                temp.name = name;
                temp._updateCallback = updateCallback;
                AssetDatabase.CreateAsset(temp, path + "/" + temp.name + ".asset");
                return temp;
            }

            updateCallback?.Invoke();
            return null;
        }

        [SerializeField, HideInInspector]
        private Action _updateCallback;

        [Button]
        private void Remove()
        {
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(this));
            _updateCallback?.Invoke();
            //AssetDatabase.RemoveObjectFromAsset(this);
        }

        #endregion


    }
}

