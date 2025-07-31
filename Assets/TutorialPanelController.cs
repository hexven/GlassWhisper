using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class TutorialPanelController : MonoBehaviour
{
    [Header("UI References")]
    public GameObject tutorialPanel;
    public Image tutorialImage;
    public Button closeButton;
    public Button previousButton;
    public Button nextButton;
    public TextMeshProUGUI pageIndicator; // แสดงหน้าปัจจุบัน เช่น "1/5"

    [Header("Tutorial Settings")]
    public bool showOnStart = true;
    public List<TutorialPage> tutorialPages = new List<TutorialPage>();

    [System.Serializable]
    public class TutorialPage
    {
        public Sprite image;
    }

    private int currentPageIndex = 0;

    void Start()
    {
        // ตั้งค่า UI เริ่มต้น
        SetupUI();

        // แสดง tutorial เมื่อเริ่มเกมถ้าตั้งค่าไว้
        if (showOnStart && tutorialPages.Count > 0)
        {
            ShowTutorial();
        }
        else
        {
            tutorialPanel.SetActive(false);
        }
    }

    void SetupUI()
    {
        // ตั้งค่า Event Listeners
        if (closeButton != null)
            closeButton.onClick.AddListener(CloseTutorial);

        if (previousButton != null)
            previousButton.onClick.AddListener(PreviousPage);

        if (nextButton != null)
            nextButton.onClick.AddListener(NextPage);
    }

    public void ShowTutorial()
    {
        if (tutorialPages.Count == 0)
        {
            Debug.LogWarning("ไม่มีหน้า Tutorial ให้แสดง!");
            return;
        }

        tutorialPanel.SetActive(true);
        currentPageIndex = 0;
        UpdateTutorialDisplay();

        // หยุดเวลาเกมถ้าต้องการ
        Time.timeScale = 0f;
    }

    public void CloseTutorial()
    {
        tutorialPanel.SetActive(false);

        // เริ่มเวลาเกมต่อ
        Time.timeScale = 1f;
    }

    public void NextPage()
    {
        if (currentPageIndex < tutorialPages.Count - 1)
        {
            currentPageIndex++;
            UpdateTutorialDisplay();
        }
    }

    public void PreviousPage()
    {
        if (currentPageIndex > 0)
        {
            currentPageIndex--;
            UpdateTutorialDisplay();
        }
    }

    void UpdateTutorialDisplay()
    {
        if (tutorialPages.Count == 0 || currentPageIndex < 0 || currentPageIndex >= tutorialPages.Count)
            return;

        TutorialPage currentPage = tutorialPages[currentPageIndex];

        // อัพเดทรูปภาพ
        if (tutorialImage != null && currentPage.image != null)
        {
            tutorialImage.sprite = currentPage.image;
        }

        // อัพเดท Page Indicator
        if (pageIndicator != null)
        {
            pageIndicator.text = $"{currentPageIndex + 1}/{tutorialPages.Count}";
        }

        // อัพเดทสถานะปุ่ม
        UpdateButtonStates();
    }

    void UpdateButtonStates()
    {
        // ปุ่ม Previous - ปิดถ้าอยู่หน้าแรก
        if (previousButton != null)
        {
            previousButton.interactable = currentPageIndex > 0;
        }

        // ปุ่ม Next - ปิดถ้าอยู่หน้าสุดท้าย
        if (nextButton != null)
        {
            nextButton.interactable = currentPageIndex < tutorialPages.Count - 1;
        }
    }

    // ฟังก์ชันสำหรับเรียกใช้จากภายนอก
    public void AddTutorialPage(Sprite image)
    {
        TutorialPage newPage = new TutorialPage();
        newPage.image = image;
        tutorialPages.Add(newPage);
    }

    public void ClearTutorialPages()
    {
        tutorialPages.Clear();
    }

    public void GoToPage(int pageIndex)
    {
        if (pageIndex >= 0 && pageIndex < tutorialPages.Count)
        {
            currentPageIndex = pageIndex;
            UpdateTutorialDisplay();
        }
    }

    // สำหรับควบคุมด้วยคีย์บอร์ด (ถ้าต้องการ)
    void Update()
    {
        // กด T เพื่อเปิด/ปิด Tutorial
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (tutorialPanel.activeInHierarchy)
            {
                CloseTutorial();
            }
            else
            {
                ShowTutorial();
            }
        }

        if (tutorialPanel.activeInHierarchy)
        {
            // กด ESC เพื่อปิด
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseTutorial();
            }

            // กดลูกศรซ้าย-ขวา เพื่อนำทาง
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                PreviousPage();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                NextPage();
            }
        }
    }

    // ฟังก์ชันสำหรับ Debug
    [ContextMenu("Test Show Tutorial")]
    void TestShowTutorial()
    {
        ShowTutorial();
    }
}