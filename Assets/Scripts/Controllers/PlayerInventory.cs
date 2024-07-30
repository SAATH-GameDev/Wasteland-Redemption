using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public GameObject UI;

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
        for(int i = 0; i < UI.transform.childCount; i++)
            for(int s = 0; s < UI.transform.GetChild(i).childCount; s++)
                slotList.Add(UI.transform.GetChild(i).GetChild(s));
    }

    public int GetEmptySlotIndex()
    {
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
                if(profile.Type != ItemProfile.ItemType.WEAPON)
                    item.count++;
            }
            else
            {
                Item newItem = new Item();
                newItem.profile = profile;
                newItem.count = 1;
                newItem.index = GetEmptySlotIndex();
                itemList.Add(newItem);
            }
        }
    }
}
