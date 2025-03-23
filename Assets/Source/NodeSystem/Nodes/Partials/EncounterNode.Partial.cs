#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using UnityEditor;

namespace EVI
{
    public partial class EncounterNode : LogicNode
    {
        #region SubNodes

        [Button("Создать саб-нод")]
        private void CreateSubNode(
            [ValueDropdown(nameof(GetNodeTypes))] Type nodeType,
            string nodeName = "NewSubNode"
        )
        {
            if (nodeType == null)
            {
                Debug.LogWarning("Не выбран тип ноды!");
                return;
            }

            // Создаём ScriptableObject указанного типа
            var newNode = ScriptableObject.CreateInstance(nodeType) as LogicNode;
            if (newNode == null)
            {
                Debug.LogError($"Не удалось привести {nodeType.Name} к LogicNode!");
                return;
            }

            newNode.name = nodeName;
            // Добавляем его как саб-объект к текущему объекту (this)
            AssetDatabase.AddObjectToAsset(newNode, this);
            _subNodes.Add(newNode);

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"Создан саб-нод {newNode.name} внутри {this.name}");
        }

        [Button]
        private void RemoveNode(LogicNode node)
        {
            
            if(_subNodes.Contains(node) == false)
                return;

            var subAsset = node;
            if (subAsset != null)
            {
                _subNodes.Remove(node);
                AssetDatabase.RemoveObjectFromAsset(subAsset);
                EditorUtility.SetDirty(this);
                AssetDatabase.SaveAssets();
                Debug.Log($"Удалён саб-ассет {subAsset.name} из {this.name}");
            }
        }

        public bool ContainsNode(LogicNode node)
        {
            return _subNodes.Contains(node);
        }

        public void AddSubNode(LogicNode node)
        {
            AssetDatabase.AddObjectToAsset(node, this);
            _subNodes.Add(node);
            EditorUtility.SetDirty(this);
        }

        /// <summary>
        /// Возвращаем список типов, унаследованных от LogicNode (по аналогии с FolderItem.GetNodeTypes).
        /// </summary>
        private static IEnumerable<ValueDropdownItem<Type>> GetNodeTypes()
        {
            return LogicNodeReflection.GetAllLogicNodeTypes();
        }

        #endregion
    }
}
#endif
