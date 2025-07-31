using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class ClickOutsideImageToClose : MonoBehaviour
{
    public GameObject notePanel;       // Panel ������
    public Image noteImage;            // ੾���ٻ�Ҿ������

    void Update()
    {
        if (notePanel.activeSelf && Input.GetMouseButtonDown(0))
        {
            if (!IsPointerOverTarget(noteImage.gameObject))
            {
                notePanel.SetActive(false);
            }
        }
    }

    bool IsPointerOverTarget(GameObject target)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var result in results)
        {
            // ��Ǩ�ͺ��� pointer ���躹 noteImage ���ͺ��١�ͧ�ѹ
            if (result.gameObject == target || result.gameObject.transform.IsChildOf(target.transform))
            {
                return true; // ���躹�ٻ�Ҿ������
            }
        }
        return false; // �͡�Ҿ
    }
}