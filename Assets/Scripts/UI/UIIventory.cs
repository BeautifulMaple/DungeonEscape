using TMPro;
using Unity.Properties;
using UnityEngine;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class UIIventory : MonoBehaviour
{
    public ItemSlot[] slots;            // 인벤토리 슬롯
    public GameObject inventoryWindow;  // 인벤토리 창
    public Transform slotParent;        //
    public Transform dropPosition;      // 버릴 위치

    [Header("Select item")]
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedStatName;
    public TextMeshProUGUI selectedStatValue;

    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unequipButton;
    public GameObject dropButton;

    private PlayerController playerController;   // 플레이어 컨트롤러
    private PlayerCondition playerCondition;     // 플레이어 상태

    ItemData selectedItem;
    int selectedItemIndex = 0;

    int curEquipIndex;

    // Start is called before the first frame update
    void Start()
    {
        playerCondition = CharacterManager.Instance.Player.condition;
        playerController = CharacterManager.Instance.Player.controller;
        CharacterManager.Instance.Player.addItem += AddItem;
        dropPosition = CharacterManager.Instance.Player.dropPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SelectItem(int index)
    {
        if (slots[index].item == null) return;

        selectedItem = slots[index].item;
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.disPlayName;
        selectedItemDescription.text = selectedItem.description;

        selectedItemName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        for (int i = 0; i < selectedItem.consumbles.Length; i++)
        {
            selectedStatName.text += selectedItem.consumbles[i].type.ToString() + "\n";
            selectedStatValue.text += selectedItem.consumbles[i].type.ToString() + "\n";
        }

        useButton.SetActive(selectedItem.type == ItemType.consumable);
        equipButton.SetActive(selectedItem.type == ItemType.Equipable && !slots[index].equipped);
        unequipButton.SetActive(selectedItem.type == ItemType.Equipable && slots[index].equipped);
        dropButton.SetActive(true);
    }

    ItemSlot GetItemStack(ItemData data)
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == data && slots[i].quantity < data.maxStackAmount)
            {
                return slots[i];
            }
        }
        return null;
    }

    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }
        return null;
    }

    void AddItem()
    {
        ItemData iData = CharacterManager.Instance.Player.itemData;

        if (iData.canStack)
        {
            ItemSlot slot = GetItemStack(iData);

        }
    }
}
