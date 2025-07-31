using Unity.Cinemachine;
using UnityEditor.Experimental;
using UnityEngine;

using System.Collections;

public class MapTransation : MonoBehaviour
{
    [SerializeField] PolygonCollider2D mapBoundry;
    CinemachineConfiner2D confiner;
    [SerializeField] Direction direction;
    [SerializeField] Transform teleportTargetPosition;

    [Header("Transition Settings")]
    [SerializeField] bool useFadeTransition = true; // ����������͡�Դ/�Դ fade

    [Header("Item Requirements")]
    [SerializeField] bool requireItem = false; // �Դ/�Դ���������
    [SerializeField] RequiredItemType requiredItemType = RequiredItemType.SingleBoolean; // �����������

    [Header("Single Boolean Item")]
    [SerializeField] string booleanItemName = "key"; // ���� boolean variable ����ͧ���

    [Header("Multiple Boolean Items")]
    [SerializeField] string[] multipleItemNames = { "nahh8", "oldPic" }; // �������µ��
    [SerializeField] bool useORLogic = false; // true = �յ��㴵��˹��, false = ��ͧ�դú�ء��� (AND)

    [Header("Choose Item System")]
    [SerializeField] int[] requiredChooseItems = { 2, 1, 3 }; // ��ҷ���ͧ�������Ѻ chooseItem01, 02, 03
    [SerializeField] bool checkChooseItems = false; // �Դ/�Դ����� choose items

    [Header("Feedback Messages")]
    [SerializeField] bool showFeedbackMessage = true;
    [SerializeField] string noItemMessage = "�س��ͧ�����������繡�͹";
    [SerializeField] float messageDisplayTime = 2f;
    [SerializeField] GameObject messagePanel; // Panel ����Ѻ�ʴ���ͤ���
    [SerializeField] UnityEngine.UI.Text messageText; // Text component ����Ѻ�ʴ���ͤ���

    [Header("Fade Settings")]
    [SerializeField] float fadeSpeed = 2f;
    [SerializeField] GameObject fadeCanvas;
    [SerializeField] UnityEngine.UI.Image fadeImage;

    enum Direction { Up, Down, Left, Right, Teleport }
    enum RequiredItemType { SingleBoolean, MultipleBooleans, ChooseItems, BooleanAndChoose, AnyOfMultipleBooleans }

    private bool isTransitioning = false;

    private void Awake()
    {
        confiner = FindObjectOfType<CinemachineConfiner2D>();

        // ���ҧ fade canvas ੾������͵�ͧ����� fade
        if (useFadeTransition && fadeCanvas == null)
        {
            CreateFadeCanvas();
        }

        // ��͹ message panel �͹�������
        if (messagePanel != null)
        {
            messagePanel.SetActive(false);
        }
    }

    private void CreateFadeCanvas()
    {
        // ���ҧ Canvas
        GameObject canvasObj = new GameObject("FadeCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 1000; // ��������ҹ���ش

        // ���ҧ CanvasScaler
        canvasObj.AddComponent<UnityEngine.UI.CanvasScaler>();
        canvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();

        // ���ҧ Image ����Ѻ fade
        GameObject imageObj = new GameObject("FadeImage");
        imageObj.transform.SetParent(canvasObj.transform, false);

        fadeImage = imageObj.AddComponent<UnityEngine.UI.Image>();
        fadeImage.color = new Color(0, 0, 0, 0); // �����������

        // ��駤������ͺ����˹�Ҩͷ�����
        RectTransform rectTransform = fadeImage.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.sizeDelta = Vector2.zero;
        rectTransform.anchoredPosition = Vector2.zero;

        fadeCanvas = canvasObj;

        // ��͹ canvas ���͹�������
        fadeCanvas.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isTransitioning)
        {
            // ��������͹�� transition
            if (requireItem && !CheckItemRequirements())
            {
                if (showFeedbackMessage)
                {
                    StartCoroutine(ShowMessage(noItemMessage));
                }
                return; // ���� transition ������������
            }

            // ���͡�� transition Ẻ�˹�����õ�駤��
            if (useFadeTransition)
            {
                StartCoroutine(TransitionWithFade(collision.gameObject));
            }
            else
            {
                StartCoroutine(InstantTransition(collision.gameObject));
            }
        }
    }

    private bool CheckItemRequirements()
    {
        switch (requiredItemType)
        {
            case RequiredItemType.SingleBoolean:
                return CheckSingleBooleanItem();

            case RequiredItemType.MultipleBooleans:
                return useORLogic ? CheckAnyBooleanItems() : CheckAllBooleanItems();

            case RequiredItemType.AnyOfMultipleBooleans:
                return CheckAnyBooleanItems();

            case RequiredItemType.ChooseItems:
                return CheckChooseItems();

            case RequiredItemType.BooleanAndChoose:
                bool boolResult = useORLogic ? CheckAnyBooleanItems() : CheckAllBooleanItems();
                return boolResult && CheckChooseItems();

            default:
                return true;
        }
    }

    private bool CheckSingleBooleanItem()
    {
        if (string.IsNullOrEmpty(booleanItemName))
            return true;

        // �� Reflection ������Ҷ֧ static boolean �ҡ chooseSystem
        var field = typeof(chooseSystem).GetField(booleanItemName,
            System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

        if (field != null && field.FieldType == typeof(bool))
        {
            return (bool)field.GetValue(null);
        }

        Debug.LogWarning($"��辺 boolean item: {booleanItemName} � chooseSystem");
        return false;
    }

    private bool CheckAnyBooleanItems()
    {
        if (multipleItemNames == null || multipleItemNames.Length == 0)
            return true;

        foreach (string itemName in multipleItemNames)
        {
            if (string.IsNullOrEmpty(itemName))
                continue;

            var field = typeof(chooseSystem).GetField(itemName,
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

            if (field != null && field.FieldType == typeof(bool))
            {
                if ((bool)field.GetValue(null))
                {
                    return true; // ��������������˹�觷���� true ��� return true
                }
            }
            else
            {
                Debug.LogWarning($"��辺 boolean item: {itemName} � chooseSystem");
            }
        }

        return false; // ������������ true
    }

    private bool CheckAllBooleanItems()
    {
        if (multipleItemNames == null || multipleItemNames.Length == 0)
            return true;

        foreach (string itemName in multipleItemNames)
        {
            if (string.IsNullOrEmpty(itemName))
                continue;

            var field = typeof(chooseSystem).GetField(itemName,
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

            if (field != null && field.FieldType == typeof(bool))
            {
                if (!(bool)field.GetValue(null))
                {
                    return false; // ��������������˹�觷���� false ��� return false
                }
            }
            else
            {
                Debug.LogWarning($"��辺 boolean item: {itemName} � chooseSystem");
                return false;
            }
        }

        return true; // �ء������� true
    }

    private bool CheckChooseItems()
    {
        if (!checkChooseItems || requiredChooseItems == null)
            return true;

        // �� chooseItem01
        if (requiredChooseItems.Length > 0)
        {
            var chooseItem01Type = System.Type.GetType("chooseItem01");
            if (chooseItem01Type != null)
            {
                var field = chooseItem01Type.GetField("chooseItem",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                if (field != null && (int)field.GetValue(null) != requiredChooseItems[0])
                {
                    return false;
                }
            }
        }

        // �� chooseItem02
        if (requiredChooseItems.Length > 1)
        {
            var chooseItem02Type = System.Type.GetType("chooseItem02");
            if (chooseItem02Type != null)
            {
                var field = chooseItem02Type.GetField("chooseItem",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                if (field != null && (int)field.GetValue(null) != requiredChooseItems[1])
                {
                    return false;
                }
            }
        }

        // �� chooseItem03
        if (requiredChooseItems.Length > 2)
        {
            var chooseItem03Type = System.Type.GetType("chooseItem03");
            if (chooseItem03Type != null)
            {
                var field = chooseItem03Type.GetField("chooseItem3",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                if (field != null && (int)field.GetValue(null) != requiredChooseItems[2])
                {
                    return false;
                }
            }
        }

        return true;
    }

    private IEnumerator ShowMessage(string message)
    {
        if (messagePanel != null && messageText != null)
        {
            messageText.text = message;
            messagePanel.SetActive(true);

            yield return new WaitForSeconds(messageDisplayTime);

            messagePanel.SetActive(false);
        }
        else
        {
            Debug.Log(message);
        }
    }

    private IEnumerator TransitionWithFade(GameObject player)
    {
        isTransitioning = true;

        // �Դ fade canvas
        fadeCanvas.SetActive(true);

        // Fade Out (�ҡ����繴�)
        yield return StartCoroutine(FadeToBlack());

        // �ѻവ���˹觼�������� camera bounds
        confiner.BoundingShape2D = mapBoundry;
        UpdatePlayerPosition(player);

        // ����� camera �Դ�����������ѧ���˹�����
        yield return new WaitForSeconds(0.1f);

        // Fade In (�ҡ�������)
        yield return StartCoroutine(FadeToTransparent());

        // ��͹ fade canvas
        fadeCanvas.SetActive(false);

        isTransitioning = false;
    }

    private IEnumerator InstantTransition(GameObject player)
    {
        isTransitioning = true;

        // �ѻവ���˹觼�������� camera bounds �ѹ��
        confiner.BoundingShape2D = mapBoundry;
        UpdatePlayerPosition(player);

        // �� frame ����������� transition �������
        yield return null;

        isTransitioning = false;
    }

    private IEnumerator FadeToBlack()
    {
        Color startColor = fadeImage.color;
        Color targetColor = new Color(0, 0, 0, 1); // �մӷֺ

        float elapsed = 0f;

        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime * fadeSpeed;
            fadeImage.color = Color.Lerp(startColor, targetColor, elapsed);
            yield return null;
        }

        fadeImage.color = targetColor;
    }

    private IEnumerator FadeToTransparent()
    {
        Color startColor = fadeImage.color;
        Color targetColor = new Color(0, 0, 0, 0); // ��

        float elapsed = 0f;

        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime * fadeSpeed;
            fadeImage.color = Color.Lerp(startColor, targetColor, elapsed);
            yield return null;
        }

        fadeImage.color = targetColor;
    }

    private void UpdatePlayerPosition(GameObject player)
    {
        if (direction == Direction.Teleport)
        {
            player.transform.position = teleportTargetPosition.position;
            return;
        }

        Vector3 additivePos = player.transform.position;
        switch (direction)
        {
            case Direction.Up:
                additivePos.y += 2;
                break;
            case Direction.Down:
                additivePos.y -= 2;
                break;
            case Direction.Left:
                additivePos.x += 2;
                break;
            case Direction.Right:
                additivePos.x -= 2;
                break;
        }
        player.transform.position = additivePos;
    }

    // �ѧ�ѹ����Ѻ����¹��õ�駤�� fade ����ѹ���
    public void SetUseFadeTransition(bool useFade)
    {
        useFadeTransition = useFade;

        // ����Դ�� fade ����ѧ����� canvas ������ҧ����
        if (useFadeTransition && fadeCanvas == null)
        {
            CreateFadeCanvas();
        }
    }

    // �ѧ�ѹ����Ѻ��ʶҹлѨ�غѹ
    public bool IsUsingFadeTransition()
    {
        return useFadeTransition;
    }

    // �ѧ�ѹ����Ѻ����¹��õ�駤������
    public void SetRequireItem(bool requireItem)
    {
        this.requireItem = requireItem;
    }

    // �ѧ�ѹ����Ѻ����Ҽ������������������
    public bool HasRequiredItems()
    {
        return !requireItem || CheckItemRequirements();
    }
}