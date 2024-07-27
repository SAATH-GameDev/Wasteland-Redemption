using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<ItemProfile> inventory = new List<ItemProfile>();

    private void OnTriggerEnter(Collider other)
    {
        ICollectible collectible = other.GetComponentInParent<ICollectible>();
        if (collectible != null)
        {
            ItemProfile itemProfile = collectible.Collect();
            inventory.Add(itemProfile);
        }
    }
}
