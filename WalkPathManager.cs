using System.Collections.Generic;
using UnityEngine;

public class WalkPathManager : MonoBehaviour//卡片戰鬥路線開啟
{
    // 公開的變數，用於存儲目標物件
    public GameObject targetObject;

    // 公開方法，用於將目標物件的 WalkPath 的 active 屬性設為 false
    public void DeactivateWalkPaths()
    {
        if (targetObject == null)
        {
            Debug.LogWarning("Target object is not assigned.");
            return;
        }

        // 獲取目標物件上的 Walkable 組件
        Walkable walkable = targetObject.GetComponent<Walkable>();

        if (walkable == null)
        {
            Debug.LogWarning("Target object does not have a Walkable component.");
            return;
        }

        // 遍歷 Walkable 中的 possiblePaths 並設置 active = false
        foreach (WalkPath path in walkable.possiblePaths)
        {
            path.active = true;
            Debug.Log($"Deactivated path to target: {path.target?.name ?? "null"}");
        }
    }
}
