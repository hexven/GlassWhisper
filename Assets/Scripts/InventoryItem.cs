using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public string letterContent; // ข้อความจดหมาย
    public GameObject letterPanel; // ลาก Panel ของจดหมายมาใส่ใน Inspector
    public Text letterText; // หรือตัว TMP_Text ก็ได้

    private void Start()
    {
        // ผูกการคลิกปุ่ม
        GetComponent<Button>().onClick.AddListener(OpenLetter);
    }

    void OpenLetter()
    {
        letterText.text = letterContent;
        letterPanel.SetActive(true);
    }
}
