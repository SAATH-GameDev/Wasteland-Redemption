using UnityEngine;

public abstract class CollectibleBase : MonoBehaviour, ICollectible
{
    public ItemProfile itemProfile;

    public virtual ItemProfile Collect()
    {
        Destroy(gameObject);
        return itemProfile;
    }
}
