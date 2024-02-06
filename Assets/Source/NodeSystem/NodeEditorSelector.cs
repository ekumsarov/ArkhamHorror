#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using Sirenix.Utilities;

namespace EVI
{
    public class NodeEditorSelector : SerializedScriptableObject
    {
        [SerializeField, HideInInspector]
        protected string _parentFolder;
        public static NodeEditorSelector Create(string path, string missionName)
        {
            NodeEditorSelector temp = ScriptableObject.CreateInstance<NodeEditorSelector>();

            temp._parentFolder = path;
            temp.name = missionName;
            if (Directory.Exists(temp._parentFolder))
                return AssetDatabase.LoadAssetAtPath<NodeEditorSelector>(temp._parentFolder + "/" + temp.name);
            else
                Directory.CreateDirectory(temp._parentFolder);

            AssetDatabase.CreateAsset(temp, temp._parentFolder + "/" + temp.name + ".asset");
            return temp;
        }
    }
}
#endif