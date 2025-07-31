using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NoteManager : MonoBehaviour
{
    public TMP_InputField titleInput;
    public TMP_InputField contentInput;
    public Button saveButton;
    public Button resetButton;

    public string noteID = "1"; // <-- เพิ่มหมายเลขเฉพาะของ Note นี้ เช่น "1", "2", "3", "4"

    private string TitleKey => "NoteTitle_" + noteID;
    private string ContentKey => "NoteContent_" + noteID;

    void Start()
    {
        saveButton.onClick.AddListener(SaveNote);
        resetButton.onClick.AddListener(ResetNote);
        LoadNote();
    }

    void SaveNote()
    {
        PlayerPrefs.SetString(TitleKey, titleInput.text);
        PlayerPrefs.SetString(ContentKey, contentInput.text);
        PlayerPrefs.Save();
    }

    void LoadNote()
    {
        if (PlayerPrefs.HasKey(TitleKey))
            titleInput.text = PlayerPrefs.GetString(TitleKey);
        if (PlayerPrefs.HasKey(ContentKey))
            contentInput.text = PlayerPrefs.GetString(ContentKey);
    }

    void ResetNote()
    {
        PlayerPrefs.DeleteKey(TitleKey);
        PlayerPrefs.DeleteKey(ContentKey);
        titleInput.text = "";
        contentInput.text = "";
    }
}
