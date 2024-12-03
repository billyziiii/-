using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandManager : MonoBehaviour
{
   public List<Card> cardData = new List<Card>(); // �d�P���

    public GameObject prefab; 
    public Transform parentTransform;  // �������m�ͦ���Prefab
    public float spacing = 50f;  // �d�P�������Z

    public UIStackDetector uiStackDetector;

    private int prefabCount = 0;  // �O���w�ͦ���Prefab�ƶq

    private Dictionary<GameObject, Vector3> cardPositions = new Dictionary<GameObject, Vector3>();   

    // �ͦ��d�PPrefab�����
    void GeneratePrefab()
    {
        if (prefab != null && parentTransform != null)
        {
            // �qcardData���H����ܤ@�i�d�P
            Card randomCard = cardData[Random.Range(0, cardData.Count)];

            // �ͦ��s��Prefab
            GameObject newPrefab = Instantiate(prefab, parentTransform);
            prefabCount++;

            // �]�w�s�ͦ����d�P���
            CardDisplay cardDisplay = newPrefab.GetComponent<CardDisplay>();
            if (cardDisplay != null)
            {
                cardDisplay.cardData = randomCard;
                cardDisplay.UpdateCardDisplay();
            }

            // �]�w�s�d������l��m
            float xOffset = cardPositions.Count * spacing;
            Vector3 cardPosition = new Vector3(xOffset, 0, 0);
            newPrefab.GetComponent<RectTransform>().localPosition = cardPosition;

            // �O�s�d������l��m
            cardPositions[newPrefab] = cardPosition;

            // �N�d���K�[�� UI ����˴���
            RectTransform cardRect = newPrefab.GetComponent<RectTransform>();
            if (cardRect != null && uiStackDetector != null)
            {
                uiStackDetector.otherUIElements.Add(cardRect);
            }


            // ��s�Ҧ��d�P����m
            UpdateCardPositions();
        }
        else
        {
            Debug.LogWarning("Prefab�Τ�����|���]�m");
        }
    }

    // ��s�Ҧ��d�P����m�A�Ϩ��V�ƦC
    void UpdateCardPositions()
    {
        for (int i = 0; i < parentTransform.childCount; i++)
        {
            Transform card = parentTransform.GetChild(i);
            float xOffset = i * spacing;

            // �]�m�d�P����m
            card.localPosition = new Vector3(xOffset, 0, 0);

            // �]�m�d�P������A�Ϩ�O�������¤W
            card.localRotation = Quaternion.identity;
        }
    }
    public void Drawattackcard()
    {
        if (prefab != null && parentTransform != null && cardData.Count > 0)
        {
            // ���w�ͦ� cardData[0] ���d�P
            Card specifiedCard = cardData[0];

            // �ͦ��s��Prefab
            GameObject newPrefab = Instantiate(prefab, parentTransform);
            prefabCount++;

            // �]�w�s�ͦ����d�P���
            CardDisplay cardDisplay = newPrefab.GetComponent<CardDisplay>();
            if (cardDisplay != null)
            {
                cardDisplay.cardData = specifiedCard;
                cardDisplay.UpdateCardDisplay();
            }

            // �]�w�s�d������l��m
            float xOffset = cardPositions.Count * spacing;
            Vector3 cardPosition = new Vector3(xOffset, 0, 0);
            newPrefab.GetComponent<RectTransform>().localPosition = cardPosition;

            // �O�s�d������l��m
            cardPositions[newPrefab] = cardPosition;

            // �N�d���K�[�� UI ����˴���
            RectTransform cardRect = newPrefab.GetComponent<RectTransform>();
            if (cardRect != null && uiStackDetector != null)
            {
                uiStackDetector.otherUIElements.Add(cardRect);
            }
        }

        UpdateCardPositions();
    }
    public Vector3 GetOriginalPosition(GameObject card)//����ͦ��d���{�b��m
    {
        return cardPositions.ContainsKey(card) ? cardPositions[card] : Vector3.zero;
    }
}
