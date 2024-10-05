using UnityEngine;

public static class RectTransformExtensions
{
    public static bool IsOverlapping(RectTransform rectTransformA, RectTransform rectTransformB)
    {
        // Получаем Rect для обоих RectTransform
        Rect rectA = GetWorldRect(rectTransformA);
        Rect rectB = GetWorldRect(rectTransformB);

        // Проверяем, пересекаются ли прямоугольники
        return rectA.Overlaps(rectB);
    }

    private static Rect GetWorldRect(RectTransform rectTransform)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        // Определяем минимальные и максимальные координаты
        Vector2 min = corners[0];
        Vector2 max = corners[2];

        return new Rect(min.x, min.y, max.x - min.x, max.y - min.y);
    }
}
