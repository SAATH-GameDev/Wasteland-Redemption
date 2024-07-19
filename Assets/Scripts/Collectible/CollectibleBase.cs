using UnityEngine;

public class CollectibleBase : MonoBehaviour, ICollectible
{
    public ItemProfile itemProfile;

    public virtual ItemProfile Collect()
    {
        Destroy(gameObject);
        return itemProfile;
    }
}
