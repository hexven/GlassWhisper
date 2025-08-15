using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PuzzleManager : MonoBehaviour
{
    public DropSlot[] slots;
    private InventoryController inventoryController;
    public GameObject itemPrefab;
    public string itemName = "Unknown Item";
    public GameObject puzzlePanel;

    void Start()
    {
        // หา InventoryController ในฉาก
        inventoryController = FindObjectOfType<InventoryController>();
        // ถ้าไม่มี itemPrefab ที่กำหนดไว้ ให้ใช้ gameObject นี้เอง
        if (itemPrefab == null)
        {
            itemPrefab = gameObject;
        }
        // ถ้าไม่มีชื่อไอเทม ให้ใช้ชื่อ GameObject
        if (itemName == "Unknown Item")
        {
            itemName = gameObject.name;
        }
    }

    public void CheckComplete()
    {
        int correctPieces = 0;
        
        foreach (var slot in slots)
        {
            if (slot.transform.childCount > 0 && 
                slot.transform.GetChild(0).name == slot.correctPieceName)
            {
                correctPieces++;
            }
        }
        
        Debug.Log($"ชิ้นส่วนที่ถูกต้อง: {correctPieces}/{slots.Length}");
        
        if (correctPieces == slots.Length)
        {
            Debug.Log("?? ปริศนาเสร็จสมบูรณ์!");
            OnPuzzleComplete();
        }
    }
    
    // ฟังก์ชันที่เรียกเมื่อปริศนาเสร็จสมบูรณ์
    private void OnPuzzleComplete()
    {
        // เพิ่มการกระทำเมื่อเสร็จสมบูรณ์ เช่น
        // - แสดง UI การแสดงความยินดี
        // - เล่นเสียง/เอฟเฟกต์
        // - บันทึกคะแนน
        // - ไปยังระดับถัดไป


        if (inventoryController != null && inventoryController.AddItem(itemPrefab))
        {
            gameObject.SetActive(false);
            puzzlePanel.SetActive(false);
            // ถ้าเพิ่มสำเร็จ แสดง popup โดยใช้ ShowItemPickup ตรงๆ
            if (ItemPickupUIController.Instance != null)
            {
                // หา sprite ของไอเทม
                Sprite itemIcon = null;
                // หา sprite จาก itemPrefab ก่อน
                if (itemPrefab != null)
                {
                    Image prefabImage = itemPrefab.GetComponent<Image>();
                    if (prefabImage != null)
                    {
                        itemIcon = prefabImage.sprite;
                    }
                }
                // ถ้าไม่เจอ ค่อยหาจาก GameObject ปัจจุบัน
                if (itemIcon == null)
                {
                    Image imageComponent = GetComponent<Image>();
                    if (imageComponent != null)
                    {
                        itemIcon = imageComponent.sprite;
                    }
                }
                // เรียกใช้ ShowItemPickup ตรงๆ
                ItemPickupUIController.Instance.ShowItemPickup(itemName, itemIcon);
            }

            // ปิด panels ตามโค้ดเดิม
            
            // เล่นเสียง (ถ้ามี SoundManager)
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySound2D("ItemPickup"); // หรือเสียงที่เหมาะสม
            }
        }

        // ล็อกทุกชิ้นส่วนเพื่อป้องกันการเปลี่ยนแปลง
        foreach (var slot in slots)
        {
            if (slot.transform.childCount > 0)
            {
                DragDrop dragComponent = slot.transform.GetChild(0).GetComponent<DragDrop>();
                if (dragComponent != null)
                {
                    dragComponent.LockPiece();
                }
            }
        }
    }
    
    // ฟังก์ชันรีเซ็ตปริศนา
    public void ResetPuzzle()
    {
        foreach (var slot in slots)
        {
            slot.ClearSlot();
        }
        Debug.Log("รีเซ็ตปริศนาแล้ว");
    }
}