using System.Collections.Generic;
using UnityEngine;
public class box : MonoBehaviour, IInteractable
{
    public bool IsOpened { get; private set; }
    public string ChestID { get; private set; }
    public GameObject itemPrefabs;
    public Sprite openedSprite;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ChestID ??= GlobalHelper.GenerateUniqueID(gameObject);
    }
    public bool CanInteract()
    {
        return !IsOpened;
    }
    public void Interact()
    {
        if (!CanInteract()) return;
        OpenChest(); // เรียก OpenedChest() ใน Interact()
    }
    private void OpenChest()
    {
        SoundManager.Instance.PlaySound2D("Click");
        SetOpened(true);
        if (itemPrefabs)
        {
            // แทนที่ Vector3.down ด้วยการเลื่อนข้างหรือไม่เลื่อนเลย
            GameObject droppedItem = Instantiate(itemPrefabs, transform.position, Quaternion.identity);
            droppedItem.GetComponent<BounceEffect>().StartBounce();
        }
    }
    public void SetOpened(bool opened)
    {
        IsOpened = opened; // แก้ไขจาก if(IsOpened = opened) เป็น IsOpened = opened
        if (IsOpened)
        {
            GetComponent<SpriteRenderer>().sprite = openedSprite;
        }
    }
}