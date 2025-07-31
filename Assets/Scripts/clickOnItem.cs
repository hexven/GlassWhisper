using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class clickOnItem : MonoBehaviour, IPointerDownHandler
{
    [Header("References")]
    public GameObject minigamePanel;
    public GameObject itemPrefab; // ไอเทม prefab ที่จะเพิ่มเข้า inventory
    [Header("Item Information")]
    public string itemName = "Unknown Item"; // ชื่อไอเทมสำหรับแสดงใน popup
    private InventoryController inventoryController;
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
    public void OnPointerDown(PointerEventData eventData)
    {
        // ลองเพิ่มไอเทมเข้า inventory
        if (inventoryController != null && inventoryController.AddItem(itemPrefab))
        {
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
            gameObject.SetActive(false);
            minigamePanel.SetActive(false);
            // เล่นเสียง (ถ้ามี SoundManager)
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySound2D("ItemPickup"); // หรือเสียงที่เหมาะสม
            }
        }
        else
        {
            // ถ้า inventory เต็ม ให้ทำตามโค้ดเดิม
            Debug.Log("Cannot add item: Inventory is full!");
            gameObject.SetActive(false);
            minigamePanel.SetActive(false);
            // อาจจะแสดง popup แจ้งว่า inventory เต็ม
            if (ItemPickupUIController.Instance != null)
            {
                ItemPickupUIController.Instance.ShowItemPickup("Inventory Full!", null);
            }
        }
    }
}