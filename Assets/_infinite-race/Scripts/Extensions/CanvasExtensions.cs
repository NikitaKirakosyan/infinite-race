using UnityEngine;

namespace Southbyte
{
    public static class CanvasExtensions
    {
        public static Vector2 GetInputPosition(this Canvas canvas)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out var currentInputPosition);
            return currentInputPosition;
        }
    }
}