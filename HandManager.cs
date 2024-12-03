using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandManager : MonoBehaviour
{
   public List<Card> cardData = new List<Card>(); // 卡牌資料

    public GameObject prefab; 
    public Transform parentTransform;  // 父物件放置生成的Prefab
    public float spacing = 50f;  // 卡牌間的間距

    public UIStackDetector uiStackDetector;

    private int prefabCount = 0;  // 記錄已生成的Prefab數量

    private Dictionary<GameObject, Vector3> cardPositions = new Dictionary<GameObject, Vector3>();   

    // 生成卡牌Prefab的函數
    void GeneratePrefab()
    {
        if (prefab != null && parentTransform != null)
        {
            // 從cardData中隨機選擇一張卡牌
            Card randomCard = cardData[Random.Range(0, cardData.Count)];

            // 生成新的Prefab
            GameObject newPrefab = Instantiate(prefab, parentTransform);
            prefabCount++;

            // 設定新生成的卡牌資料
            CardDisplay cardDisplay = newPrefab.GetComponent<CardDisplay>();
            if (cardDisplay != null)
            {
                cardDisplay.cardData = randomCard;
                cardDisplay.UpdateCardDisplay();
            }

            // 設定新卡片的初始位置
            float xOffset = cardPositions.Count * spacing;
            Vector3 cardPosition = new Vector3(xOffset, 0, 0);
            newPrefab.GetComponent<RectTransform>().localPosition = cardPosition;

            // 保存卡片的初始位置
            cardPositions[newPrefab] = cardPosition;

            // 將卡片添加到 UI 堆棧檢測器
            RectTransform cardRect = newPrefab.GetComponent<RectTransform>();
            if (cardRect != null && uiStackDetector != null)
            {
                uiStackDetector.otherUIElements.Add(cardRect);
            }


            // 更新所有卡牌的位置
            UpdateCardPositions();
        }
        else
        {
            Debug.LogWarning("Prefab或父物件尚未設置");
        }
    }

    // 更新所有卡牌的位置，使其橫向排列
    void UpdateCardPositions()
    {
        for (int i = 0; i < parentTransform.childCount; i++)
        {
            Transform card = parentTransform.GetChild(i);
            float xOffset = i * spacing;

            // 設置卡牌的位置
            card.localPosition = new Vector3(xOffset, 0, 0);

            // 設置卡牌的旋轉，使其保持正面朝上
            card.localRotation = Quaternion.identity;
        }
    }
    public void Drawattackcard()
    {
        if (prefab != null && parentTransform != null && cardData.Count > 0)
        {
            // 指定生成 cardData[0] 的卡牌
            Card specifiedCard = cardData[0];

            // 生成新的Prefab
            GameObject newPrefab = Instantiate(prefab, parentTransform);
            prefabCount++;

            // 設定新生成的卡牌資料
            CardDisplay cardDisplay = newPrefab.GetComponent<CardDisplay>();
            if (cardDisplay != null)
            {
                cardDisplay.cardData = specifiedCard;
                cardDisplay.UpdateCardDisplay();
            }

            // 設定新卡片的初始位置
            float xOffset = cardPositions.Count * spacing;
            Vector3 cardPosition = new Vector3(xOffset, 0, 0);
            newPrefab.GetComponent<RectTransform>().localPosition = cardPosition;

            // 保存卡片的初始位置
            cardPositions[newPrefab] = cardPosition;

            // 將卡片添加到 UI 堆棧檢測器
            RectTransform cardRect = newPrefab.GetComponent<RectTransform>();
            if (cardRect != null && uiStackDetector != null)
            {
                uiStackDetector.otherUIElements.Add(cardRect);
            }
        }

        UpdateCardPositions();
    }
    public Vector3 GetOriginalPosition(GameObject card)//獲取生成卡片現在位置
    {
        return cardPositions.ContainsKey(card) ? cardPositions[card] : Vector3.zero;
    }
}
