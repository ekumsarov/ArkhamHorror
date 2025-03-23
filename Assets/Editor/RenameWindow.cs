using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.IO;
using System.Linq;

public class RenameWindow : EditorWindow
{
    private CampaignEditorWindow editor;
    private OdinMenuItem menuItem;
    private string newName = "";

    public static void ShowWindow(CampaignEditorWindow editor, OdinMenuItem item)
    {
        var window = ScriptableObject.CreateInstance<RenameWindow>();
        window.editor = editor;
        window.menuItem = item;
        window.newName = item.Name; // по умолчанию текущее имя
        window.titleContent = new GUIContent("Переименовать");
        window.position = new Rect(Screen.width / 2f, Screen.height / 2f, 300, 100);
        window.ShowUtility(); // ShowUtility() → небольшое всплывающее окно
    }

    private void OnGUI()
    {
        GUILayout.Label("Введите новое имя:", EditorStyles.boldLabel);
        newName = EditorGUILayout.TextField(newName);

        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Отмена"))
        {
            Close();
        }
        if (GUILayout.Button("Переименовать"))
        {
            DoRename();
            Close();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void DoRename()
    {
        if (string.IsNullOrEmpty(newName))
        {
            Debug.LogWarning("Новое имя не может быть пустым!");
            return;
        }

        // Определяем, папка или ассет?
        if (menuItem.Value is CampaignEditorWindow.FolderItem folder)
        {
            RenameFolder(folder.FullPath, newName);
        }
        else if (menuItem.Value is UnityEngine.Object obj)
        {
            RenameAsset(obj, newName);
        }
        else
        {
            Debug.LogWarning("Невозможно переименовать данный элемент!");
        }
    }

    /// <summary>
    /// Переименовать папку с проверкой и удалением пустой старой.
    /// </summary>
    private void RenameFolder(string oldPath, string newFolderName)
    {
        string parentDir = Path.GetDirectoryName(oldPath);
        string newFullPath = Path.Combine(parentDir, newFolderName);

        if (Directory.Exists(newFullPath))
        {
            Debug.LogWarning($"Папка {newFolderName} уже существует в {parentDir}!");
            return;
        }

        try
        {
            // Перемещаем / переименовываем на уровне ОС
            Directory.Move(oldPath, newFullPath);

            // На некоторых системах может оставаться старая пустая папка или .meta
            // Проверим и удалим, если пустая
            CleanupOldFolder(oldPath);

            AssetDatabase.Refresh();
            editor.ForceMenuTreeRebuild();
            Debug.Log($"Папка '{oldPath}' переименована на '{newFullPath}'");
        }
        catch (Exception e)
        {
            Debug.LogError($"Ошибка при переименовании папки: {e.Message}");
        }
    }

    /// <summary>
    /// Если после Directory.Move старая папка осталась и опустела (не считая .meta), удаляем её.
    /// </summary>
    private void CleanupOldFolder(string oldPath)
    {
        if (Directory.Exists(oldPath))
        {
            if (IsDirectoryEmptyExcludingMeta(oldPath))
            {
                try
                {
                    Directory.Delete(oldPath, true);
                    Debug.Log($"Удалена старая пустая папка '{oldPath}'.");
                }
                catch (Exception e)
                {
                    Debug.LogError($"Не удалось удалить пустую папку '{oldPath}': {e.Message}");
                }
            }
        }

        // Удаляем meta-файл, если остался
        string oldMetaFile = oldPath.TrimEnd(Path.DirectorySeparatorChar) + ".meta";
        if (File.Exists(oldMetaFile))
        {
            try
            {
                File.Delete(oldMetaFile);
                Debug.Log($"Удалён meta-файл: {oldMetaFile}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Не удалось удалить мета-файл '{oldMetaFile}': {e.Message}");
            }
        }
    }

    /// <summary>
    /// Проверяет, пуста ли директория, игнорируя файлы .meta.
    /// </summary>
    private bool IsDirectoryEmptyExcludingMeta(string path)
    {
        // Получаем все файлы, кроме .meta
        var files = Directory.GetFiles(path)
            .Where(f => !f.EndsWith(".meta", StringComparison.OrdinalIgnoreCase))
            .ToArray();
        if (files.Length > 0)
            return false;

        // Проверяем подпапки
        var dirs = Directory.GetDirectories(path);
        if (dirs.Length > 0)
            return false;

        return true;
    }

    /// <summary>
    /// Переименовать Unity ассет (main или sub-asset).
    /// </summary>
    private void RenameAsset(UnityEngine.Object obj, string newAssetName)
    {
        string oldPath = AssetDatabase.GetAssetPath(obj);
        if (string.IsNullOrEmpty(oldPath))
        {
            Debug.LogWarning("Не удалось получить путь ассета!");
            return;
        }

        // Unity требует имя без расширения
        string error = AssetDatabase.RenameAsset(oldPath, newAssetName);
        if (!string.IsNullOrEmpty(error))
        {
            Debug.LogError($"Не удалось переименовать ассет: {error}");
        }
        else
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            editor.ForceMenuTreeRebuild();
            Debug.Log($"Ассет переименован на '{newAssetName}'");
        }
    }
}
