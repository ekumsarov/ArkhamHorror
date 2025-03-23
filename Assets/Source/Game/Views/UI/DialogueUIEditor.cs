#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class DialogueUIEditor : MonoBehaviour
{
    [Button]
    public void GenerateDialogueUI()
    {
        // Найдём Canvas в сцене
        Canvas canvas = GetComponent<Canvas>();
        if (canvas == null)
        {
            GameObject canvasGO = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvas = canvasGO.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGO.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        }

        // Создаём главное окно диалога
        GameObject dialoguePanel = new GameObject("DialoguePanel", typeof(RectTransform), typeof(Image));
        RectTransform panelRect = dialoguePanel.GetComponent<RectTransform>();
        panelRect.SetParent(canvas.transform, false);
        panelRect.sizeDelta = new Vector2(800, 400);
        panelRect.anchoredPosition = Vector2.zero;
        dialoguePanel.GetComponent<Image>().color = new Color(0, 0, 0, 0.8f); // Тёмный фон

        // Создаём текст диалога
        GameObject textGO = new GameObject("DialogueText", typeof(RectTransform), typeof(Text));
        RectTransform textRect = textGO.GetComponent<RectTransform>();
        textRect.SetParent(dialoguePanel.transform, false);
        textRect.sizeDelta = new Vector2(750, 200);
        textRect.anchoredPosition = new Vector2(0, 100);

        Text dialogueText = textGO.GetComponent<Text>();
        dialogueText.text = "Вы встретили таинственного незнакомца. Что делать?";
        dialogueText.alignment = TextAnchor.MiddleCenter;
        dialogueText.fontSize = 24;
        dialogueText.color = Color.white;

        // Создаём кнопки-выборы
        string[] choices = { "Спросить", "Проигнорировать", "Атаковать" };
        for (int i = 0; i < choices.Length; i++)
        {
            CreateChoiceButton(choices[i], i, dialoguePanel.transform);
        }

        // После генерации удаляем скрипт
        DestroyImmediate(this);
    }

    private void CreateChoiceButton(string choiceText, int index, Transform parent)
    {
        GameObject buttonGO = new GameObject($"Choice_{index}", typeof(RectTransform), typeof(Button), typeof(Image), typeof(Text));
        RectTransform buttonRect = buttonGO.GetComponent<RectTransform>();
        buttonRect.SetParent(parent, false);
        buttonRect.sizeDelta = new Vector2(700, 50);
        buttonRect.anchoredPosition = new Vector2(0, -50 * index - 50);

        Button button = buttonGO.GetComponent<Button>();
        Image buttonImage = buttonGO.GetComponent<Image>();
        buttonImage.color = new Color(1, 1, 1, 0.5f); // Полупрозрачная кнопка
        button.targetGraphic = buttonImage;

        Text buttonText = buttonGO.GetComponent<Text>();
        buttonText.text = choiceText;
        buttonText.alignment = TextAnchor.MiddleCenter;
        buttonText.fontSize = 20;
        buttonText.color = Color.black;
    }
}
#endif
