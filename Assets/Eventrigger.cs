using UnityEngine;
public class Eventrigger : MonoBehaviour
{
    public string soundName = "";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // ��Ǩ�Ѻ����ͼ����蹪�
        {
            SoundManager.Instance.PlaySound2D(soundName);
        }
    }
}