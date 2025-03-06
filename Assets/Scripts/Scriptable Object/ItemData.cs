using UnityEngine;

public enum ItemType
{
    Resouce,        // 자원
    Equipablc,      // 장비
    consumable,     // 소비
}

public enum ConsumableType
{
    Health,
    hunger, 
}
[System.Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value;
}
[CreateAssetMenu (fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Item Info")]
    public string disPlayName;  // 보여지는 이름
    public string description;  // 설명
    public ItemType type;
    public Sprite icon;
    public GameObject dropObject;

    [Header("Setting")]
    public bool canStack;
    public int MaxStackAmount;

    [Header("Consumable")]
    public ItemDataConsumable[] consumbles;

}
