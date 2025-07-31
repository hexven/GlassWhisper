using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiaryUIController : MonoBehaviour
{
    public static DiaryUIController Instance;
    [SerializeField] private GameObject diaryPanel;
    [SerializeField] private TextMeshProUGUI diaryTitleText; // เพิ่มสำหรับแสดงหัวข้อ
    [SerializeField] private TextMeshProUGUI diaryText;     // ใช้แสดงเนื้อหา
    [SerializeField] private Image diaryImage;
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button closeButton;

    private Item currentItem;
    private int currentPageIndex;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (diaryPanel != null)
        {
            diaryPanel.SetActive(false);
        }
    }

    public void ShowDiary(Item item)
    {
        if (item == null || !item.hasNote || item.diaryPages == null || item.diaryPages.Count == 0)
        {
            return;
        }

        currentItem = item;
        currentPageIndex = 0;
        diaryPanel.SetActive(true);

        if (ItemTooltipUI.Instance != null)
        {
            ItemTooltipUI.Instance.HideTooltip();
        }

        UpdateDiaryDisplay();

        previousButton.onClick.RemoveAllListeners();
        nextButton.onClick.RemoveAllListeners();
        closeButton.onClick.RemoveAllListeners();

        previousButton.onClick.AddListener(PreviousPage);
        nextButton.onClick.AddListener(NextPage);
        closeButton.onClick.AddListener(HideDiary);
    }

    private void UpdateDiaryDisplay()
    {
        if (diaryTitleText != null && currentItem.diaryPages.Count > currentPageIndex)
        {
            diaryTitleText.text = currentItem.diaryPages[currentPageIndex].Title;
        }
        if (diaryText != null && currentItem.diaryPages.Count > currentPageIndex)
        {
            diaryText.text = currentItem.diaryPages[currentPageIndex].Content;
        }

        if (diaryImage != null)
        {
            diaryImage.gameObject.SetActive(currentItem.noteImage != null);
            if (currentItem.noteImage != null)
            {
                diaryImage.sprite = currentItem.noteImage;
            }
        }

        previousButton.interactable = currentPageIndex > 0;
        nextButton.interactable = currentPageIndex < currentItem.diaryPages.Count - 1;
    }

    private void PreviousPage()
    {
        if (currentPageIndex > 0)
        {
            currentPageIndex--;
            UpdateDiaryDisplay();
        }
    }

    private void NextPage()
    {
        if (currentPageIndex < currentItem.diaryPages.Count - 1)
        {
            currentPageIndex++;
            UpdateDiaryDisplay();
        }
    }

    public void HideDiary()
    {
        diaryPanel.SetActive(false);
        currentItem = null;
        currentPageIndex = 0;
    }
}