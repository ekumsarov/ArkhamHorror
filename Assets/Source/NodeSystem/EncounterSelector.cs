using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

namespace EVI
{
    public class EncounterSelector : NodeEditorSelector
    {
        public enum EncounerPath
        {
            Base,
            Quest,
            Unique
        }

        public static new EncounterSelector Create(string path, string missionName)
        {
            EncounterSelector temp = ScriptableObject.CreateInstance<EncounterSelector>();

            temp._parentFolder = path;
            temp.name = missionName;
            if (Directory.Exists(temp._parentFolder))
                return AssetDatabase.LoadAssetAtPath<EncounterSelector>(temp._parentFolder + "/" + temp.name);
            else
                Directory.CreateDirectory(temp._parentFolder);

            AssetDatabase.CreateAsset(temp, temp._parentFolder + "/" + temp.name + ".asset");
            return temp;
        }

        [Button]
        private void CreateEncounter(EncounerPath path, string encounterName)
        {

            if (Directory.Exists(_parentFolder + "/" + path.ToString()) == false)
            {
                Directory.CreateDirectory(_parentFolder + "/" + path.ToString());
            }

            if(File.Exists(_parentFolder + "/" + path.ToString() + "/" + encounterName + ".asset"))
            {
                Debug.LogError("File exist with name " + encounterName);
            }

            var obj = Encounter.Create();
            AssetDatabase.CreateAsset(obj, _parentFolder + "/" + path.ToString() + "/" + encounterName + ".asset");
            AssetDatabase.Refresh();
        }
    }
}