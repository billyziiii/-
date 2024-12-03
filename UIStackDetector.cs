using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class UIStackDetector : MonoBehaviour
{

    // 要檢測是否被堆疊的目標 UI 元素
    public RectTransform targetUIElement;

    // 可能堆疊在目標上的其他 UI 元素列表
    public List<RectTransform> otherUIElements;

    // 標記是否被堆疊
    private bool isStacked = false;

    // 可選：在被堆疊時顯示的提示
    public Image overlayImage; // 可以是一個半透明的圖像來表示堆疊狀態

    private void Start()
    {
        overlayImage.enabled = false;
    }
    void Update()
    {
        if (targetUIElement == null)
        {
            Debug.LogWarning("Target UI Element is not assigned.");
            return;
        }

        // 檢查是否有其他 UI 元素與目標重疊
        bool overlapped = false;
        foreach (RectTransform other in otherUIElements)
        {
            if (other == null || other == targetUIElement)
                continue;

            if (IsOverlapping(targetUIElement, other))
            {
                overlapped = true;
                break;
            }
        }

        if (overlapped != isStacked)
        {
            isStacked = overlapped;
            if (isStacked)
            {
                Debug.Log($"{targetUIElement.name} 被堆疊了！");
                if (overlayImage != null)
                {
                    overlayImage.enabled = true;
                }
            }
            else
            {
                Debug.Log($"{targetUIElement.name} 沒有被堆疊。");
                if (overlayImage != null)
                {
                    overlayImage.enabled = false;
                }
            }
        }
    }

    // 檢查兩個 RectTransform 是否重疊
    bool IsOverlapping(RectTransform rect1, RectTransform rect2)
    {
        Rect r1 = GetWorldRect(rect1);
        Rect r2 = GetWorldRect(rect2);
        return r1.Overlaps(r2);
    }

    // 將 RectTransform 轉換為世界坐標下的 Rect
    Rect GetWorldRect(RectTransform rectTransform)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        float minX = corners[0].x;
        float minY = corners[0].y;
        float maxX = corners[2].x;
        float maxY = corners[2].y;

        return new Rect(minX, minY, maxX - minX, maxY - minY);
    }
}
