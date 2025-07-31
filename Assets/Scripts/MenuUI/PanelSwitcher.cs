using UnityEngine;

public class PanelSwitcher : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject daughterPanel;
    public GameObject dadPanel;
    public GameObject momPanel;

    void Start()
    {
        ShowMainPanel();
    }

    public void ShowMainPanel()
    {
        mainPanel.SetActive(true);
        daughterPanel.SetActive(false);
        dadPanel.SetActive(false);
        momPanel.SetActive(false);
    }

    public void ShowDaughterPanel()
    {
        mainPanel.SetActive(false);
        daughterPanel.SetActive(true);
        dadPanel.SetActive(false);
        momPanel.SetActive(false);
    }

    public void ShowDadPanel()
    {
        mainPanel.SetActive(false);
        daughterPanel.SetActive(false);
        dadPanel.SetActive(true);
        momPanel.SetActive(false);
    }

    public void ShowMomPanel()
    {
        mainPanel.SetActive(false);
        daughterPanel.SetActive(false);
        dadPanel.SetActive(false);
        momPanel.SetActive(true);
    }
}
