using System.Collections.Generic;
using UnityEngine;

public class openPuzzle : MonoBehaviour, IInteractable
{
    public bool IsOpened { get; private set; }
    public string ChestID { get; private set; }
    public GameObject puzzleCanvas;
    public Sprite openedSprite;
    
    void Start()
    {
        ChestID ??= GlobalHelper.GenerateUniqueID(gameObject);
        puzzleCanvas.SetActive(false);
    }
    public bool CanInteract()
    {
        return !IsOpened;
    }
    public void Interact()
    {
        if (!CanInteract()) return;
        OpenChest(); // ���¡ OpenedChest() � Interact()
    }
    private void OpenChest()
    {
        puzzleCanvas.SetActive(true);
        SetOpened(true);

    }
    public void SetOpened(bool opened)
    {
        IsOpened = opened; // ��䢨ҡ if(IsOpened = opened) �� IsOpened = opened
        if (IsOpened)
        {
            GetComponent<SpriteRenderer>().sprite = openedSprite;
            
        }
        chooseSystem.note = true;
    }
}
