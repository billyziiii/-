using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardDrag : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    //private HandManager cardManager;
    private bool isReturning = false;
    private Vector3 originalPosition;//原位
   
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

       // cardManager = FindObjectOfType<HandManager>();
        originalPosition = rectTransform.localPosition;
        
    }
    private void Update()
    {
        // 檢測空白處的點擊，恢復卡牌大小
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            rectTransform.localScale = Vector3.one;  // 恢復原來大小
        }       
    }
    public void OnPointerDown(PointerEventData eventData) //按下滑鼠按鍵或觸摸屏幕
    {
        rectTransform.localScale = new Vector3(1.1f, 1.1f, 1.1f); // 將卡牌放大 10%
    }

    public void OnBeginDrag(PointerEventData eventData)//開始拖曳時
    {
        isReturning = false;
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)//拖曳時
    {
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)//結束拖曳時
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        HandManager handManager = FindObjectOfType<HandManager>();//場景搜尋HandManager
        CardDesignatedArea cardDesignatedArea = FindObjectOfType<CardDesignatedArea>();

        // 检查卡牌是否在删除区域中
        if (CardDesignatedArea.deleteAreaRect != null && RectTransformUtility.RectangleContainsScreenPoint(CardDesignatedArea.deleteAreaRect, Input.mousePosition))
        {
            Destroy(gameObject); // 如果卡牌在删除区域内，删除卡牌
        }
        else
        {
            // 如果不在删除区域，则返回原来的位置
            if (handManager != null)
            {
                Vector3 targetPosition = handManager != null ? handManager.GetOriginalPosition(gameObject) : originalPosition;
                StartCoroutine(SmoothMoveBack(targetPosition)); // 返回原位置动画
            }
            else
            {
                StartCoroutine(SmoothMoveBack(originalPosition)); // 如果 HandManager 沒有設置，使用原始位置
            }
           
        }
    }
    
    private System.Collections.IEnumerator SmoothMoveBack(Vector3 targetPosition)//Dotween動畫輔助
    {
        isReturning = true;
        float duration = 0.2f;
        float elapsed = 0f;

        Vector3 startPosition = rectTransform.localPosition;

        while (elapsed < duration)
        {
            if (!isReturning) yield break;

            rectTransform.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        rectTransform.localPosition = targetPosition;
        isReturning = false;
    }
}
