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
    public TextMeshProUGUI pageIndicator; // �ʴ�˹�һѨ�غѹ �� "1/5"

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
        // ��駤�� UI �������
        SetupUI();

        // �ʴ� tutorial ��������������ҵ�駤�����
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
        // ��駤�� Event Listeners
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
            Debug.LogWarning("�����˹�� Tutorial ����ʴ�!");
            return;
        }

        tutorialPanel.SetActive(true);
        currentPageIndex = 0;
        UpdateTutorialDisplay();

        // ��ش��������ҵ�ͧ���
        Time.timeScale = 0f;
    }

    public void CloseTutorial()
    {
        tutorialPanel.SetActive(false);

        // ��������������
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

        // �Ѿഷ�ٻ�Ҿ
        if (tutorialImage != null && currentPage.image != null)
        {
            tutorialImage.sprite = currentPage.image;
        }

        // �Ѿഷ Page Indicator
        if (pageIndicator != null)
        {
            pageIndicator.text = $"{currentPageIndex + 1}/{tutorialPages.Count}";
        }

        // �Ѿഷʶҹл���
        UpdateButtonStates();
    }

    void UpdateButtonStates()
    {
        // ���� Previous - �Դ�������˹���á
        if (previousButton != null)
        {
            previousButton.interactable = currentPageIndex > 0;
        }

        // ���� Next - �Դ�������˹���ش����
        if (nextButton != null)
        {
            nextButton.interactable = currentPageIndex < tutorialPages.Count - 1;
        }
    }

    // �ѧ��ѹ����Ѻ���¡��ҡ��¹͡
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

    // ����Ѻ�Ǻ������¤������ (��ҵ�ͧ���)
    void Update()
    {
        // �� T �����Դ/�Դ Tutorial
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
            // �� ESC ���ͻԴ
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseTutorial();
            }

            // ���١�ë���-��� ���͹ӷҧ
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

    // �ѧ��ѹ����Ѻ Debug
    [ContextMenu("Test Show Tutorial")]
    void TestShowTutorial()
    {
        ShowTutorial();
    }
}