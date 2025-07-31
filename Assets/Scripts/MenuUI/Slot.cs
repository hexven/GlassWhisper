using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    public GameObject currentItem;
    public GameObject selectedShader;
    public bool thisItemSelected;
    private InventoryController inventoryController;

    void Start()
    {
        inventoryController = FindObjectOfType<InventoryController>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }
    }

    public void OnLeftClick()
    {
        if (inventoryController != null)
        {
            inventoryController.DeselectAllSlots();
            if (ItemTooltipUI.Instance != null)
            {
                ItemTooltipUI.Instance.HideTooltip();
            }
        }

        SoundManager.Instance.PlaySound2D("Click");
        selectedShader.SetActive(true);
        thisItemSelected = true;

        if (currentItem != null)
        {
            Item itemComponent = currentItem.GetComponent<Item>();
            if (itemComponent != null)
            {
                ShowItemTooltip();

                if (itemComponent.isBox && ItemTooltipUI.Instance != null)
                {
                    ItemTooltipUI.Instance.ShowOpenBoxButton(true);
                    ItemTooltipUI.Instance.SetCurrentBox(itemComponent, this);
                }
                else
                {
                    if (ItemTooltipUI.Instance != null)
                    {
                        ItemTooltipUI.Instance.ShowOpenBoxButton(false);
                    }
                }
                return;
            }
        }
        ShowItemTooltip(); // Fallback
    }

    public void OnRightClick()
    {
        // ฟังก์ชันสำหรับคลิกขวา (ถ้าต้องการ)
    }

    public void DeselectSlot()
    {
        selectedShader.SetActive(false);
        thisItemSelected = false;
        if (ItemTooltipUI.Instance != null)
        {
            ItemTooltipUI.Instance.HideTooltip();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowItemTooltip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideItemTooltip();
    }

private void ShowItemTooltip()
{
    if (currentItem != null)
    {
        Item itemComponent = currentItem.GetComponent<Item>();
        if (itemComponent != null && ItemTooltipUI.Instance != null)
        {
            Sprite itemIcon = null;
            Image itemImage = currentItem.GetComponent<Image>();
            if (itemImage != null)
            {
                itemIcon = itemImage.sprite;
            }
            ItemTooltipUI.Instance.ShowTooltip(
                itemComponent.Name,
                itemComponent.Description,
                itemIcon,
                itemComponent.hasNote,
                itemComponent.noteImage,
                itemComponent.noteDisplayType
            );
            // ส่ง Item ไปยัง Tooltip
            ItemTooltipUI.Instance.SetCurrentItem(itemComponent); // เพิ่ม method นี้ใน ItemTooltipUI
        }
    }
}

    private void HideItemTooltip()
    {
        if (ItemTooltipUI.Instance != null)
        {
            ItemTooltipUI.Instance.HideTooltip();
        }
    }
}