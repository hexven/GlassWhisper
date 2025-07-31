using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class ClickOutsideImageToClose : MonoBehaviour
{
    public GameObject notePanel;       // Panel ทั้งหมด
    public Image noteImage;            // เฉพาะรูปภาพจดหมาย

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
            // ตรวจสอบว่า pointer อยู่บน noteImage หรือบนลูกของมัน
            if (result.gameObject == target || result.gameObject.transform.IsChildOf(target.transform))
            {
                return true; // อยู่บนรูปภาพจดหมาย
            }
        }
        return false; // นอกภาพ
    }
}