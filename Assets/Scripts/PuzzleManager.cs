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
        
        Debug.Log($"�����ǹ���١��ͧ: {correctPieces}/{slots.Length}");
        
        if (correctPieces == slots.Length)
        {
            Debug.Log("?? ���ȹ���������ó�!");
            OnPuzzleComplete();
        }
    }
    
    // �ѧ��ѹ������¡����ͻ��ȹ���������ó�
    private void OnPuzzleComplete()
    {
        // ������á�з��������������ó� ��
        // - �ʴ� UI ����ʴ������Թ��
        // - ������§/�Ϳ࿡��
        // - �ѹ�֡��ṹ
        // - ��ѧ�дѺ�Ѵ�


        if (inventoryController != null && inventoryController.AddItem(itemPrefab))
        {
            gameObject.SetActive(false);
            puzzlePanel.SetActive(false);
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
            
            // ������§ (����� SoundManager)
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySound2D("ItemPickup"); // �������§����������
            }
        }

        // ��͡�ء�����ǹ���ͻ�ͧ�ѹ�������¹�ŧ
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
    
    // �ѧ��ѹ���絻��ȹ�
    public void ResetPuzzle()
    {
        foreach (var slot in slots)
        {
            slot.ClearSlot();
        }
        Debug.Log("���絻��ȹ�����");
    }
}