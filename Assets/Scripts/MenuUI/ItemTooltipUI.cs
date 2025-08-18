using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum NoteDisplayType
{
    Default,
    Horizontal,
    Vertical
}

public class ItemTooltipUI : MonoBehaviour
{
    public static ItemTooltipUI Instance;
    [Header("UI Components")]
    public GameObject tooltipPanel;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;
    public Image itemIconImage;
    public Button openNoteButton;
    public GameObject notePanel;

    [Header("Note Display Options")]
    public Image noteImageDisplay;
    public Image noteImageHorizontal;
    public Image noteImageVertical;

    private Item currentBoxItem;
    private Slot currentBoxSlot;
    public GameObject openBoxButton;
    public GameObject endingButton;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        HideTooltip();
    }

    public void ShowTooltip(string itemName, string itemDescription, Sprite itemIcon, bool hasNote, Sprite noteImage)
    {
        ShowTooltip(itemName, itemDescription, itemIcon, hasNote, noteImage, NoteDisplayType.Default);
    }

    public void ShowTooltip(string itemName, string itemDescription, Sprite itemIcon, bool hasNote, Sprite noteImage, NoteDisplayType displayType)
    {
        if (tooltipPanel != null)
        {
            tooltipPanel.SetActive(true);
            if (itemNameText != null)
                itemNameText.text = itemName;
            if (itemDescriptionText != null)
                itemDescriptionText.text = itemDescription;
            if (itemIconImage != null && itemIcon != null)
                itemIconImage.sprite = itemIcon;
            if (openNoteButton != null)
                openNoteButton.gameObject.SetActive(hasNote);

            openNoteButton.onClick.RemoveAllListeners();
            openNoteButton.onClick.AddListener(() =>
            {
                Item item = currentBoxItem;
                if (item != null && item.diaryPages != null && item.diaryPages.Count > 0)
                {
                    if (DiaryUIController.Instance != null)
                    {
                        DiaryUIController.Instance.ShowDiary(item);
                        HideTooltip();
                    }
                }
                else
                {
                    ShowNotePanel(noteImage, displayType);
                }
            });
        }
    }

    private void ShowNotePanel(Sprite noteImage, NoteDisplayType displayType)
    {
        if (notePanel != null)
        {
            HideAllNoteDisplays();
            switch (displayType)
            {
                case NoteDisplayType.Default:
                    if (noteImageDisplay != null)
                    {
                        noteImageDisplay.sprite = noteImage;
                        noteImageDisplay.gameObject.SetActive(true);
                    }
                    break;
                case NoteDisplayType.Horizontal:
                    if (noteImageHorizontal != null)
                    {
                        noteImageHorizontal.sprite = noteImage;
                        noteImageHorizontal.gameObject.SetActive(true);
                    }
                    break;
                case NoteDisplayType.Vertical:
                    if (noteImageVertical != null)
                    {
                        noteImageVertical.sprite = noteImage;
                        noteImageVertical.gameObject.SetActive(true);
                    }
                    break;
            }
            notePanel.SetActive(true);
        }
    }

    private void HideAllNoteDisplays()
    {
        if (noteImageDisplay != null) noteImageDisplay.gameObject.SetActive(false);
        if (noteImageHorizontal != null) noteImageHorizontal.gameObject.SetActive(false);
        if (noteImageVertical != null) noteImageVertical.gameObject.SetActive(false);
    }

    public void HideTooltip()
    {
        if (tooltipPanel != null) tooltipPanel.SetActive(false);
        if (notePanel != null) notePanel.SetActive(false);
    }

    public void SetCurrentBox(Item item, Slot slot)
    {
        currentBoxItem = item;
        currentBoxSlot = slot;
    }

    public void ShowOpenBoxButton(bool show)
    {
        if (openBoxButton != null) openBoxButton.SetActive(show);
    }

    public void OnClickOpenBox()
    {
        Debug.Log("✅ ปุ่มเปิดกล่องถูกกด");
        if (currentBoxItem == null || currentBoxSlot == null)
        {
            Debug.Log("❌ currentBoxItem หรือ currentBoxSlot เป็น null");
            return;
        }
        InventoryController inv = FindObjectOfType<InventoryController>();
        if (inv.HasItem(currentBoxItem.requiredItem))
        {
            Debug.Log("✅ มีกุญแจใน inventory");
            chooseSystem.oldPic = true;
            inv.RemoveItem(currentBoxItem.requiredItem);
            inv.AddItem(currentBoxItem.rewardItemPrefab);
            GameObject.Destroy(currentBoxSlot.currentItem);
            currentBoxSlot.currentItem = null;
            currentBoxSlot.DeselectSlot();
            HideTooltip();
        }
        else
        {
            Debug.Log("❌ ไม่มีไอเทมที่ใช้เปิดกล่อง");
        }
    }
    public void SetCurrentItem(Item item)
    {
        currentBoxItem = item; // ใช้ currentBoxItem เดิม หรือสร้างตัวแปรใหม่ถ้าต้องการแยก
    }
}