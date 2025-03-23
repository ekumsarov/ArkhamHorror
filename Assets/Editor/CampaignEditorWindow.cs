#if UNITY_EDITOR
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using EVI;
using EVI.Game;
using System;

public class CampaignEditorWindow : OdinMenuEditorWindow
{
    private const string CampaignsPath = "Assets/Resources/Data/Campaigns/";

    internal List<string> availableCampaigns = new();
    internal string selectedCampaign;
    private MainPanel mainPanel;
    private CampaignData _campaignData;

    [MenuItem("Tools/Simplified Campaign Editor")]
    private static void OpenWindow()
    {
        var window = GetWindow<CampaignEditorWindow>();
        window.position = GUIHelper.GetEditorWindowRect().AlignCenter(1200, 700);
        window.titleContent = new GUIContent("Campaign Editor");
        window.Show();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        RefreshCampaignList();
        mainPanel = new MainPanel(this);
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree(true)
        {
            DefaultMenuStyle = { IconSize = 20f, IndentAmount = 15f },
            Config = { DrawSearchToolbar = true }
        };

        // "Main"
        tree.Add("Main", mainPanel);

        if (!string.IsNullOrEmpty(selectedCampaign))
        {
            string campaignRoot = Path.Combine(CampaignsPath, selectedCampaign);
            AddDirectoryToTree(tree, campaignRoot, "");

            // Добавление CampaignData (создаём если не существует)
            string dataPath = Path.Combine(campaignRoot, "CampaignData.asset");
            string relPath = dataPath.Replace(Application.dataPath, "Assets").Replace("\\", "/");

            _campaignData = AssetDatabase.LoadAssetAtPath<CampaignData>(relPath);

            if (_campaignData == null)
            {
                _campaignData = ScriptableObject.CreateInstance<CampaignData>();
                AssetDatabase.CreateAsset(_campaignData, relPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                Debug.Log($"Создан новый CampaignData в {relPath}");
            }

            tree.Add("Campaign Settings", _campaignData);

        }

        // Включаем Drag & Drop для каждого пункта
        foreach (var item in tree.EnumerateTree())
        {
            AddDragHandles(item);
            item.OnRightClick += ShowContextMenu;
        }

        return tree;
    }

    private void ShowContextMenu(OdinMenuItem menuItem)
    {
        GenericMenu gm = new GenericMenu();

        // Добавляем пункт «Переименовать»
        gm.AddItem(new GUIContent("Переименовать"), false, () => PromptRename(menuItem));

        // Можно добавить другие опции
        // gm.AddItem(new GUIContent("Удалить"), false, () => DeleteItem(menuItem));

        gm.ShowAsContext();
    }

    private void PromptRename(OdinMenuItem menuItem)
    {
        RenameWindow.ShowWindow(this, menuItem);
    }

    private void AddDirectoryToTree(OdinMenuTree tree, string folderPath, string menuPath)
    {
        if (!Directory.Exists(folderPath)) return;

        string folderName = Path.GetFileName(folderPath);
        string newMenuPath = string.IsNullOrEmpty(menuPath)
            ? folderName
            : $"{menuPath}/{folderName}";

        // Папка
        tree.Add(newMenuPath, new FolderItem(this, folderPath));

        // Все .asset в папке
        var assetFiles = Directory.GetFiles(folderPath, "*.asset", SearchOption.TopDirectoryOnly);
        foreach (var file in assetFiles)
        {
            var assetPath = ToAssetPath(file);
            var allObjs = AssetDatabase.LoadAllAssetsAtPath(assetPath);
            if (allObjs == null || allObjs.Length == 0)
                continue;

            // Ищем главный объект
            var mainObj = allObjs.FirstOrDefault(o => AssetDatabase.IsMainAsset(o));
            if (mainObj == null)
                continue;

            string mainObjPath = $"{newMenuPath}/{mainObj.name}";
            tree.Add(mainObjPath, mainObj);

            // Саб-объекты
            var subObjs = allObjs.Where(o => AssetDatabase.IsSubAsset(o));
            foreach (var subObj in subObjs)
            {
                string subPath = $"{mainObjPath}/{subObj.name}";
                tree.Add(subPath, subObj);
            }
        }

        // Рекурсивно подпапки
        foreach (var subDir in Directory.GetDirectories(folderPath))
        {
            AddDirectoryToTree(tree, subDir, newMenuPath);
        }
    }

    private void AddDragHandles(OdinMenuItem menuItem)
    {
        menuItem.OnDrawItem += (item) =>
        {
            // Получаем прямоугольную область пункта
            Rect rect = item.Rect;

            // 1) DragZone — чтобы сам пункт можно было «схватить» и перетащить
            DragAndDropUtilities.DragZone(rect, menuItem.Value, false, false);
        };
    }

    private void HandleDropOnMenuItem(OdinMenuItem menuItem, UnityEngine.Object[] draggedObjects)
    {
        var target = menuItem.Value;

        // Сценарий 1: Если целевой пункт — это папка (FolderItem)
        if (target is FolderItem folder)
        {
            string folderPath = folder.FullPath;
            MoveAssetsToFolder(folderPath, draggedObjects);
        }
        // Сценарий 2: Если целевой пункт — это ListLogicNode
        else if (target is ListLogicNode listNode)
        {
            // Добавим любые перетаскиваемые LogicNode в _nodes,
            // предварительно убрав их из старых мест (если нужно).
            AddNodesToListLogicNode(listNode, draggedObjects);
        }
        // Сценарий 3: Если хотим обработать дроп на обычный LogicNode
        else if (target is LogicNode logicNode)
        {
            // Например, сказать, что draggedObject теперь nextNode = logicNode
            // Или наоборот. Это уже зависит от вашей логики.
            Debug.Log($"[Drop] You dropped something on LogicNode: {logicNode.name}");
        }
        else
        {
            Debug.Log($"[Drop] Dropped on: {target?.GetType().Name ?? "null"}. " +
                      $"No specific logic handled.");
        }

        ForceMenuTreeRebuild();
    }

    private void MoveAssetsToFolder(string folderPath, UnityEngine.Object[] draggedObjects)
    {
        foreach (var obj in draggedObjects)
        {
            // Получаем путь .asset
            string oldPath = AssetDatabase.GetAssetPath(obj);
            if (string.IsNullOrEmpty(oldPath))
                continue;

            // Формируем новый путь
            string fileName = Path.GetFileName(oldPath);
            string newPath = Path.Combine(folderPath, fileName).Replace("\\", "/");

            // Пытаемся переместить
            var result = AssetDatabase.MoveAsset(oldPath, newPath);
            if (!string.IsNullOrEmpty(result))
            {
                Debug.LogWarning($"Не удалось переместить '{fileName}' в '{folderPath}'. " +
                                 $"Причина: {result}");
            }
        }
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// Добавляем LogicNode-объекты в список _nodes внутри ListLogicNode,
    /// делая их саб-ассетами (если хотим).
    /// </summary>
    private void AddNodesToListLogicNode(ListLogicNode listNode, UnityEngine.Object[] draggedObjects)
    {
        bool changed = false;

        foreach (var obj in draggedObjects)
        {
            if (obj is LogicNode logicNode)
            {
                // Сначала убираем его из старого файла, если нужно
                RemoveObjectFromAnyAsset(logicNode);

                // Проверяем, нет ли его уже внутри этого ListLogicNode
                if (!listNode.ContainsNode(logicNode))
                {
                    // Добавляем как саб-нод (инкапсулировано в ListLogicNode)
                    listNode.AddSubNode(logicNode);

                    changed = true;
                    Debug.Log($"[Drop] Добавили {logicNode.name} в {listNode.name}.");
                }
            }
        }

        if (changed)
        {
            AssetDatabase.SaveAssets();
        }
    }

    private void RemoveObjectFromAnyAsset(UnityEngine.Object obj)
    {
        string oldPath = AssetDatabase.GetAssetPath(obj);
        if (!string.IsNullOrEmpty(oldPath))
        {
            // Если объект - sub-asset, то убираем его.
            // Если это main asset, RemoveObjectFromAsset не сработает (и не нужно).
            if (AssetDatabase.IsSubAsset(obj))
            {
                AssetDatabase.RemoveObjectFromAsset(obj);
            }
        }
    }

    internal void RefreshCampaignList()
    {
        if (!Directory.Exists(CampaignsPath))
            Directory.CreateDirectory(CampaignsPath);

        availableCampaigns = Directory.GetDirectories(CampaignsPath)
            .Select(Path.GetFileName)
            .ToList();
    }

    private string ToAssetPath(string absolutePath)
    {
        return absolutePath.Replace(Application.dataPath, "Assets").Replace("\\", "/");
    }

    public class MainPanel
    {
        private readonly CampaignEditorWindow editor;
        public MainPanel(CampaignEditorWindow editorWindow)
        {
            this.editor = editorWindow;
        }

        [Title("Управление кампаниями")]
        [ValueDropdown("GetCampaignList")]
        [ShowInInspector]
        public string SelectedCampaign
        {
            get => editor.selectedCampaign;
            set
            {
                editor.selectedCampaign = value;
                EditorSettings.CurrentCampaign = value;
                editor.ForceMenuTreeRebuild();
            }
        }

        [Button("Обновить список")]
        private void RefreshList()
        {
            editor.RefreshCampaignList();
        }

        [Button("Создать новую кампанию")]
        private void CreateNewCampaign(string CampaignName)
        {
            string newCampaignName = CampaignName;
            string newCampaignPath = Path.Combine(CampaignsPath, newCampaignName);

            if (!Directory.Exists(newCampaignPath))
            {
                Directory.CreateDirectory(newCampaignPath);
                foreach (var folder in new[] { "Nodes", "Locations", "Cards", "ResourceList" })
                {
                    Directory.CreateDirectory(Path.Combine(newCampaignPath, folder));
                }
                editor.RefreshCampaignList();
            }
            else
            {
                Debug.LogWarning($"Кампания '{newCampaignName}' уже существует!");
            }
        }

        private IEnumerable<string> GetCampaignList()
        {
            return editor.availableCampaigns;
        }
    }

    public class FolderItem
    {
        private readonly CampaignEditorWindow editor;
        private readonly string folderPath;
        public FolderItem(CampaignEditorWindow editor, string folderPath)
        {
            this.editor = editor;
            this.folderPath = folderPath;
        }

        [ShowInInspector, ReadOnly]
        public string FullPath => folderPath;

        [Button("Создать подпапку")]
        private void CreateSubfolder(string name)
        {
            if (string.IsNullOrEmpty(name)) return;
            string newPath = Path.Combine(folderPath, name);
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
                AssetDatabase.Refresh();
                editor.ForceMenuTreeRebuild();
            }
            else
            {
                Debug.LogWarning($"Подпапка '{name}' уже существует в {folderPath}");
            }
        }

        [Button("Удалить папку")]
        private void DeleteFolder()
        {
            if (!Directory.Exists(folderPath)) return;
            if (EditorUtility.DisplayDialog("Удалить папку?",
                $"Вы уверены, что хотите удалить '{folderPath}' вместе со всем содержимым?",
                "Да", "Отмена"))
            {
                Directory.Delete(folderPath, true);
                AssetDatabase.Refresh();
                editor.ForceMenuTreeRebuild();
            }
        }

        [Title("Создание ассетов")]
        [ShowInInspector, ValueDropdown("GetNodeTypes"), FoldoutGroup("Nodes")]
        private Type selectedNodeType;

        // Поле для ввода названия
        [ShowInInspector, FoldoutGroup("Nodes")]
        private string nodeName = "NewNode";

        [Button("Создать выбранный LogicNode"), FoldoutGroup("Nodes")]
        private void CreateSelectedLogicNode()
        {
            if (selectedNodeType == null)
            {
                Debug.LogWarning("Не выбран тип ноды!");
                return;
            }

            string filePath = System.IO.Path.Combine(folderPath, $"{nodeName}.asset");
            if (System.IO.File.Exists(filePath))
            {
                Debug.LogWarning($"Файл {nodeName}.asset уже существует в {folderPath}");
                return;
            }

            // Создаём инстанс выбранного класса
            var node = ScriptableObject.CreateInstance(selectedNodeType) as LogicNode;
            node.name = nodeName;

            // Сохраняем в папку
            string relPath = filePath.Replace(Application.dataPath, "Assets").Replace("\\", "/");
            AssetDatabase.CreateAsset(node, relPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            editor.ForceMenuTreeRebuild();
            Debug.Log($"Создан {selectedNodeType.Name} по пути {relPath}");
        }

        private static IEnumerable<ValueDropdownItem<Type>> GetNodeTypes()
        {
            var items = LogicNodeReflection.GetAllLogicNodeTypes();
            // Odin понимает ValueDropdownItem<Type>, поэтому можно вернуть напрямую:
            return items;
        }

        [Button("Создать ListLogicNode (корневой)")]
        private void CreateListLogicNodeAsset(string nodeName = "NewListLogicNode")
        {
            CreateScriptableObject<ListLogicNode>(nodeName);
        }

        [Button("Создать Location")]
        private void CreateLocationAsset(string locName = "NewLocation")
        {
            CreateScriptableObject<Location>(locName);
        }

        [Button("Создать GameCard")]
        private void CreateGameCardAsset(string cardName = "NewCard")
        {
            CreateScriptableObject<GameCard>(cardName);
        }

        [Button("Создать Resource")]
        private void CreateResourceAsset(string resourceName = "NewResource")
        {
            CreateScriptableObject<Resource>(resourceName);
        }

        private void CreateScriptableObject<T>(string assetName) where T : ScriptableObject
        {
            if (string.IsNullOrEmpty(assetName)) return;
            string filePath = Path.Combine(folderPath, assetName + ".asset");
            if (File.Exists(filePath))
            {
                Debug.LogWarning($"Файл {assetName}.asset уже существует в {folderPath}");
                return;
            }

            T obj = ScriptableObject.CreateInstance<T>();
            obj.name = assetName;

            string relPath = filePath.Replace(Application.dataPath, "Assets").Replace("\\", "/");
            AssetDatabase.CreateAsset(obj, relPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            editor.ForceMenuTreeRebuild();
        }
    }
}
#endif
