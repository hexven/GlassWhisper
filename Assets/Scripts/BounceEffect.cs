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
        // �Ҽ����� (��Ѻ tag ������س��)
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
            // �駢�����ŧ ����Ҽ�����
            yield return Bounce(startPosition, targetPosition, currentHeight, currentDuration);

            // �Ѿഷ���˹������������
            startPosition = transform.position;

            // Ŵ�����٧�������
            currentHeight *= 0.7f;
            currentDuration *= 0.8f;
        }

        // 仵��˹觼�����
        if (playerTransform)
            transform.position = targetPosition;
    }

    private IEnumerator Bounce(Vector3 start, Vector3 target, float height, float duration)
    {
        Vector3 midPoint = Vector3.Lerp(start, target, 0.5f);
        Vector3 peak = midPoint + Vector3.up * height;

        float halfDuration = duration / 2f;
        float elapsed = 0f;

        // �駢��
        while (elapsed < halfDuration)
        {
            float t = elapsed / halfDuration;
            float easedT = 1f - (1f - t) * (1f - t);
            transform.position = Vector3.Lerp(start, peak, easedT);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // ��ŧ ���˹����Ҽ�����
        elapsed = 0f;
        while (elapsed < halfDuration)
        {
            float t = elapsed / halfDuration;
            float easedT = t * t;
            transform.position = Vector3.Lerp(peak, target, easedT);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Force 仵��˹��������
        transform.position = target;
    }
}
