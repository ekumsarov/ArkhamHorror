#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using System.Linq;
using Sirenix.OdinInspector;
using System.IO;
using System;
using static UnityEditor.Progress;

namespace EVI
{
    public class NodeEditor : OdinMenuEditorWindow
    {
        private OdinMenuTree _tree;

        [MenuItem("Tools/NodeEditor")]
        private static void Open()
        {
            var window = GetWindow<NodeEditor>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            BuildCampaign();

            _tree = new OdinMenuTree(true);
            _tree.DefaultMenuStyle.IconSize = 28.00f;
            _tree.Config.DrawSearchToolbar = true;

            _tree.Add("Main", this);

            if (Directory.Exists(_folderPath + "Rules") == false)
            {
                NodeEditorSelector.Create(_folderPath + "Rules", "Rules");
            }

            if (Directory.Exists(_folderPath + "Quest") == false)
            {
                QuestSelector.Create(_folderPath + "Quest", "Quest");
            }

            if (Directory.Exists(_folderPath + "Encounters") == false)
            {
                EncounterSelector.Create(_folderPath + "Encounters", "Encounters");
            }

            if (File.Exists(_folderPath + "CampaignData.asset") == false)
            {
                CampaignData temp = ScriptableObject.CreateInstance<CampaignData>();
                AssetDatabase.CreateAsset(temp, _folderPath + "CampaignData.asset");
                _tree.Add("Configuration", temp);
            }
            else
            {
                CampaignData temp = AssetDatabase.LoadAssetAtPath<CampaignData>(_folderPath + "CampaignData.asset");
                _tree.Add("Configuration", temp);
            }

            _tree.AddAllAssetsAtPath("", _folderPath, typeof(NodeEditorSelector), includeSubDirectories: true, flattenSubDirectories: true);
            List<OdinMenuItem> items = _tree.EnumerateTree().Where(x => x.Value as NodeEditorSelector).ToList();
            foreach (var item in items)
            {
                foreach (var directory in Directory.GetDirectories(_folderPath + item.Name))
                {
                    DirectoryInfo info = new DirectoryInfo(directory);
                    _tree.AddAssetAtPath(item.Name + "/" + info.Name, _folderPath + item.Name + "/" + info.Name + "/QuestContainer.asset", typeof(QuestContainer));
                    _tree.AddAllAssetsAtPath(item.Name + "/" + info.Name, _folderPath + item.Name + "/" + info.Name + "/", typeof(Encounter)).ForEach(AddDragHandles);
                    foreach (var subDir in Directory.GetDirectories(directory))
                    {
                        DirectoryInfo subInfo = new DirectoryInfo(subDir);
                        string sub = item.Name + "/" + info.Name + "/" + subInfo.Name;
                        _tree.AddAssetAtPath(sub, _folderPath + item.Name + "/" + info.Name + "/" + subInfo.Name + "/" + subInfo.Name + ".asset", typeof(QuestNode)).ForEach(AddDragHandles);
                        _tree.AddAllAssetsAtPath(sub, _folderPath + item.Name + "/" + info.Name + "/" + subInfo.Name, typeof(LogicNode)).ForEach(AddDragHandles);
                    }
                }

            }
            // Add drag handles to items, so they can be easily dragged into the inventory if characters etc...
            //_tree.EnumerateTree().Where(x => x.Value as SceneData).ForEach(AddDragHandles);

            return _tree;
        }

        private void AddItem(OdinMenuItem menuItem)
        {

        }

        private void AddDragHandles(OdinMenuItem menuItem)
        {
            menuItem.OnDrawItem += x => DragAndDropUtilities.DragZone(menuItem.Rect, menuItem.Value, false, false);
        }

        /*protected override void OnBeginDrawEditors()
        {
            var selected = this.MenuTree.Selection.FirstOrDefault();
            var toolbarHeight = this.MenuTree.Config.SearchToolbarHeight;

            // Draws a toolbar with the name of the currently selected menu item.
            SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
            {
                if (selected != null)
                {
                    GUILayout.Label(selected.Name);
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }*/

        #region Setup

        private void BuildCampaign()
        {
            if (_folderPath.IsNullOrWhitespace() == false)
                return;

            _folderPath = _parentFolder + _campaign.ToString() + "/";

            AssetDatabase.Refresh();
            if (Directory.Exists(_folderPath) == false)
            {
                Directory.CreateDirectory(_folderPath);
            }
        }

        private string _parentFolder = "Assets/Resources/Data/Nodes/";
        private string _folderPath;
        [SerializeField, OnValueChanged("RebuildCampaign"), OnInspectorInit("RebuildCampaign")] private Campaings _campaign = Campaings.Empty;

        private void RebuildCampaign()
        {
            
            _folderPath = _parentFolder + _campaign.ToString() + "/";

            AssetDatabase.Refresh();
            if(Directory.Exists(_folderPath) == false)
            {
                Directory.CreateDirectory(_folderPath);
            }
            this.ForceMenuTreeRebuild();
            //_tree.Add(_relicName, NodeEditorSelector.Create(_folderPath));
        }
        #endregion

        public enum Campaings
        {
            Empty,
            BrandOfMadness
        }
    }
}


#endif