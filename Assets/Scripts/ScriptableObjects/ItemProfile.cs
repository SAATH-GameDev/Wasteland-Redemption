using UnityEngine;

[CreateAssetMenu(fileName = "New Item Profile", menuName = "Profile/Item", order = 0)]
public class ItemProfile : Profile
{
    public enum ItemType
    {
        COLLECTIBLE,
        WEAPON,
        FOOD
    }
    public ItemType Type;
    public float value;

    [Space]
    public int buyPrice;
    public int sellPrice;
}