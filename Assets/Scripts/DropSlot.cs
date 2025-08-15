using UnityEngine;
using UnityEngine.EventSystems;

public class DropSlot : MonoBehaviour, IDropHandler
{
    public string correctPieceName;
    private bool isOccupied = false;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && !isOccupied)
        {
            if (eventData.pointerDrag.name == correctPieceName)
            {
                // เปลี่ยน parent ก่อน
                eventData.pointerDrag.transform.SetParent(transform);

                // รีเซ็ตตำแหน่งให้อยู่ที่จุดศูนย์กลางของ slot
                RectTransform pieceRect = eventData.pointerDrag.GetComponent<RectTransform>();
                pieceRect.anchoredPosition = Vector2.zero;

                // หรือใช้วิธีนี้เพื่อความแม่นยำมากขึ้น
                // pieceRect.localPosition = Vector3.zero;

                // ล็อกชิ้นส่วนไว้
                DragDrop dragComponent = eventData.pointerDrag.GetComponent<DragDrop>();
                if (dragComponent != null)
                {
                    dragComponent.LockPiece();
                }

                isOccupied = true;

                // เรียกตรวจสอบว่าปริศนาเสร็จสมบูรณ์หรือไม่
                PuzzleManager puzzleManager = FindObjectOfType<PuzzleManager>();
                if (puzzleManager != null)
                {
                    puzzleManager.CheckComplete();
                }

                Debug.Log($"{correctPieceName} ถูกวางใน slot ที่ถูกต้องแล้ว!");
            }
            else
            {
                Debug.Log("ชิ้นส่วนไม่ถูกต้องสำหรับ slot นี้");
            }
        }
    }

    // ฟังก์ชันสำหรับเคลียร์ slot (ถ้าต้องการรีเซ็ต)
    public void ClearSlot()
    {
        isOccupied = false;
        if (transform.childCount > 0)
        {
            DragDrop dragComponent = transform.GetChild(0).GetComponent<DragDrop>();
            if (dragComponent != null)
            {
                dragComponent.UnlockPiece();
            }
        }
    }
}