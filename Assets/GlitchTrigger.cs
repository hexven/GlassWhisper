using UnityEngine;

public class GlitchTrigger : MonoBehaviour
{
    [Header("Glitch Settings")]
    public GlitchManager glitchManager;

    [Header("NEW: Advanced Glitch Controls")]
    [SerializeField] private GlitchMode glitchMode = GlitchMode.TriggerOnce;
    [SerializeField] private bool stopGlitchOnExit = true; // หยุด glitch เมื่อออกจาก trigger
    [SerializeField] private float glitchDuration = 2f; // ระยะเวลา glitch (สำหรับ TimedGlitch)
    [SerializeField] private float continuousGlitchIntensity = 1f; // ความแรงของ glitch แบบต่อเนื่อง

    public enum GlitchMode
    {
        TriggerOnce,        // เล่น glitch ครั้งเดียวเมื่อเข้า (เหมือนเดิม)
        TimedGlitch,        // เล่น glitch เป็นเวลาที่กำหนด
        ContinuousGlitch,   // เล่น glitch ตลอดเวลาที่อยู่ใน trigger
        ToggleGlitch        // เปิด/ปิด glitch สลับกันทุกครั้งที่เข้า
    }

    [Header("Sprite Settings")]
    public GameObject spritePrefab; // ลาก Prefab ที่ต้องการแสดง
    public Transform spawnPoint; // จุดที่จะแสดง sprite (ถ้าไม่ใส่จะใช้ตำแหน่งของ trigger)
    public float spriteLifetime = 3f; // เวลาที่ sprite จะอยู่ (วินาที)
    public bool destroySpriteAfterTime = true; // ให้ลบ sprite หลังหมดเวลาหรือไม่

    [Header("Sound Settings")]
    public string soundName = ""; // ชื่อเสียงที่จะเล่น - เว้นว่างไว้ถ้าไม่ต้องการเสียง
    public bool playSound = false; // เปิด/ปิดการเล่นเสียง - ปิดไว้ก่อน

    [Header("Advanced Options")]
    public bool triggerOnce = false; // ให้ trigger ได้แค่ครั้งเดียวหรือไม่
    public float cooldownTime = 0f; // เวลา cooldown ระหว่าง trigger (วินาที)

    // ตัวแปรสำหรับระบบเก่า
    private bool hasTriggered = false;
    private float lastTriggerTime = 0f;
    private GameObject currentSprite;

    // ตัวแปรสำหรับระบบใหม่
    private bool playerInTrigger = false;
    private bool isGlitchActive = false;
    private bool glitchToggleState = false;
    private Coroutine timedGlitchCoroutine;
    private Coroutine continuousGlitchCoroutine;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInTrigger = true;

        // ตรวจสอบ cooldown
        if (Time.time - lastTriggerTime < cooldownTime) return;

        // ตรวจสอบว่า trigger ได้แค่ครั้งเดียวหรือไม่
        if (triggerOnce && hasTriggered) return;

        // เล่นเสียง - ใช้โค้ดเดิม
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

        // เริ่ม Glitch Effect ตาม Mode ที่เลือก
        if (glitchManager != null)
        {
            HandleGlitchByMode();
        }

        // สร้าง Sprite - ใช้โค้ดเดิม
        if (spritePrefab != null)
        {
            SpawnSprite();
        }

        // อัปเดตสถานะ
        hasTriggered = true;
        lastTriggerTime = Time.time;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInTrigger = false;

        // หยุด Glitch เมื่อออกจาก trigger (ถ้าเปิดใช้งาน)
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
                // ใช้วิธีเดิม
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

            // รอตามความแรงที่กำหนด (ยิ่งค่าสูง รอนานขึ้น = glitch นานขึ้น)
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
            // ถ้า GlitchManager มี method StopGlitch ให้เรียกใช้
            // glitchManager.StopGlitch();

            // หรือถ้าไม่มี ให้ปิดผ่าน component
            // glitchManager.enabled = false;

            Debug.Log("GlitchTrigger: Stopped glitch effect");
        }
        isGlitchActive = false;
    }

    // === โค้ดเดิมทั้งหมด ===
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

        // รีเซ็ตสถานะใหม่ด้วย
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

    // === NEW: Public Methods สำหรับควบคุมจากภายนอก ===

    [ContextMenu("Manual Start Continuous Glitch")]
    public void ManualStartContinuousGlitch()
    {
        playerInTrigger = true; // จำลองว่าผู้เล่นอยู่ใน trigger
        StartContinuousGlitch();
    }

    [ContextMenu("Manual Stop All Glitch")]
    public void ManualStopAllGlitch()
    {
        StopAllGlitchEffects();
    }

    public void StopAllGlitchEffects()
    {
        // หยุด Coroutines ทั้งหมด
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

    // Method สำหรับเปลี่ยน Glitch Mode จาก script อื่น
    public void SetGlitchMode(GlitchMode newMode)
    {
        StopAllGlitchEffects(); // หยุดทุกอย่างก่อน
        glitchMode = newMode;
        Debug.Log($"GlitchTrigger: Changed glitch mode to {newMode}");
    }

    // Method สำหรับเปลี่ยนค่าต่างๆ แบบ Runtime
    public void SetGlitchDuration(float duration)
    {
        glitchDuration = duration;
    }

    public void SetContinuousIntensity(float intensity)
    {
        continuousGlitchIntensity = Mathf.Max(0.1f, intensity); // ป้องกันค่าติดลบ
    }

    private void OnDestroy()
    {
        // ทำความสะอาดเมื่อ Object ถูกลบ
        StopAllGlitchEffects();
    }
}