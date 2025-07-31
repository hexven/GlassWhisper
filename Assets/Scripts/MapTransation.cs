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
    [SerializeField] bool useFadeTransition = true; // เพิ่มตัวเลือกเปิด/ปิด fade

    [Header("Item Requirements")]
    [SerializeField] bool requireItem = false; // เปิด/ปิดการเช็คไอเทม
    [SerializeField] RequiredItemType requiredItemType = RequiredItemType.SingleBoolean; // ประเภทการเช็ค

    [Header("Single Boolean Item")]
    [SerializeField] string booleanItemName = "key"; // ชื่อ boolean variable ที่ต้องการ

    [Header("Multiple Boolean Items")]
    [SerializeField] string[] multipleItemNames = { "nahh8", "oldPic" }; // ไอเทมหลายตัว
    [SerializeField] bool useORLogic = false; // true = มีตัวใดตัวหนึ่ง, false = ต้องมีครบทุกตัว (AND)

    [Header("Choose Item System")]
    [SerializeField] int[] requiredChooseItems = { 2, 1, 3 }; // ค่าที่ต้องการสำหรับ chooseItem01, 02, 03
    [SerializeField] bool checkChooseItems = false; // เปิด/ปิดการเช็ค choose items

    [Header("Feedback Messages")]
    [SerializeField] bool showFeedbackMessage = true;
    [SerializeField] string noItemMessage = "คุณต้องมีไอเทมที่จำเป็นก่อน";
    [SerializeField] float messageDisplayTime = 2f;
    [SerializeField] GameObject messagePanel; // Panel สำหรับแสดงข้อความ
    [SerializeField] UnityEngine.UI.Text messageText; // Text component สำหรับแสดงข้อความ

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

        // สร้าง fade canvas เฉพาะเมื่อต้องการใช้ fade
        if (useFadeTransition && fadeCanvas == null)
        {
            CreateFadeCanvas();
        }

        // ซ่อน message panel ตอนเริ่มต้น
        if (messagePanel != null)
        {
            messagePanel.SetActive(false);
        }
    }

    private void CreateFadeCanvas()
    {
        // สร้าง Canvas
        GameObject canvasObj = new GameObject("FadeCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 1000; // ให้อยู่ด้านบนสุด

        // สร้าง CanvasScaler
        canvasObj.AddComponent<UnityEngine.UI.CanvasScaler>();
        canvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();

        // สร้าง Image สำหรับ fade
        GameObject imageObj = new GameObject("FadeImage");
        imageObj.transform.SetParent(canvasObj.transform, false);

        fadeImage = imageObj.AddComponent<UnityEngine.UI.Image>();
        fadeImage.color = new Color(0, 0, 0, 0); // เริ่มต้นเป็นใส

        // ตั้งค่าให้ครอบคลุมหน้าจอทั้งหมด
        RectTransform rectTransform = fadeImage.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.sizeDelta = Vector2.zero;
        rectTransform.anchoredPosition = Vector2.zero;

        fadeCanvas = canvasObj;

        // ซ่อน canvas ไว้ตอนเริ่มต้น
        fadeCanvas.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isTransitioning)
        {
            // เช็คไอเทมก่อนทำ transition
            if (requireItem && !CheckItemRequirements())
            {
                if (showFeedbackMessage)
                {
                    StartCoroutine(ShowMessage(noItemMessage));
                }
                return; // ไม่ทำ transition ถ้าไม่มีไอเทม
            }

            // เลือกใช้ transition แบบไหนตามการตั้งค่า
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

        // ใช้ Reflection เพื่อเข้าถึง static boolean จาก chooseSystem
        var field = typeof(chooseSystem).GetField(booleanItemName,
            System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

        if (field != null && field.FieldType == typeof(bool))
        {
            return (bool)field.GetValue(null);
        }

        Debug.LogWarning($"ไม่พบ boolean item: {booleanItemName} ใน chooseSystem");
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
                    return true; // ถ้ามีไอเทมใดไอเทมหนึ่งที่เป็น true ให้ return true
                }
            }
            else
            {
                Debug.LogWarning($"ไม่พบ boolean item: {itemName} ใน chooseSystem");
            }
        }

        return false; // ไม่มีไอเทมใดเป็น true
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
                    return false; // ถ้ามีไอเทมใดไอเทมหนึ่งที่เป็น false ให้ return false
                }
            }
            else
            {
                Debug.LogWarning($"ไม่พบ boolean item: {itemName} ใน chooseSystem");
                return false;
            }
        }

        return true; // ทุกไอเท็มเป็น true
    }

    private bool CheckChooseItems()
    {
        if (!checkChooseItems || requiredChooseItems == null)
            return true;

        // เช็ค chooseItem01
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

        // เช็ค chooseItem02
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

        // เช็ค chooseItem03
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

        // เปิด fade canvas
        fadeCanvas.SetActive(true);

        // Fade Out (จากใสไปเป็นดำ)
        yield return StartCoroutine(FadeToBlack());

        // อัปเดตตำแหน่งผู้เล่นและ camera bounds
        confiner.BoundingShape2D = mapBoundry;
        UpdatePlayerPosition(player);

        // รอให้ camera ติดตามผู้เล่นไปยังตำแหน่งใหม่
        yield return new WaitForSeconds(0.1f);

        // Fade In (จากดำไปเป็นใส)
        yield return StartCoroutine(FadeToTransparent());

        // ซ่อน fade canvas
        fadeCanvas.SetActive(false);

        isTransitioning = false;
    }

    private IEnumerator InstantTransition(GameObject player)
    {
        isTransitioning = true;

        // อัปเดตตำแหน่งผู้เล่นและ camera bounds ทันที
        confiner.BoundingShape2D = mapBoundry;
        UpdatePlayerPosition(player);

        // รอ frame เดียวเพื่อให้ transition เสร็จสิ้น
        yield return null;

        isTransitioning = false;
    }

    private IEnumerator FadeToBlack()
    {
        Color startColor = fadeImage.color;
        Color targetColor = new Color(0, 0, 0, 1); // สีดำทึบ

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
        Color targetColor = new Color(0, 0, 0, 0); // ใส

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

    // ฟังชันสำหรับเปลี่ยนการตั้งค่า fade ขณะรันไทม์
    public void SetUseFadeTransition(bool useFade)
    {
        useFadeTransition = useFade;

        // ถ้าเปิดใช้ fade และยังไม่มี canvas ให้สร้างใหม่
        if (useFadeTransition && fadeCanvas == null)
        {
            CreateFadeCanvas();
        }
    }

    // ฟังชันสำหรับเช็คสถานะปัจจุบัน
    public bool IsUsingFadeTransition()
    {
        return useFadeTransition;
    }

    // ฟังชันสำหรับเปลี่ยนการตั้งค่าไอเทม
    public void SetRequireItem(bool requireItem)
    {
        this.requireItem = requireItem;
    }

    // ฟังชันสำหรับเช็คว่าผู้เล่นมีไอเทมหรือไม่
    public bool HasRequiredItems()
    {
        return !requireItem || CheckItemRequirements();
    }
}