using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public GameObject UI;
    public Vector2 offset;

    public class Item
    {
        public ItemProfile profile;
        public int count;
        public int index;
    }
    public List<Item> itemList = new List<Item>();
    private List<Transform> slotList = new List<Transform>();

    public void SetupSlots()
    {
        Transform slotGroup = UI.transform.GetChild(0);
        for(int i = 0; i < slotGroup.childCount; i++)
            for(int s = 0; s < slotGroup.GetChild(i).childCount; s++)
                slotList.Add(slotGroup.GetChild(i).GetChild(s));
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
