using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public string letterContent; // ��ͤ���������
    public GameObject letterPanel; // �ҡ Panel �ͧ������������ Inspector
    public Text letterText; // ���͵�� TMP_Text ����

    private void Start()
    {
        // �١��ä�ԡ����
        GetComponent<Button>().onClick.AddListener(OpenLetter);
    }

    void OpenLetter()
    {
        letterText.text = letterContent;
        letterPanel.SetActive(true);
    }
}
