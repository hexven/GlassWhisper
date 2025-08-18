using Unity.Cinemachine;
using UnityEditor.Experimental;
using UnityEngine;
using System.Collections;

public class MapTransation : MonoBehaviour
{
    // ��õ�駤�Ҿ�鹰ҹ����Ѻ Map Transition
    [SerializeField] PolygonCollider2D mapBoundry; // �ͺࢵ�ͧ�������
    CinemachineConfiner2D confiner; // ��ǤǺ����ͺࢵ���ͧ
    [SerializeField] Direction direction; // ��ȷҧ�������͹���¼�����
    [SerializeField] Transform teleportTargetPosition; // ���˹������������Ѻ Teleport

    // ��õ�駤���Ϳ�硵�������¹���
    [Header("Transition Settings")]
    [SerializeField] bool useFadeTransition = true; // �Դ/�Դ�Ϳ�硵� fade
    [SerializeField] bool oneTimeUse = false; // �������������

    // ��õ�駤�����͹����������ͧ���
    [Header("Item Requirements")]
    [SerializeField] bool requireItem = false; // �Դ/�Դ����������
    [SerializeField] RequiredItemType requiredItemType = RequiredItemType.SingleBoolean; // ����������������

    // ��õ�駤������Ѻ����������� Boolean �����
    [Header("Single Boolean Item")]
    [SerializeField] string booleanItemName = "key"; // ���͵���� boolean ����ͧ���

    // ��õ�駤������Ѻ����������� Boolean ���µ��
    [Header("Multiple Boolean Items")]
    [SerializeField] string[] multipleItemNames = { "nahh8", "oldPic" }; // ��ª�����������µ��
    [SerializeField] bool useORLogic = false; // true = �յ��㴵��˹��, false = ��ͧ�դú�ء���

    // ��õ�駤������Ѻ�к� Choose Item
    [Header("Choose Item System")]
    [SerializeField] int[] requiredChooseItems = { 2, 1, 3 }; // ��ҷ���ͧ�������Ѻ chooseItem01, 02, 03
    [SerializeField] bool checkChooseItems = false; // �Դ/�Դ����� choose items

    // ��õ�駤���Ϳ�硵� Fade
    [Header("Fade Settings")]
    [SerializeField] float fadeSpeed = 2f; // �������Ǣͧ�Ϳ�硵� fade
    [SerializeField] GameObject fadeCanvas; // Canvas ����Ѻ�Ϳ�硵� fade
    [SerializeField] UnityEngine.UI.Image fadeImage; // Image component ����Ѻ fade

    // Enums ����Ѻ��˹���ȷҧ��л���������������
    enum Direction { Up, Down, Left, Right, Teleport }
    enum RequiredItemType { SingleBoolean, MultipleBooleans, ChooseItems, BooleanAndChoose, AnyOfMultipleBooleans }

    // ����äǺ���ʶҹС������¹���
    private bool isTransitioning = false;
    private bool hasBeenUsed = false; // ��Ǩ�ͺ�������������������

    // �ѧ��ѹ������� - �� confiner ������ҧ fade canvas
    private void Awake()
    {
        confiner = FindObjectOfType<CinemachineConfiner2D>();

        // ���ҧ fade canvas ੾������͵�ͧ����� fade transition
        if (useFadeTransition && fadeCanvas == null)
        {
            CreateFadeCanvas();
        }
    }

    // ���ҧ Canvas ����Ѻ�Ϳ�硵� fade Ẻ dynamic
    private void CreateFadeCanvas()
    {
        // ���ҧ GameObject ����Ѻ Canvas
        GameObject canvasObj = new GameObject("FadeCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 1000; // ��������ҹ���ش�ͧ UI

        // ���� Components ����������Ѻ Canvas
        canvasObj.AddComponent<UnityEngine.UI.CanvasScaler>();
        canvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();

        // ���ҧ Image Object ����Ѻ�Ϳ�硵� fade
        GameObject imageObj = new GameObject("FadeImage");
        imageObj.transform.SetParent(canvasObj.transform, false);

        fadeImage = imageObj.AddComponent<UnityEngine.UI.Image>();
        fadeImage.color = new Color(0, 0, 0, 0); // �������������

        // ��駤�� RectTransform ����ͺ����˹�Ҩͷ�����
        RectTransform rectTransform = fadeImage.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.sizeDelta = Vector2.zero;
        rectTransform.anchoredPosition = Vector2.zero;

        fadeCanvas = canvasObj;
        fadeCanvas.SetActive(false); // ��͹ canvas ���͹�������
    }

    // ��Ǩ�Ѻ��ê��ͧ�����蹡Ѻ Trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ��Ǩ�ͺ����繼���������������������ҧ�������¹���
        if (collision.gameObject.CompareTag("Player") && !isTransitioning)
        {
            // ��Ǩ�ͺ��Ҷ���� one time use ������������ ������ӧҹ
            if (oneTimeUse && hasBeenUsed) return;

            // �����͹��������͹�ӡ������¹���
            if (requireItem && !CheckItemRequirements())
            {
                // �ҡ���������������� ������ӡ������¹���
                return;
            }

            // ����ͷӡ������¹������� �����������������
            if (oneTimeUse) hasBeenUsed = true;

            // ���͡�������������¹��������õ�駤��
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

    // �ѧ��ѹ��ѡ����Ѻ�����͹������
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
                return true; // �ҡ��������͹� ����ҹ�ء����
        }
    }

    // ������������� Boolean �����
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

    // ������������� Boolean ���µ�� (OR logic - �յ��㴵��˹�觡��)
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
                    return true; // ������������ true
                }
            }
            else
            {
                Debug.LogWarning($"��辺 boolean item: {itemName} � chooseSystem");
            }
        }

        return false; // ������������� true
    }

    // ������������� Boolean ���µ�� (AND logic - ��ͧ�դú�ء���)
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
                    return false; // ������������ false
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

    // ������������� Choose Items
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

    // �������¹���Ẻ���Ϳ�硵� fade
    private IEnumerator TransitionWithFade(GameObject player)
    {
        isTransitioning = true;

        // �Դ fade canvas
        fadeCanvas.SetActive(true);

        // Fade Out (�ҡ����繴�)
        yield return StartCoroutine(FadeToBlack());

        // �ѻവ���˹觼�������Тͺࢵ���ͧ
        confiner.BoundingShape2D = mapBoundry;
        UpdatePlayerPosition(player);

        // �������ͧ�Դ�����������ѧ���˹�����
        yield return new WaitForSeconds(0.1f);

        // Fade In (�ҡ�������)
        yield return StartCoroutine(FadeToTransparent());

        // ��͹ fade canvas
        fadeCanvas.SetActive(false);

        isTransitioning = false;
    }

    // �������¹���Ẻ�ѹ�� (������Ϳ�硵�)
    private IEnumerator InstantTransition(GameObject player)
    {
        isTransitioning = true;

        // �ѻവ���˹觼�������Тͺࢵ���ͧ�ѹ��
        confiner.BoundingShape2D = mapBoundry;
        UpdatePlayerPosition(player);

        // �� 1 frame �������������¹�ŧ�������
        yield return null;

        isTransitioning = false;
    }

    // �Ϳ�硵� Fade Out (�ҡ����繴�)
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

    // �Ϳ�硵� Fade In (�ҡ�������)
    private IEnumerator FadeToTransparent()
    {
        Color startColor = fadeImage.color;
        Color targetColor = new Color(0, 0, 0, 0); // ����

        float elapsed = 0f;

        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime * fadeSpeed;
            fadeImage.color = Color.Lerp(startColor, targetColor, elapsed);
            yield return null;
        }

        fadeImage.color = targetColor;
    }

    // �ѻവ���˹觼����蹵����ȷҧ����˹�
    private void UpdatePlayerPosition(GameObject player)
    {
        // �ҡ�� Teleport �������͹������ѧ���˹�������·���˹�
        if (direction == Direction.Teleport)
        {
            player.transform.position = teleportTargetPosition.position;
            return;
        }

        // ����͹���¼����蹵����ȷҧ����˹�
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

    // === Public Functions ����Ѻ�Ǻ����ҡ��¹͡ ===

    // ����¹��õ�駤�� fade transition ����ѹ���
    public void SetUseFadeTransition(bool useFade)
    {
        useFadeTransition = useFade;

        // ���ҧ fade canvas �����ҡ��ͧ��������ѧ�����
        if (useFadeTransition && fadeCanvas == null)
        {
            CreateFadeCanvas();
        }
    }

    // ��Ǩ�ͺʶҹлѨ�غѹ�ͧ fade transition
    public bool IsUsingFadeTransition()
    {
        return useFadeTransition;
    }

    // ����¹��õ�駤�ҡ���������
    public void SetRequireItem(bool requireItem)
    {
        this.requireItem = requireItem;
    }

    // ��Ǩ�ͺ��Ҽ�������������������������
    public bool HasRequiredItems()
    {
        return !requireItem || CheckItemRequirements();
    }
}