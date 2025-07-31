using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class clickOnItem : MonoBehaviour, IPointerDownHandler
{
    [Header("References")]
    public GameObject minigamePanel;
    public GameObject itemPrefab; // ���� prefab ����������� inventory
    [Header("Item Information")]
    public string itemName = "Unknown Item"; // ������������Ѻ�ʴ�� popup
    private InventoryController inventoryController;
    void Start()
    {
        // �� InventoryController 㹩ҡ
        inventoryController = FindObjectOfType<InventoryController>();
        // �������� itemPrefab ����˹���� ����� gameObject ����ͧ
        if (itemPrefab == null)
        {
            itemPrefab = gameObject;
        }
        // �������ժ������� �������� GameObject
        if (itemName == "Unknown Item")
        {
            itemName = gameObject.name;
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        // �ͧ����������� inventory
        if (inventoryController != null && inventoryController.AddItem(itemPrefab))
        {
            // ������������ �ʴ� popup ���� ShowItemPickup �ç�
            if (ItemPickupUIController.Instance != null)
            {
                // �� sprite �ͧ����
                Sprite itemIcon = null;
                // �� sprite �ҡ itemPrefab ��͹
                if (itemPrefab != null)
                {
                    Image prefabImage = itemPrefab.GetComponent<Image>();
                    if (prefabImage != null)
                    {
                        itemIcon = prefabImage.sprite;
                    }
                }
                // �������� �����Ҩҡ GameObject �Ѩ�غѹ
                if (itemIcon == null)
                {
                    Image imageComponent = GetComponent<Image>();
                    if (imageComponent != null)
                    {
                        itemIcon = imageComponent.sprite;
                    }
                }
                // ���¡�� ShowItemPickup �ç�
                ItemPickupUIController.Instance.ShowItemPickup(itemName, itemIcon);
            }

            // �Դ panels ��������
            gameObject.SetActive(false);
            minigamePanel.SetActive(false);
            // ������§ (����� SoundManager)
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySound2D("ItemPickup"); // �������§����������
            }
        }
        else
        {
            // ��� inventory ��� ���ӵ�������
            Debug.Log("Cannot add item: Inventory is full!");
            gameObject.SetActive(false);
            minigamePanel.SetActive(false);
            // �Ҩ���ʴ� popup ����� inventory ���
            if (ItemPickupUIController.Instance != null)
            {
                ItemPickupUIController.Instance.ShowItemPickup("Inventory Full!", null);
            }
        }
    }
}