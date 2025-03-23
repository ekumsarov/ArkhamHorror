using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.IO;
using UnityEditor;
using Sirenix.Utilities;
using System.Linq;
using Unity.VisualScripting;

namespace EVI
{
    public class SceneData : ScriptableObject
    {
        [SerializeField] private string _nodeName;
        [SerializeField] private NodeType _nodeType;

        [Button, PropertyOrder(-2)]
        public void UpdateData()
        {
            if(_nodes != null && _nodes.Count > 0)
            {
                _nodes.Clear();
            }

            _nodes = new List<LogicNode>();
            string dest = Directory.GetParent(AssetDatabase.GetAssetPath(this)).FullName;
            PathUtilities.TryMakeRelative(Path.GetDirectoryName(Application.dataPath), dest, out dest);

            List<string> check = Directory.GetFiles(dest).ToList();
            foreach (var file in check)
            {
                string tempFile = (file ?? "").Replace("\\", "/");
                var noda = AssetDatabase.LoadAssetAtPath<LogicNode>(tempFile);
                if(noda != null)
                    _nodes.Add(noda);
            }
        }

        public static SceneData ReadData(string path)
        {
            return Resources.Load<SceneData>("Data/Nodes/" + path + "/SceneData").Clone();
        }

        [Button, PropertyOrder(-1)]
        private void CreateNode()
        {
            if (string.IsNullOrEmpty(_nodeName))
                _nodeName = _nodeType.ToString();

            string dest = Directory.GetParent(AssetDatabase.GetAssetPath(this)).FullName;
            PathUtilities.TryMakeRelative(Path.GetDirectoryName(Application.dataPath), dest, out dest);

            //LogicNode.CreateNode(_nodeName, dest, _nodeType, UpdateData);
        }

        [SerializeField] private LogicNode _rootNode;
        public LogicNode RootNode => _rootNode == null ? _nodes[0] : _rootNode;

        [PropertySpace(spaceBefore: 64)]
        [SerializeField, ListDrawerSettings(HideRemoveButton = true, HideAddButton = true), InlineEditor] private List<LogicNode> _nodes;

        public List<LogicNode> Nodes => _nodes;

        private SceneData Clone()
        {
            return this.MemberwiseClone() as SceneData;
        }

    }
}