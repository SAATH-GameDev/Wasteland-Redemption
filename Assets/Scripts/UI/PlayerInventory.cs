using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class PlayerInventory : MonoBehaviour
{
    public GameObject UI;
    public Vector2 offset;
    public int selectionIndex = 0;

    public class Item
    {
        public ItemProfile profile;
        public int count;
        public int index;
    }
    public List<Item> itemList = new List<Item>();
    private List<Transform> slotList = new List<Transform>();
    private Transform selectionIndicator;
    private PlayerWeaponController weapon;

    public Item GetItemOfIndex(int index)
    {
        foreach(Item item in itemList)
            if(item.index == index)
                return item;
        return null;
    }

    public void SetupSlots()
    {
        selectionIndicator = UI.transform.GetChild(0);
        Transform slotGroup = UI.transform.GetChild(1);
        for(int i = 0; i < slotGroup.childCount; i++)
            for(int s = 0; s < slotGroup.GetChild(i).childCount; s++)
                slotList.Add(slotGroup.GetChild(i).GetChild(s));
        UpdateSlots();

        weapon = GetComponentInChildren<PlayerWeaponController>();
    }

    public void UpdateSelection(Vector2 movement)
    {
        if(movement.y != 0.0f)
            selectionIndex += movement.y < 0.0f ? 4 : -4;
        if(movement.x != 0.0f)
            selectionIndex += movement.x > 0.0f ? 1 : -1;

        if(selectionIndex >= slotList.Count)
            selectionIndex -= slotList.Count;
        else if(selectionIndex < 0)
            selectionIndex = slotList.Count + selectionIndex;

        selectionIndicator.transform.position = slotList[selectionIndex].position;
    }

    public void UseSelected()
    {
        Item selectedItem = GetItemOfIndex(selectionIndex);
        if(selectedItem != null)
        {
            switch(selectedItem.profile.Type)
            {
                case ItemProfile.ItemType.WEAPON:
                    if(weapon.profile == selectedItem.profile)
                    {
                        weapon.profile = null;
                        weapon.Set();
                        weapon.SetRightArm(0.0f);
                        weapon.SetLeftArm(0.0f);
                    }
                    else
                    {
                        weapon.Set((WeaponProfile)selectedItem.profile);
                        weapon.SetRightArm(1.0f);
                        weapon.SetLeftArm(weapon.profile.bothArms ? 1.0f : 0.0f);
                    }
                    break;

                case ItemProfile.ItemType.FOOD:
                    GetComponent<PlayerController>().IncrementHunger(selectedItem.profile.value);
                    
                    selectedItem.count--;
                    if(selectedItem.count <= 0)
                        itemList.Remove(selectedItem);
                    break;
            }
            UpdateSlots();
        }
    }

    public void DiscardSelected()
    {
        Item selectedItem = GetItemOfIndex(selectionIndex);
        if(selectedItem != null)
        {
            selectedItem.count--;
            if(selectedItem.count <= 0)
                itemList.Remove(selectedItem);
        }
        UpdateSlots();
    }

    public void UpdateSlots()
    {
        foreach(Transform slot in slotList)
        {
            slot.GetChild(0).gameObject.SetActive(false);

            slot.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";

            slot.GetChild(2).gameObject.SetActive(false);
        }
        foreach(Item item in itemList)
        {
            slotList[item.index].GetChild(0).GetComponent<Image>().sprite = item.profile.icon;
            slotList[item.index].GetChild(0).gameObject.SetActive(true);

            if(item.count > 1)
                slotList[item.index].GetChild(1).GetComponent<TextMeshProUGUI>().text = item.count.ToString();

            if(weapon.profile == item.profile)
                slotList[item.index].GetChild(2).gameObject.SetActive(true);
        }
    }

    public int GetEmptySlotIndex()
    {
        for(int i = 0; i < slotList.Count; i++)
            if(!slotList[i].GetChild(0).gameObject.activeSelf)
                return i;
        return 0;
    }

    public Item GetItemOfProfile(ItemProfile profile)
    {
        foreach(Item item in itemList)
            if(item.profile == profile)
                return item;
        return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        ICollectible collectible = other.GetComponentInParent<ICollectible>();
        if (collectible != null)
        {
            ItemProfile profile = collectible.Collect();

            Item item = GetItemOfProfile(profile);
            if(item != null)
            {
                if(item.profile.Type != ItemProfile.ItemType.WEAPON)
                {
                    item.count++;
                    UpdateSlots();
                }
            }
            else
            {
                Item newItem = new Item();
                newItem.profile = profile;
                newItem.count = 1;
                newItem.index = GetEmptySlotIndex();
                itemList.Add(newItem);
                UpdateSlots();
            }
        }
    }
}
