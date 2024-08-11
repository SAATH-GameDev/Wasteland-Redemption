using UnityEngine;

[CreateAssetMenu(fileName = "New Item Profile", menuName = "Profile/Item", order = 0)]
public class ItemProfile : Profile
{
    public Sprite icon;
    public enum Type
    {
        COLLECTIBLE,
        WEAPON,
        AMMO,
        FOOD
    }
    public Type type;
    public float value;
    public int price;
}