using System.Collections.Generic; // 新增這一行
using UnityEngine;

public class WalkableChecker : MonoBehaviour
{
    public Walkable targetWalkable;
    public GameObject target3DObject;

    private void Update()
    {
        // 檢查 targetWalkable 是否存在，並判斷 iscardTile
        if (targetWalkable != null && targetWalkable.iscardTile == false)
        {
            // 當 iscardTile 為 false 時關閉 3D 物件
            if (target3DObject != null)
            {
                target3DObject.SetActive(false);
            }
        }
    }
}