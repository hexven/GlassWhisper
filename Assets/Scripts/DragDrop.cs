using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform parentAfterDrag;
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private bool isLocked = false; // เพิ่มตัวแปรล็อก

    void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // ป้องกันการลากถ้าถูกล็อกแล้ว
        if (isLocked) return;

        parentAfterDrag = transform.parent;
        transform.SetParent(canvas.transform);
        transform.SetAsLastSibling();
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isLocked) return;

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isLocked) return;

        // ตรวจสอบว่าถูกวางใน slot ที่ถูกต้องหรือไม่
        bool droppedInCorrectSlot = false;

        // หาว่าถูก drop ใน slot ไหน
        foreach (var result in eventData.hovered)
        {
            DropSlot slot = result.GetComponent<DropSlot>();
            if (slot != null && gameObject.name == slot.correctPieceName)
            {
                droppedInCorrectSlot = true;
                break;
            }
        }

        // ถ้าไม่ได้วางใน slot ที่ถูกต้อง ให้กลับไปตำแหน่งเดิม
        if (!droppedInCorrectSlot)
        {
            transform.SetParent(parentAfterDrag);
        }

        canvasGroup.blocksRaycasts = true;
    }

    // ฟังก์ชันสำหรับล็อกชิ้นส่วน
    public void LockPiece()
    {
        isLocked = true;
        canvasGroup.interactable = false; // ทำให้ไม่สามารถโต้ตอบได้
    }

    // ฟังก์ชันสำหรับปลดล็อก (ถ้าต้องการ)
    public void UnlockPiece()
    {
        isLocked = false;
        canvasGroup.interactable = true;
    }
}