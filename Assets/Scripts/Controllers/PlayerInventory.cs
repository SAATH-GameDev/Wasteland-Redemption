using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public GameObject UI;
    public List<ItemProfile> itemList = new List<ItemProfile>();

    private void OnTriggerEnter(Collider other)
    {
        ICollectible collectible = other.GetComponentInParent<ICollectible>();
        if (collectible != null)
        {
            ItemProfile itemProfile = collectible.Collect();
            itemList.Add(itemProfile);
        }
    }
}
