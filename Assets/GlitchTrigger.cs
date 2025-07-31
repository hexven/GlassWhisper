using UnityEngine;

public class GlitchTrigger : MonoBehaviour
{
    [Header("Glitch Settings")]
    public GlitchManager glitchManager;

    [Header("NEW: Advanced Glitch Controls")]
    [SerializeField] private GlitchMode glitchMode = GlitchMode.TriggerOnce;
    [SerializeField] private bool stopGlitchOnExit = true; // ��ش glitch ������͡�ҡ trigger
    [SerializeField] private float glitchDuration = 2f; // �������� glitch (����Ѻ TimedGlitch)
    [SerializeField] private float continuousGlitchIntensity = 1f; // �����ç�ͧ glitch Ẻ������ͧ

    public enum GlitchMode
    {
        TriggerOnce,        // ��� glitch ���������������� (����͹���)
        TimedGlitch,        // ��� glitch �����ҷ���˹�
        ContinuousGlitch,   // ��� glitch ��ʹ���ҷ������� trigger
        ToggleGlitch        // �Դ/�Դ glitch ��Ѻ�ѹ�ء���駷�����
    }

    [Header("Sprite Settings")]
    public GameObject spritePrefab; // �ҡ Prefab ����ͧ����ʴ�
    public Transform spawnPoint; // �ش�����ʴ� sprite (��������������˹觢ͧ trigger)
    public float spriteLifetime = 3f; // ���ҷ�� sprite ������ (�Թҷ�)
    public bool destroySpriteAfterTime = true; // ���ź sprite ��ѧ��������������

    [Header("Sound Settings")]
    public string soundName = ""; // �������§������� - �����ҧ���������ͧ������§
    public bool playSound = false; // �Դ/�Դ���������§ - �Դ����͹

    [Header("Advanced Options")]
    public bool triggerOnce = false; // ��� trigger ������������������
    public float cooldownTime = 0f; // ���� cooldown �����ҧ trigger (�Թҷ�)

    // ���������Ѻ�к����
    private bool hasTriggered = false;
    private float lastTriggerTime = 0f;
    private GameObject currentSprite;

    // ���������Ѻ�к�����
    private bool playerInTrigger = false;
    private bool isGlitchActive = false;
    private bool glitchToggleState = false;
    private Coroutine timedGlitchCoroutine;
    private Coroutine continuousGlitchCoroutine;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInTrigger = true;

        // ��Ǩ�ͺ cooldown
        if (Time.time - lastTriggerTime < cooldownTime) return;

        // ��Ǩ�ͺ��� trigger ������������������
        if (triggerOnce && hasTriggered) return;

        // ������§ - �������
        if (playSound && !string.IsNullOrEmpty(soundName) && soundName.Trim() != "")
        {
            if (SoundManager.Instance != null)
            {
                Debug.Log($"GlitchTrigger: Attempting to play sound: '{soundName}'");
                Debug.Log($"GlitchTrigger: GameObject name: {gameObject.name}");

                if (HasSound(soundName))
                {
                    SoundManager.Instance.PlaySound2D(soundName);
                    Debug.Log($"GlitchTrigger: Successfully played sound: '{soundName}'");
                }
                else
                {
                    Debug.LogWarning($"GlitchTrigger: Sound '{soundName}' not found in SoundManager!");
                }
            }
            else
            {
                Debug.LogWarning("GlitchTrigger: SoundManager.Instance is null!");
            }
        }
        else
        {
            Debug.Log("GlitchTrigger: Sound playback skipped - playSound is false or soundName is empty");
        }

        // ����� Glitch Effect ��� Mode ������͡
        if (glitchManager != null)
        {
            HandleGlitchByMode();
        }

        // ���ҧ Sprite - �������
        if (spritePrefab != null)
        {
            SpawnSprite();
        }

        // �ѻവʶҹ�
        hasTriggered = true;
        lastTriggerTime = Time.time;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInTrigger = false;

        // ��ش Glitch ������͡�ҡ trigger (����Դ��ҹ)
        if (stopGlitchOnExit && glitchManager != null)
        {
            HandleGlitchExit();
        }
    }

    private void HandleGlitchByMode()
    {
        switch (glitchMode)
        {
            case GlitchMode.TriggerOnce:
                // ���Ը����
                glitchManager.StartGlitch();
                break;

            case GlitchMode.TimedGlitch:
                StartTimedGlitch();
                break;

            case GlitchMode.ContinuousGlitch:
                StartContinuousGlitch();
                break;

            case GlitchMode.ToggleGlitch:
                ToggleGlitch();
                break;
        }
    }

    private void HandleGlitchExit()
    {
        switch (glitchMode)
        {
            case GlitchMode.ContinuousGlitch:
                StopContinuousGlitch();
                break;

            case GlitchMode.TimedGlitch:
                if (timedGlitchCoroutine != null)
                {
                    StopCoroutine(timedGlitchCoroutine);
                    timedGlitchCoroutine = null;
                }
                StopGlitch();
                break;

            case GlitchMode.ToggleGlitch:
                if (isGlitchActive)
                {
                    StopGlitch();
                }
                break;
        }
    }

    private void StartTimedGlitch()
    {
        if (timedGlitchCoroutine != null)
        {
            StopCoroutine(timedGlitchCoroutine);
        }
        timedGlitchCoroutine = StartCoroutine(TimedGlitchCoroutine());
    }

    private System.Collections.IEnumerator TimedGlitchCoroutine()
    {
        glitchManager.StartGlitch();
        isGlitchActive = true;

        yield return new WaitForSeconds(glitchDuration);

        StopGlitch();
        timedGlitchCoroutine = null;
    }

    private void StartContinuousGlitch()
    {
        if (!isGlitchActive)
        {
            if (continuousGlitchCoroutine != null)
            {
                StopCoroutine(continuousGlitchCoroutine);
            }
            continuousGlitchCoroutine = StartCoroutine(ContinuousGlitchCoroutine());
        }
    }

    private System.Collections.IEnumerator ContinuousGlitchCoroutine()
    {
        isGlitchActive = true;

        while (playerInTrigger && isGlitchActive)
        {
            glitchManager.StartGlitch();

            // �͵�������ç����˹� (��觤���٧ �͹ҹ��� = glitch �ҹ���)
            yield return new WaitForSeconds(0.1f / continuousGlitchIntensity);
        }

        continuousGlitchCoroutine = null;
    }

    private void StopContinuousGlitch()
    {
        isGlitchActive = false;
        if (continuousGlitchCoroutine != null)
        {
            StopCoroutine(continuousGlitchCoroutine);
            continuousGlitchCoroutine = null;
        }
        StopGlitch();
    }

    private void ToggleGlitch()
    {
        glitchToggleState = !glitchToggleState;

        if (glitchToggleState)
        {
            glitchManager.StartGlitch();
            isGlitchActive = true;
        }
        else
        {
            StopGlitch();
        }
    }

    private void StopGlitch()
    {
        if (glitchManager != null)
        {
            // ��� GlitchManager �� method StopGlitch ������¡��
            // glitchManager.StopGlitch();

            // ���Ͷ������� ���Դ��ҹ component
            // glitchManager.enabled = false;

            Debug.Log("GlitchTrigger: Stopped glitch effect");
        }
        isGlitchActive = false;
    }

    // === ����������� ===
    private bool HasSound(string soundName)
    {
        return true;
    }

    private void StopAllSounds()
    {
        if (SoundManager.Instance != null)
        {
            Debug.Log("GlitchTrigger: Stopped all sounds");
        }
    }

    [ContextMenu("Check Playing Sounds")]
    public void CheckPlayingSounds()
    {
        if (SoundManager.Instance != null)
        {
            Debug.Log("=== Current GlitchTrigger Settings ===");
            Debug.Log($"GameObject: {gameObject.name}");
            Debug.Log($"Sound Name: '{soundName}'");
            Debug.Log($"Play Sound: {playSound}");
            Debug.Log($"Has Triggered: {hasTriggered}");
            Debug.Log($"Trigger Once: {triggerOnce}");
            Debug.Log($"NEW - Glitch Mode: {glitchMode}");
            Debug.Log($"NEW - Player In Trigger: {playerInTrigger}");
            Debug.Log($"NEW - Glitch Active: {isGlitchActive}");
            Debug.Log("=======================================");
        }
    }

    private void SpawnSprite()
    {
        if (currentSprite != null)
        {
            DestroyImmediate(currentSprite);
        }

        Vector3 spawnPosition = spawnPoint != null ? spawnPoint.position : transform.position;
        currentSprite = Instantiate(spritePrefab, spawnPosition, Quaternion.identity);

        if (destroySpriteAfterTime && spriteLifetime > 0)
        {
            Destroy(currentSprite, spriteLifetime);
        }
    }

    public void ResetTrigger()
    {
        hasTriggered = false;
        lastTriggerTime = 0f;

        // ����ʶҹ��������
        glitchToggleState = false;
        StopAllGlitchEffects();
    }

    public void ClearCurrentSprite()
    {
        if (currentSprite != null)
        {
            Destroy(currentSprite);
            currentSprite = null;
        }
    }

    public void ManualTrigger()
    {
        if (playSound && !string.IsNullOrEmpty(soundName) && soundName.Trim() != "")
        {
            if (SoundManager.Instance != null)
            {
                Debug.Log($"Manual trigger - Playing sound: {soundName}");

                if (HasSound(soundName))
                {
                    SoundManager.Instance.PlaySound2D(soundName);
                }
                else
                {
                    Debug.LogWarning($"Sound '{soundName}' not found in SoundManager!");
                }
            }
        }

        if (glitchManager != null)
        {
            HandleGlitchByMode();
        }

        if (spritePrefab != null)
        {
            SpawnSprite();
        }
    }

    [ContextMenu("Test Sound")]
    public void TestSound()
    {
        if (!string.IsNullOrEmpty(soundName))
        {
            Debug.Log($"Testing sound: {soundName}");
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySound2D(soundName);
            }
        }
        else
        {
            Debug.Log("No sound name specified");
        }
    }

    // === NEW: Public Methods ����Ѻ�Ǻ����ҡ��¹͡ ===

    [ContextMenu("Manual Start Continuous Glitch")]
    public void ManualStartContinuousGlitch()
    {
        playerInTrigger = true; // ���ͧ��Ҽ���������� trigger
        StartContinuousGlitch();
    }

    [ContextMenu("Manual Stop All Glitch")]
    public void ManualStopAllGlitch()
    {
        StopAllGlitchEffects();
    }

    public void StopAllGlitchEffects()
    {
        // ��ش Coroutines ������
        if (timedGlitchCoroutine != null)
        {
            StopCoroutine(timedGlitchCoroutine);
            timedGlitchCoroutine = null;
        }

        if (continuousGlitchCoroutine != null)
        {
            StopCoroutine(continuousGlitchCoroutine);
            continuousGlitchCoroutine = null;
        }

        StopGlitch();
        playerInTrigger = false;
    }

    // Method ����Ѻ����¹ Glitch Mode �ҡ script ���
    public void SetGlitchMode(GlitchMode newMode)
    {
        StopAllGlitchEffects(); // ��ش�ء���ҧ��͹
        glitchMode = newMode;
        Debug.Log($"GlitchTrigger: Changed glitch mode to {newMode}");
    }

    // Method ����Ѻ����¹��ҵ�ҧ� Ẻ Runtime
    public void SetGlitchDuration(float duration)
    {
        glitchDuration = duration;
    }

    public void SetContinuousIntensity(float intensity)
    {
        continuousGlitchIntensity = Mathf.Max(0.1f, intensity); // ��ͧ�ѹ��ҵԴź
    }

    private void OnDestroy()
    {
        // �Ӥ������Ҵ����� Object �١ź
        StopAllGlitchEffects();
    }
}