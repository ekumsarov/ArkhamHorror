using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace EVI
{
    public static class AssetSelector
    {
        public static IEnumerable<ValueDropdownItem<T>> GetDropdownItems<T>() where T : BaseModel
        {
            List<ValueDropdownItem<T>> result = new();

            AddFromPath("Data/Common", "Common", result);
            AddFromPath($"Data/Campaigns/{EditorSettings.CurrentCampaign}", EditorSettings.CurrentCampaign, result);

            return result;
        }

        public static IEnumerable<ValueDropdownItem<string>> GetDropdownIDs<T>() where T : BaseModel
        {
            List<ValueDropdownItem<string>> result = new();

            AddIDFromPath<T>("Data/Common", "Common", result);
            AddIDFromPath<T>($"Data/Campaigns/{EditorSettings.CurrentCampaign}", EditorSettings.CurrentCampaign, result);

            return result;
        }

        private static void AddFromPath<T>(string path, string labelPrefix, List<ValueDropdownItem<T>> list) where T : BaseModel
        {
            var assets = Resources.LoadAll<T>(path);
            foreach (var asset in assets)
            {
                string label = $"{labelPrefix}/{asset.ID}";
                list.Add(new ValueDropdownItem<T>(label, asset));
            }
        }

        private static void AddIDFromPath<T>(string path, string labelPrefix, List<ValueDropdownItem<string>> list) where T : BaseModel
        {
            var assets = Resources.LoadAll<T>(path);
            foreach (var asset in assets)
            {
                string label = $"{labelPrefix}/{asset.ID}";
                list.Add(new ValueDropdownItem<string>(label, asset.ID));
            }
        }
    }
}
