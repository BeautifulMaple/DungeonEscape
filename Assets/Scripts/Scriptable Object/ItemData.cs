using UnityEngine;

public enum ItemType
{
    Resouce,        // �ڿ�
    Equipablc,      // ���
    consumable,     // �Һ�
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
    public string disPlayName;  // �������� �̸�
    public string description;  // ����
    public ItemType type;
    public Sprite icon;
    public GameObject dropObject;

    [Header("Setting")]
    public bool canStack;
    public int MaxStackAmount;

    [Header("Consumable")]
    public ItemDataConsumable[] consumbles;

}
