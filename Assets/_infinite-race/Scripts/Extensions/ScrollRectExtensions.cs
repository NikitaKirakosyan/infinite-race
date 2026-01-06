using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Southbyte
{
	public static class ScrollRectExtensions
	{
		public static void FocusAtPoint(this ScrollRect scrollView, Vector2 focusPoint)
		{
			scrollView.normalizedPosition = scrollView.CalculateFocusedScrollPosition(focusPoint);
		}

		public static void SmoothFocusAtPoint(this ScrollRect scrollView, Vector2 focusPoint, float duration)
		{
			scrollView.DoNormalizedPos(scrollView.CalculateFocusedScrollPosition(focusPoint), duration);
		}

		public static void FocusOnItem(this ScrollRect scrollView, RectTransform item)
		{
			scrollView.normalizedPosition = scrollView.CalculateFocusedScrollPosition(item);
		}

		public static void SmoothFocusOnItem(this ScrollRect scrollView, RectTransform item, float duration)
		{
			scrollView.DoNormalizedPos(scrollView.CalculateFocusedScrollPosition(item), duration);
		}


		private static Vector2 CalculateFocusedScrollPosition(this ScrollRect scrollView, Vector2 focusPoint)
		{
			var contentSize = scrollView.content.rect.size;
			var contentScale = scrollView.content.localScale;
			var viewportSize = ((RectTransform)scrollView.content.parent).rect.size;

			contentSize.Scale(contentScale);
			focusPoint.Scale(contentScale);

			var scrollPosition = scrollView.normalizedPosition;

			if(scrollView.horizontal && contentSize.x > viewportSize.x)
			{
				scrollPosition.x = Mathf.Clamp01((focusPoint.x - viewportSize.x * 0.5f) / (contentSize.x - viewportSize.x));
			}

			if(scrollView.vertical && contentSize.y > viewportSize.y)
			{
				scrollPosition.y = Mathf.Clamp01((focusPoint.y - viewportSize.y * 0.5f) / (contentSize.y - viewportSize.y));
			}

			return scrollPosition;
		}

		private static Vector2 CalculateFocusedScrollPosition(this ScrollRect scrollView, RectTransform item)
		{
			Vector2 itemCenterPoint = scrollView.content.InverseTransformPoint(item.transform.TransformPoint(item.rect.center));

			var contentSizeOffset = scrollView.content.rect.size;
			contentSizeOffset.Scale(scrollView.content.pivot);

			return scrollView.CalculateFocusedScrollPosition(itemCenterPoint + contentSizeOffset);
		}

		private static void DoNormalizedPos(this ScrollRect scrollView, Vector2 targetNormalizedPos, float duration)
		{
			scrollView.DOKill();
			scrollView.DONormalizedPos(targetNormalizedPos, duration);
		}
	}
}