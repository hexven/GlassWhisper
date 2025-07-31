using System.Collections;
using UnityEngine;

public class BounceEffect : MonoBehaviour
{
    public float bounceHeight = 0.3f;
    public float bounceDuration = 0.4f;
    public int bounceCount = 2;

    private Transform playerTransform;

    void Start()
    {
        // หาผู้เล่น (ปรับ tag ตามที่คุณใช้)
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerTransform = player.transform;
    }

    public void StartBounce()
    {
        StartCoroutine(BounceHandler());
    }

    private IEnumerator BounceHandler()
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = playerTransform ? playerTransform.position : startPosition;

        float currentHeight = bounceHeight;
        float currentDuration = bounceDuration;

        for (int i = 0; i < bounceCount; i++)
        {
            // เด้งขึ้นและลง แต่ไปหาผู้เล่น
            yield return Bounce(startPosition, targetPosition, currentHeight, currentDuration);

            // อัพเดทตำแหน่งเริ่มต้นใหม่
            startPosition = transform.position;

            // ลดความสูงและเวลา
            currentHeight *= 0.7f;
            currentDuration *= 0.8f;
        }

        // ไปตำแหน่งผู้เล่น
        if (playerTransform)
            transform.position = targetPosition;
    }

    private IEnumerator Bounce(Vector3 start, Vector3 target, float height, float duration)
    {
        Vector3 midPoint = Vector3.Lerp(start, target, 0.5f);
        Vector3 peak = midPoint + Vector3.up * height;

        float halfDuration = duration / 2f;
        float elapsed = 0f;

        // เด้งขึ้น
        while (elapsed < halfDuration)
        {
            float t = elapsed / halfDuration;
            float easedT = 1f - (1f - t) * (1f - t);
            transform.position = Vector3.Lerp(start, peak, easedT);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // เด้งลง มุ่งหน้าไปหาผู้เล่น
        elapsed = 0f;
        while (elapsed < halfDuration)
        {
            float t = elapsed / halfDuration;
            float easedT = t * t;
            transform.position = Vector3.Lerp(peak, target, easedT);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Force ไปตำแหน่งเป้าหมาย
        transform.position = target;
    }
}
