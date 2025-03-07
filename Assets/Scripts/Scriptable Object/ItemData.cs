using UnityEngine;

public enum ItemType
{
    Resouce,        // �ڿ�
    Equipable,      // ���
    Consumable,     // �Һ�
}

public enum ConsumableType
{
    Health,
    Hunger,
    JumpBoost,  // ������ ����
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
    public GameObject dropPrefab;

    [Header("Setting")]
    public bool canStack;
    public int maxStackAmount;

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;

}
