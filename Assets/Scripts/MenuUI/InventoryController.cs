using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Unity.VisualScripting;

public class InventoryController : MonoBehaviour
{
    public GameObject InventoryPanel;
    public GameObject slotPrefab;
    public int slotCount;
    public GameObject[] itemPrefabs;


    void Start()
    {
        for (int i = 0; i < slotCount; i++)
        {
            Slot slot = Instantiate(slotPrefab, InventoryPanel.transform).GetComponent<Slot>();
            if (i < itemPrefabs.Length)
            {
                GameObject item = Instantiate(itemPrefabs[i], slot.transform);
                item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = item;
            }
        }
    }

    public bool AddItem(GameObject itemPrefab)
    {
        foreach (Transform slotTranform in InventoryPanel.transform)
        {
            Slot slot = slotTranform.GetComponent<Slot>();
            if (slot != null && slot.currentItem == null)
            {
                GameObject newItem = Instantiate(itemPrefab, slotTranform);
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = newItem;
                if (Item.idStatic == 1)
                {
                    chooseSystem.item = true;
                }

                else if (Item.idStatic == 20)
                {
                    chooseSystem.letter = true;
                }

                else if (Item.idStatic == 6)
                {
                    chooseSystem.soap = true;
                }

                else if (Item.idStatic == 7)
                {
                    chooseSystem.trash = true;
                }

                else if (Item.idStatic == 8)
                {
                    chooseSystem.money = true;
                }

                else if (Item.idStatic == 9)
                {
                    chooseSystem.nahh3 = true;
                }

                else if (Item.idStatic == 3)
                {
                    chooseSystem.box = true;
                }

                else if (Item.idStatic == 11)
                {
                    chooseSystem.mouseTrap = true;
                }

                else if (Item.idStatic == 12)
                {
                    chooseSystem.book1 = true;
                }

                else if (Item.idStatic == 13)
                {
                    chooseSystem.book2 = true;
                }

                else if (Item.idStatic == 14)
                {
                    chooseSystem.diary = true;
                }

                else if (Item.idStatic == 5)
                {
                    chooseSystem.nahhNewspaper = true;
                }

                //else if (Item.idStatic == 3)
                //{
                //    chooseSystem.note = true;
                //}

                else if (Item.idStatic == 2)
                {
                    chooseSystem.oldPic = true;
                }

                //else if (Item.idStatic == 15)
                //{
                //    chooseSystem.key = true;
                //}

                else if (Item.idStatic == 25)
                {
                    chooseSystem.note2 = true;
                }

                else if (Item.idStatic == 26)
                {
                    chooseSystem.nahh6 = true;
                }
                return true;
            }
        }
        Debug.Log("Inventory is Full!");
        return false;
    }

    public void DeselectAllSlots()
    {
        foreach (Transform slotTransform in InventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null)
            {
                slot.DeselectSlot();
            }
        }

    }


    public Item GetSelectedItem()
    {
        foreach (Transform slotTransform in InventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null && slot.thisItemSelected && slot.currentItem != null)
            {
                return slot.currentItem.GetComponent<Item>();
            }
        }
        return null;
    }
    public bool HasItem(GameObject itemPrefab)
    {
        foreach (Transform slotTransform in InventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null && slot.currentItem != null)
            {
                // เปรียบเทียบว่าไอเทมในช่องเป็นชนิดเดียวกับ itemPrefab
                if (slot.currentItem.name.StartsWith(itemPrefab.name)) // แบบง่าย (ชื่อ prefab)
                {
                    return true;
                }

                // หรือใช้ Compare แบบละเอียด
                Item item = slot.currentItem.GetComponent<Item>();
                Item refItem = itemPrefab.GetComponent<Item>();
                if (item != null && refItem != null && item.ID == refItem.ID)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public void RemoveItem(GameObject itemPrefab)
    {
        foreach (Transform slotTransform in InventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null && slot.currentItem != null)
            {
                // เปรียบเทียบจากชื่อ prefab
                if (slot.currentItem.name.StartsWith(itemPrefab.name))
                {
                    GameObject.Destroy(slot.currentItem);
                    slot.currentItem = null;
                    return;
                }

                // เปรียบเทียบด้วย ID ก็ได้ถ้ามี
                Item item = slot.currentItem.GetComponent<Item>();
                Item refItem = itemPrefab.GetComponent<Item>();
                if (item != null && refItem != null && item.ID == refItem.ID)
                {
                    GameObject.Destroy(slot.currentItem);
                    slot.currentItem = null;
                    return;
                }
            }
        }
    }
}

