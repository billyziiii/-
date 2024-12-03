using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardDesignatedArea : MonoBehaviour
{  
    // 用于标识删除区域的 RectTransform
    public static RectTransform deleteAreaRect;

    // UI堆栈管理
    private readonly List<RectTransform> uiStack = new List<RectTransform>();
    public int maxStackSize = 10;

   
    private void Awake()
    {
        
        // 获取删除区域的 RectTransform
        deleteAreaRect = GetComponent<RectTransform>();
       
    }   
    // 更新UI堆栈：检测当前在删除区域上的 UI 元素
    private void UpdateUIStack()
    {
        uiStack.Clear();
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas == null) return;

        GraphicRaycaster raycaster = canvas.GetComponent<GraphicRaycaster>();
        if (raycaster == null) return;

        PointerEventData pointerData = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerData, results);

        foreach (RaycastResult result in results)
        {          
            RectTransform rt = result.gameObject.GetComponent<RectTransform>();
            if (rt != null && rt != deleteAreaRect && !uiStack.Contains(rt))
            {
                uiStack.Add(rt);
                if (uiStack.Count >= maxStackSize) break;
            }
        }
    }

    // 手动向堆栈添加元素
    public void AddToStack(RectTransform uiElement)
    {
        if (uiStack.Contains(uiElement)) return;
        uiStack.Add(uiElement);
        if (uiStack.Count > maxStackSize)
        {
            uiStack.RemoveAt(0); // 超出堆栈容量限制时移除最早的元素
        }
    }

    // 手动从堆栈移除元素
    public void RemoveFromStack(RectTransform uiElement)
    {
        uiStack.Remove(uiElement);
    }
}
