using UnityEngine;
public class Eventrigger : MonoBehaviour
{
    public string soundName = "";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // ตรวจจับเมื่อผู้เล่นชน
        {
            SoundManager.Instance.PlaySound2D(soundName);
        }
    }
}