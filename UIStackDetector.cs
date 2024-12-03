using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class UIStackDetector : MonoBehaviour
{

    // �n�˴��O�_�Q���|���ؼ� UI ����
    public RectTransform targetUIElement;

    // �i����|�b�ؼФW����L UI �����C��
    public List<RectTransform> otherUIElements;

    // �аO�O�_�Q���|
    private bool isStacked = false;

    // �i��G�b�Q���|����ܪ�����
    public Image overlayImage; // �i�H�O�@�ӥb�z�����Ϲ��Ӫ�ܰ��|���A

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

        // �ˬd�O�_����L UI �����P�ؼЭ��|
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
                Debug.Log($"{targetUIElement.name} �Q���|�F�I");
                if (overlayImage != null)
                {
                    overlayImage.enabled = true;
                }
            }
            else
            {
                Debug.Log($"{targetUIElement.name} �S���Q���|�C");
                if (overlayImage != null)
                {
                    overlayImage.enabled = false;
                }
            }
        }
    }

    // �ˬd��� RectTransform �O�_���|
    bool IsOverlapping(RectTransform rect1, RectTransform rect2)
    {
        Rect r1 = GetWorldRect(rect1);
        Rect r2 = GetWorldRect(rect2);
        return r1.Overlaps(r2);
    }

    // �N RectTransform �ഫ���@�ɧ��ФU�� Rect
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
