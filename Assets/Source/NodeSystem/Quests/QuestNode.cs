using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EVI
{
    public class QuestNode : SerializedScriptableObject
    {
        [SerializeField, ReadOnly]
        private List<LogicNode> _nodes;

        [SerializeField, HideInInspector]
        private string _path;

        public static QuestNode Create(string name, string path)
        {
            QuestNode temp = ScriptableObject.CreateInstance<QuestNode>();

            temp.name = name;
            temp._path = path;

            return temp;
        }

        private void UpdataData()
        {
            if (_nodes != null && _nodes.Count > 0)
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
                if (noda != null)
                    _nodes.Add(noda);
            }
        }

        [Button]
        private void CreateNode(NodeType type)
        {
            if (_nodes == null)
                _nodes = new List<LogicNode>();

            string dest = Directory.GetParent(AssetDatabase.GetAssetPath(this)).FullName;
            PathUtilities.TryMakeRelative(Path.GetDirectoryName(Application.dataPath), dest, out dest);
            int index = 0;
            List<string> check = Directory.GetFiles(dest).ToList();
            foreach (var file in check)
            {
                string tempFile = (file ?? "").Replace("\\", "/");
                var noda = AssetDatabase.LoadAssetAtPath<LogicNode>(tempFile);
                if (noda != null)
                    index++;
            }


            _nodes.Add(LogicNode.CreateNode(type.ToString() + name + index.ToString(), _path, type, UpdataData));
            
        }
    }
}

