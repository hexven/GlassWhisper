using Unity.Cinemachine;
using UnityEditor.Experimental;
using UnityEngine;
using System.Collections;

public class MapTransation : MonoBehaviour
{
    // การตั้งค่าพื้นฐานสำหรับ Map Transition
    [SerializeField] PolygonCollider2D mapBoundry; // ขอบเขตของแมพใหม่
    CinemachineConfiner2D confiner; // ตัวควบคุมขอบเขตกล้อง
    [SerializeField] Direction direction; // ทิศทางการเคลื่อนย้ายผู้เล่น
    [SerializeField] Transform teleportTargetPosition; // ตำแหน่งเป้าหมายสำหรับ Teleport

    // การตั้งค่าเอฟเฟ็กต์การเปลี่ยนแมพ
    [Header("Transition Settings")]
    [SerializeField] bool useFadeTransition = true; // เปิด/ปิดเอฟเฟ็กต์ fade
    [SerializeField] bool oneTimeUse = false; // ใช้ได้แค่ครั้งเดียว

    // การตั้งค่าเงื่อนไขไอเท็มที่ต้องการ
    [Header("Item Requirements")]
    [SerializeField] bool requireItem = false; // เปิด/ปิดการเช็คไอเท็ม
    [SerializeField] RequiredItemType requiredItemType = RequiredItemType.SingleBoolean; // ประเภทการเช็คไอเท็ม

    // การตั้งค่าสำหรับไอเท็มประเภท Boolean เดี่ยว
    [Header("Single Boolean Item")]
    [SerializeField] string booleanItemName = "key"; // ชื่อตัวแปร boolean ที่ต้องการ

    // การตั้งค่าสำหรับไอเท็มประเภท Boolean หลายตัว
    [Header("Multiple Boolean Items")]
    [SerializeField] string[] multipleItemNames = { "nahh8", "oldPic" }; // รายชื่อไอเท็มหลายตัว
    [SerializeField] bool useORLogic = false; // true = มีตัวใดตัวหนึ่ง, false = ต้องมีครบทุกตัว

    // การตั้งค่าสำหรับระบบ Choose Item
    [Header("Choose Item System")]
    [SerializeField] int[] requiredChooseItems = { 2, 1, 3 }; // ค่าที่ต้องการสำหรับ chooseItem01, 02, 03
    [SerializeField] bool checkChooseItems = false; // เปิด/ปิดการเช็ค choose items

    // การตั้งค่าเอฟเฟ็กต์ Fade
    [Header("Fade Settings")]
    [SerializeField] float fadeSpeed = 2f; // ความเร็วของเอฟเฟ็กต์ fade
    [SerializeField] GameObject fadeCanvas; // Canvas สำหรับเอฟเฟ็กต์ fade
    [SerializeField] UnityEngine.UI.Image fadeImage; // Image component สำหรับ fade

    // Enums สำหรับกำหนดทิศทางและประเภทการเช็คไอเท็ม
    enum Direction { Up, Down, Left, Right, Teleport }
    enum RequiredItemType { SingleBoolean, MultipleBooleans, ChooseItems, BooleanAndChoose, AnyOfMultipleBooleans }

    // ตัวแปรควบคุมสถานะการเปลี่ยนแมพ
    private bool isTransitioning = false;
    private bool hasBeenUsed = false; // ตรวจสอบว่าได้ใช้ไปแล้วหรือไม่

    // ฟังก์ชันเริ่มต้น - หา confiner และสร้าง fade canvas
    private void Awake()
    {
        confiner = FindObjectOfType<CinemachineConfiner2D>();

        // สร้าง fade canvas เฉพาะเมื่อต้องการใช้ fade transition
        if (useFadeTransition && fadeCanvas == null)
        {
            CreateFadeCanvas();
        }
    }

    // สร้าง Canvas สำหรับเอฟเฟ็กต์ fade แบบ dynamic
    private void CreateFadeCanvas()
    {
        // สร้าง GameObject สำหรับ Canvas
        GameObject canvasObj = new GameObject("FadeCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 1000; // ให้อยู่ด้านบนสุดของ UI

        // เพิ่ม Components ที่จำเป็นสำหรับ Canvas
        canvasObj.AddComponent<UnityEngine.UI.CanvasScaler>();
        canvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();

        // สร้าง Image Object สำหรับเอฟเฟ็กต์ fade
        GameObject imageObj = new GameObject("FadeImage");
        imageObj.transform.SetParent(canvasObj.transform, false);

        fadeImage = imageObj.AddComponent<UnityEngine.UI.Image>();
        fadeImage.color = new Color(0, 0, 0, 0); // เริ่มต้นเป็นสีใส

        // ตั้งค่า RectTransform ให้ครอบคลุมหน้าจอทั้งหมด
        RectTransform rectTransform = fadeImage.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.sizeDelta = Vector2.zero;
        rectTransform.anchoredPosition = Vector2.zero;

        fadeCanvas = canvasObj;
        fadeCanvas.SetActive(false); // ซ่อน canvas ไว้ตอนเริ่มต้น
    }

    // ตรวจจับการชนของผู้เล่นกับ Trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ตรวจสอบว่าเป็นผู้เล่นและไม่อยู่ในระหว่างการเปลี่ยนแมพ
        if (collision.gameObject.CompareTag("Player") && !isTransitioning)
        {
            // ตรวจสอบว่าถ้าเป็น one time use และได้ใช้ไปแล้ว ให้ไม่ทำงาน
            if (oneTimeUse && hasBeenUsed) return;

            // เช็คเงื่อนไขไอเท็มก่อนทำการเปลี่ยนแมพ
            if (requireItem && !CheckItemRequirements())
            {
                // หากไม่มีไอเท็มที่จำเป็น ให้ไม่ทำการเปลี่ยนแมพ
                return;
            }

            // เมื่อทำการเปลี่ยนแมพแล้ว ให้เซ็ตว่าได้ใช้ไปแล้ว
            if (oneTimeUse) hasBeenUsed = true;

            // เลือกประเภทการเปลี่ยนแมพตามการตั้งค่า
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

    // ฟังก์ชันหลักสำหรับเช็คเงื่อนไขไอเท็ม
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
                return true; // หากไม่มีเงื่อนไข ให้ผ่านทุกครั้ง
        }
    }

    // เช็คไอเท็มประเภท Boolean เดี่ยว
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

    // เช็คไอเท็มประเภท Boolean หลายตัว (OR logic - มีตัวใดตัวหนึ่งก็พอ)
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
                    return true; // พบไอเท็มที่เป็น true
                }
            }
            else
            {
                Debug.LogWarning($"ไม่พบ boolean item: {itemName} ใน chooseSystem");
            }
        }

        return false; // ไม่มีไอเท็มใดเป็น true
    }

    // เช็คไอเท็มประเภท Boolean หลายตัว (AND logic - ต้องมีครบทุกตัว)
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
                    return false; // พบไอเท็มที่เป็น false
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

    // เช็คไอเท็มประเภท Choose Items
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

    // การเปลี่ยนแมพแบบมีเอฟเฟ็กต์ fade
    private IEnumerator TransitionWithFade(GameObject player)
    {
        isTransitioning = true;

        // เปิด fade canvas
        fadeCanvas.SetActive(true);

        // Fade Out (จากใสไปเป็นดำ)
        yield return StartCoroutine(FadeToBlack());

        // อัปเดตตำแหน่งผู้เล่นและขอบเขตกล้อง
        confiner.BoundingShape2D = mapBoundry;
        UpdatePlayerPosition(player);

        // รอให้กล้องติดตามผู้เล่นไปยังตำแหน่งใหม่
        yield return new WaitForSeconds(0.1f);

        // Fade In (จากดำไปเป็นใส)
        yield return StartCoroutine(FadeToTransparent());

        // ซ่อน fade canvas
        fadeCanvas.SetActive(false);

        isTransitioning = false;
    }

    // การเปลี่ยนแมพแบบทันที (ไม่มีเอฟเฟ็กต์)
    private IEnumerator InstantTransition(GameObject player)
    {
        isTransitioning = true;

        // อัปเดตตำแหน่งผู้เล่นและขอบเขตกล้องทันที
        confiner.BoundingShape2D = mapBoundry;
        UpdatePlayerPosition(player);

        // รอ 1 frame เพื่อให้การเปลี่ยนแปลงเสร็จสิ้น
        yield return null;

        isTransitioning = false;
    }

    // เอฟเฟ็กต์ Fade Out (จากใสไปเป็นดำ)
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

    // เอฟเฟ็กต์ Fade In (จากดำไปเป็นใส)
    private IEnumerator FadeToTransparent()
    {
        Color startColor = fadeImage.color;
        Color targetColor = new Color(0, 0, 0, 0); // สีใส

        float elapsed = 0f;

        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime * fadeSpeed;
            fadeImage.color = Color.Lerp(startColor, targetColor, elapsed);
            yield return null;
        }

        fadeImage.color = targetColor;
    }

    // อัปเดตตำแหน่งผู้เล่นตามทิศทางที่กำหนด
    private void UpdatePlayerPosition(GameObject player)
    {
        // หากเป็น Teleport ให้เคลื่อนย้ายไปยังตำแหน่งเป้าหมายที่กำหนด
        if (direction == Direction.Teleport)
        {
            player.transform.position = teleportTargetPosition.position;
            return;
        }

        // เคลื่อนย้ายผู้เล่นตามทิศทางที่กำหนด
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

    // === Public Functions สำหรับควบคุมจากภายนอก ===

    // เปลี่ยนการตั้งค่า fade transition ขณะรันไทม์
    public void SetUseFadeTransition(bool useFade)
    {
        useFadeTransition = useFade;

        // สร้าง fade canvas ใหม่หากต้องการใช้แต่ยังไม่มี
        if (useFadeTransition && fadeCanvas == null)
        {
            CreateFadeCanvas();
        }
    }

    // ตรวจสอบสถานะปัจจุบันของ fade transition
    public bool IsUsingFadeTransition()
    {
        return useFadeTransition;
    }

    // เปลี่ยนการตั้งค่าการเช็คไอเท็ม
    public void SetRequireItem(bool requireItem)
    {
        this.requireItem = requireItem;
    }

    // ตรวจสอบว่าผู้เล่นมีไอเท็มที่จำเป็นหรือไม่
    public bool HasRequiredItems()
    {
        return !requireItem || CheckItemRequirements();
    }
}