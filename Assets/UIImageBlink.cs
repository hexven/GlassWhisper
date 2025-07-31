using UnityEngine;
using System.Collections;

public class SpriteBlink : MonoBehaviour
{
    [Header("Blink Settings")]
    public float blinkInterval = 1.0f; // ช่วงเวลาการกระพริบ (วินาที)
    public float fadeDuration = 0.3f;  // ระยะเวลาในการ fade in/out
    public bool startBlinkingOnStart = true;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool isBlinking = false;
    private Coroutine blinkCoroutine;
    [Header("Glitch Manager")]
    public GlitchManager glitchManager; // หรือชื่อ class ที่ถูกต้อง

    void Start()
    {
        // ดึง SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteBlink: ไม่พบ SpriteRenderer component บน GameObject นี้!");
            return;
        }

        // เก็บสีเดิมไว้
        originalColor = spriteRenderer.color;

        // เริ่มกระพริบถ้าตั้งค่าไว้
        if (startBlinkingOnStart)
        {
            StartBlinking();
        }
    }

    public void StartBlinking()
    {
        if (!isBlinking && spriteRenderer != null)
        {
            isBlinking = true;
            blinkCoroutine = StartCoroutine(BlinkCoroutine());
        }
    }

    public void StopBlinking()
    {
        if (isBlinking)
        {
            isBlinking = false;

            if (blinkCoroutine != null)
            {
                StopCoroutine(blinkCoroutine);
                blinkCoroutine = null;
            }

            // หยุด glitch เมื่อหยุดการกระพริบ
            glitchManager.StopGlitch(); // หรือใช้ชื่อ method ที่ถูกต้อง

            // คืนค่าสีเดิม
            if (spriteRenderer != null)
            {
                spriteRenderer.color = originalColor;
            }
        }
    }

    private IEnumerator BlinkCoroutine()
    {
        while (isBlinking)
        {
            
            glitchManager.StopGlitch();

            
            yield return StartCoroutine(FadeToAlpha(0f));


            SoundManager.Instance.PlaySound2D("TVStaticEffect");
            glitchManager.StartGlitch();// หรือใช้ชื่อ method ที่ถูกต้อง

            // รอสักครู่
            yield return new WaitForSeconds(0.1f);

            // Fade In (ทำให้เข้มขึ้น)
            yield return StartCoroutine(FadeToAlpha(originalColor.a));

            // รอช่วงเวลาที่กำหนด
            yield return new WaitForSeconds(blinkInterval);
        }
    }

    private IEnumerator FadeToAlpha(float targetAlpha)
    {
        if (spriteRenderer == null) yield break;

        Color currentColor = spriteRenderer.color;
        float startAlpha = currentColor.a;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);

            currentColor.a = alpha;
            spriteRenderer.color = currentColor;

            yield return null;
        }

        // ตั้งค่าสุดท้ายให้แน่ใจ
        currentColor.a = targetAlpha;
        spriteRenderer.color = currentColor;
    }

    // สำหรับเรียกใช้จาก Inspector หรือ script อื่น
    public void ToggleBlinking()
    {
        if (isBlinking)
            StopBlinking();
        else
            StartBlinking();
    }

    // วิธีเปลี่ยนรูปแบบการกระพริบ
    public void SetBlinkSettings(float interval, float duration)
    {
        blinkInterval = interval;
        fadeDuration = duration;
    }

    // วิธีเปลี่ยนสีขณะกระพริบ
    public void SetBlinkColor(Color newColor)
    {
        originalColor = newColor;
        if (spriteRenderer != null && !isBlinking)
        {
            spriteRenderer.color = originalColor;
        }
    }

    void OnDestroy()
    {
        StopBlinking();
    }
}