using UnityEngine;
using System.Collections;

public class SpriteBlink : MonoBehaviour
{
    [Header("Blink Settings")]
    public float blinkInterval = 1.0f; // ��ǧ���ҡ�á�о�Ժ (�Թҷ�)
    public float fadeDuration = 0.3f;  // ��������㹡�� fade in/out
    public bool startBlinkingOnStart = true;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool isBlinking = false;
    private Coroutine blinkCoroutine;
    [Header("Glitch Manager")]
    public GlitchManager glitchManager; // ���ͪ��� class ���١��ͧ

    void Start()
    {
        // �֧ SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteBlink: ��辺 SpriteRenderer component �� GameObject ���!");
            return;
        }

        // ����������
        originalColor = spriteRenderer.color;

        // �������о�Ժ��ҵ�駤�����
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

            // ��ش glitch �������ش��á�о�Ժ
            glitchManager.StopGlitch(); // ��������� method ���١��ͧ

            // �׹��������
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
            glitchManager.StartGlitch();// ��������� method ���١��ͧ

            // ���ѡ����
            yield return new WaitForSeconds(0.1f);

            // Fade In (�����������)
            yield return StartCoroutine(FadeToAlpha(originalColor.a));

            // �ͪ�ǧ���ҷ���˹�
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

        // ��駤���ش����������
        currentColor.a = targetAlpha;
        spriteRenderer.color = currentColor;
    }

    // ����Ѻ���¡��ҡ Inspector ���� script ���
    public void ToggleBlinking()
    {
        if (isBlinking)
            StopBlinking();
        else
            StartBlinking();
    }

    // �Ը�����¹�ٻẺ��á�о�Ժ
    public void SetBlinkSettings(float interval, float duration)
    {
        blinkInterval = interval;
        fadeDuration = duration;
    }

    // �Ը�����¹�բ�С�о�Ժ
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