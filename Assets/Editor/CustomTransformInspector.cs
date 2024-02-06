using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
/*
[CustomEditor(typeof(Transform))]
[CanEditMultipleObjects]
public class CustomTransformInspector : Editor
{
    public override VisualElement CreateInspectorGUI()
    {
        VisualElement root = new VisualElement();

        /*positionField = new Vector3Field("Position");
        root.Add(positionField);
        positionField.RegisterValueChangedCallback(eventData =>
        {
            foreach (GameObject selected in Selection.gameObjects)
            {
                Undo.RecordObject(selected.transform, "ChangePosition");
                selected.transform.localPosition = eventData.newValue;
            }
        });

        rotationField = new Vector3Field("Rotation");
        root.Add(rotationField);
        rotationField.RegisterValueChangedCallback(eventData =>
        {
            foreach (GameObject selected in Selection.gameObjects)
            {
                Undo.RecordObject(selected.transform, "ChangeRotation");
                TransformUtils.SetInspectorRotation(selected.transform, eventData.newValue);
            }
        });

        scaleField = new Vector3Field("Scale");
        root.Add(scaleField);
        scaleField.RegisterValueChangedCallback(eventData =>
        {
            foreach (GameObject selected in Selection.gameObjects)
            {
                Undo.RecordObject(selected.transform, "ChangeScale");
                selected.transform.localScale = eventData.newValue;
            }
        });

        OnSceneGUI();

        Label label = new Label("It's working");
        root.Add(label);

        return root;
    }
}
*/