using UnityEngine;
using UnityEditor;
using EVI;
using UnityEditor.Presets;

public class EditorMenus : Editor
{
    [MenuItem("GameObject/EVI Extension/Create EVI Empty", priority = -100)]
    private static void CreateEmptyObject()
    {
        GameObject obj = new GameObject();
        TransformExtension transorm = obj.AddComponent<TransformExtension>();
        obj.name = "NewEmpty";

        Preset transformPreset = AssetDatabase.LoadAssetAtPath<Preset>("Assets/Editor/Resources/Presets/TransformExtension.preset");
        transformPreset.ApplyTo(transorm);

        if (Selection.activeTransform != null)
        {
            obj.transform.SetParent(Selection.activeTransform);
            obj.transform.position = Selection.activeTransform.position;
        }
        Selection.activeTransform = obj.transform;
    }

    [MenuItem("GameObject/EVI Extension/Create Sprite", priority = -99)]
    private static void CreateSpriteObject()
    {
        GameObject obj = new GameObject();
        obj.name = "NewSprite";

        TransformExtension transorm = obj.AddComponent<TransformExtension>();
        Preset transformPreset = AssetDatabase.LoadAssetAtPath<Preset>("Assets/Editor/Resources/Presets/TransformExtension.preset");
        transformPreset.ApplyTo(transorm);

        SpriteRenderer sprite = obj.AddComponent<SpriteRenderer>();
        Sprite tex = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Editor/Resources/64 flat icons/png/128px/UI_Gamepad.png", typeof(Sprite));
        sprite.sprite = tex;

        if (Selection.activeTransform != null)
        {
            obj.transform.SetParent(Selection.activeTransform);
            obj.transform.position = Selection.activeTransform.position;
        }
        Selection.activeTransform = obj.transform;
    }

    [MenuItem("GameObject/EVI Extension/Create BaseView", priority = -98)]
    private static void CreateBaseViewObject()
    {
        GameObject obj = new GameObject();
        obj.name = "NewBaseView";

        TransformExtension transorm = obj.AddComponent<TransformExtension>();
        Preset transformPreset = AssetDatabase.LoadAssetAtPath<Preset>("Assets/Editor/Resources/Presets/TransformExtension.preset");
        transformPreset.ApplyTo(transorm);

        SpriteRenderer sprite = obj.AddComponent<SpriteRenderer>();
        Sprite tex = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Editor/Resources/64 flat icons/png/128px/UI_Gamepad.png", typeof(Sprite));
        sprite.sprite = tex;

        if (Selection.activeTransform != null)
        {
            obj.transform.SetParent(Selection.activeTransform);
            obj.transform.position = Selection.activeTransform.position;
        }
        Selection.activeTransform = obj.transform;

        obj.AddComponent<BaseView>();
    }
}