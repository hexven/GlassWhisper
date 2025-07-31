using UnityEngine;

public class openMiniGame : MonoBehaviour, IInteractable
{
    public bool IsOpened { get; private set; }
    public string ChestID { get; private set; }
    public GameObject minigameCanvas;
    public Sprite openedSprite;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ChestID ??= GlobalHelper.GenerateUniqueID(gameObject);
        minigameCanvas.SetActive(false);
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
        minigameCanvas.SetActive(true);
        SetOpened(true);
        if (minigameCanvas)
        {
            //minigameCanvas.SetActive(true);
            //Debug.Log("YO");
            //// แทนที่ Vector3.down ด้วยการเลื่อนข้างหรือไม่เลื่อนเลย
            //GameObject droppedItem = Instantiate(itemPrefabs, transform.position, Quaternion.identity);
            //droppedItem.GetComponent<BounceEffect>().StartBounce();
        }
    }
    public void SetOpened(bool opened)
    {
        IsOpened = opened; // แก้ไขจาก if(IsOpened = opened) เป็น IsOpened = opened
        if (IsOpened)
        {
            GetComponent<SpriteRenderer>().sprite = openedSprite;
            //Debug.Log("YO");
        }
        chooseSystem.note = true;
    }
}

