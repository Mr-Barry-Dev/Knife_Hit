using UnityEngine;

// Har shop item ke liye ek ScriptableObject asset banega.
// Project window -> Right Click -> Create -> Shop -> Item Data
[CreateAssetMenu(fileName = "NewShopItem", menuName = "Shop/Item Data")]
public class ShopItemData : ScriptableObject
{
    [Header("Item Info")]
    public string itemId;          // Unique id, PlayerPrefs save/load ke liye use hoga
    public string itemName;
    public int price;
    public Sprite icon;

    [TextArea]
    public string description;
}