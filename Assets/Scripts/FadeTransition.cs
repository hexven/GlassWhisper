using UnityEngine;
using Unity.Cinemachine;
using System.Collections; // เพิ่มสำหรับ Coroutine

public class MapTransition : MonoBehaviour
{
    [SerializeField] PolygonCollider2D mapBoundry;
    [SerializeField] CanvasGroup fadeCanvasGroup; // เพิ่มสำหรับ fade effect
    CinemachineConfiner2D confiner;
    [SerializeField] Direction direction;
    [SerializeField] Transform teleportTargetPosition;
    enum Direction { Up, Down, Left, Right, Teleport }

    private void Awake()
    {
        confiner = FindObjectOfType<CinemachineConfiner2D>();
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 0; // เริ่มต้นให้โปร่งใส
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(TransitionWithFade(collision.gameObject));
        }
    }

    private IEnumerator TransitionWithFade(GameObject player)
    {
        // ปิดการเคลื่อนที่ของกล้องชั่วคราว
        var virtualCamera = confiner.GetComponent<CinemachineVirtualCamera>();
        virtualCamera.enabled = false;

        // Fade out
        yield return StartCoroutine(Fade(0f, 1f, 0.5f));

        // อัพเดทตำแหน่ง
        confiner.BoundingShape2D = mapBoundry;
        UpdatePlayerPosition(player);

        // รอให้กล้องปรับตำแหน่ง
        yield return new WaitForEndOfFrame();

        // Fade in
        yield return StartCoroutine(Fade(1f, 0f, 0.5f));

        // เปิดกล้องกลับ
        virtualCamera.enabled = true;
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            yield return null;
        }
        fadeCanvasGroup.alpha = endAlpha;
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
}