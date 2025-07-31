using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable] // อนุญาตให้แสดงใน Inspector
public class DiaryPage
{
    public string Title;    // หัวข้อของหน้า
    [TextArea(3, 10)]       // อนุญาตให้ใส่ข้อความยาวใน Inspector
    public string Content;  // เนื้อหาของหน้า
}

public class Item : MonoBehaviour
{
    public int ID;
    public string Name;
    [TextArea(3, 5)]
    public string Description;
    public bool hasNote;
    public Sprite noteImage;
    public List<DiaryPage> diaryPages;  // เปลี่ยนเป็น List<DiaryPage>
    public NoteDisplayType noteDisplayType = NoteDisplayType.Default;
    public bool isBox;
    public GameObject requiredItem;
    public GameObject rewardItemPrefab;

    public static int idStatic;

    public virtual void PickUp()
    {
        Sprite itemIcon = null;
        Image imageComponent = GetComponent<Image>();
        if (imageComponent != null)
        {
            itemIcon = imageComponent.sprite;
        }
        if (ItemPickupUIController.Instance != null)
        {
            ItemPickupUIController.Instance.ShowItemPickup(Name, itemIcon);
        }
    }

    private void Update()
    {
        idStatic = ID;
    }
}