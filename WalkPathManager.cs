using System.Collections.Generic;
using UnityEngine;

public class WalkPathManager : MonoBehaviour//�d���԰����u�}��
{
    // ���}���ܼơA�Ω�s�x�ؼЪ���
    public GameObject targetObject;

    // ���}��k�A�Ω�N�ؼЪ��� WalkPath �� active �ݩʳ]�� false
    public void DeactivateWalkPaths()
    {
        if (targetObject == null)
        {
            Debug.LogWarning("Target object is not assigned.");
            return;
        }

        // ����ؼЪ���W�� Walkable �ե�
        Walkable walkable = targetObject.GetComponent<Walkable>();

        if (walkable == null)
        {
            Debug.LogWarning("Target object does not have a Walkable component.");
            return;
        }

        // �M�� Walkable ���� possiblePaths �ó]�m active = false
        foreach (WalkPath path in walkable.possiblePaths)
        {
            path.active = true;
            Debug.Log($"Deactivated path to target: {path.target?.name ?? "null"}");
        }
    }
}
