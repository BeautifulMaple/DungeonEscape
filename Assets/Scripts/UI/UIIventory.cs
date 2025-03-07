using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIIventory : MonoBehaviour
{
    public ItemSlot[] slots;            // �κ��丮 ���� �迭
    public GameObject inventoryWindow;  // �κ��丮 â
    public Transform slotParent;        // ���� �θ� ������Ʈ
    public Transform dropPosition;      // �������� ���� ��ġ

    [Header("Select item")]
    public TextMeshProUGUI selectedItemName;        // ���õ� �������� �̸��� ǥ���� UI �ؽ�Ʈ
    public TextMeshProUGUI selectedItemDescription; // ���õ� �������� ������ ǥ���� UI �ؽ�Ʈ
    public TextMeshProUGUI selectedStatName;        // ���õ� �������� ���� �̸��� ǥ���� UI �ؽ�Ʈ
    public TextMeshProUGUI selectedStatValue;       // ���õ� �������� ���� ���� ǥ���� UI �ؽ�Ʈ

    // ������ ���, ����, ����, ������ ��ư
    public Button useButton;    // ������ ��� ��ư
    public Button equipButton;  // ������ ���� ��ư
    public Button unequipButton;// ������ ���� ��ư
    public Button dropButton;   // ������ ������ ��ư

    private PlayerController playerController;   // �÷��̾� ��Ʈ�ѷ� ����
    private PlayerCondition playerCondition;     // �÷��̾� ���� ����

    ItemData selectedItem;    // ������ ������ ������
    int selectedItemIndex = 0; // ������ �������� �ε���
    int curEquipIndex;         // ���� ������ �������� �ε���

    void Start()
    {
        // �÷��̾� ���� �� ��Ʈ�ѷ� ��������
        playerCondition = CharacterManager.Instance.Player.condition;
        playerController = CharacterManager.Instance.Player.controller;
        dropPosition = CharacterManager.Instance.Player.dropPosition;

        // ������ �߰� �̺�Ʈ ���
        CharacterManager.Instance.Player.addItem += AddItem;
        playerController.Inventory += Toggle; // �κ��丮 ��� �̺�Ʈ ���

        // ��ư �̺�Ʈ ������ �߰�
        useButton.onClick.AddListener(OnUeseButton);
        dropButton.onClick.AddListener(OnDropButton);

        inventoryWindow.SetActive(false); // ���� �� �κ��丮 â ��Ȱ��ȭ

        // ���� �迭 �ʱ�ȭ
        slots = new ItemSlot[slotParent.childCount];
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotParent.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
        }

        ClearSelctedItemWindow(); // ���õ� ������ ���� �ʱ�ȭ
    }

    // ���õ� ������ ���� �ʱ�ȭ
    void ClearSelctedItemWindow()
    {
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        useButton.gameObject.SetActive(false);
        equipButton.gameObject.SetActive(false);
        unequipButton.gameObject.SetActive(false);
        dropButton.gameObject.SetActive(false);
    }

    // �κ��丮 â�� ���ݱ�
    public void Toggle()
    {
        inventoryWindow.SetActive(!IsOpen());
    }

    // �κ��丮 â�� ���� �ִ��� Ȯ��
    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    // ������ ����
    public void SelectItem(int index)
    {
        if (slots[index].item == null) return;

        selectedItem = slots[index].item;
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.disPlayName;
        selectedItemDescription.text = selectedItem.description;

        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        // �Һ� �������� �ɷ�ġ�� UI�� ǥ��
        for (int i = 0; i < selectedItem.consumables.Length; i++)
        {
            selectedStatName.text += selectedItem.consumables[i].type.ToString() + "\n";
            selectedStatValue.text += selectedItem.consumables[i].value.ToString() + "\n";
        }

        // ��ư Ȱ��ȭ ���� ����
        useButton.gameObject.SetActive(selectedItem.type == ItemType.Consumable);
        equipButton.gameObject.SetActive(selectedItem.type == ItemType.Equipable && !slots[index].equipped);
        unequipButton.gameObject.SetActive(selectedItem.type == ItemType.Equipable && slots[index].equipped);
        dropButton.gameObject.SetActive(true);
    }

    // ��ø ������ ������ ���� ã��
    ItemSlot GetItemStack(ItemData data)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == data && slots[i].quantity < data.maxStackAmount)
            {
                return slots[i];
            }
        }
        return null;
    }

    // �� ���� ã��
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

    // ������ �߰�
    void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemData;

        if (data.canStack) // ��ø ���� ���� Ȯ��
        {
            ItemSlot slot = GetItemStack(data);
            if (slot != null)
            {
                slot.quantity++;
                UpdataUI();
                CharacterManager.Instance.Player.itemData = null;
                return;
            }
        }

        ItemSlot emptySlot = GetEmptySlot(); // �� ���� Ȯ��

        if (emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdataUI();
            CharacterManager.Instance.Player.itemData = null;
            return;
        }

        ThrowItem(data); // �� ������ ������ �ٴڿ� ������ ���
        CharacterManager.Instance.Player.itemData = null;
    }

    // UI ����
    void UpdataUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                slots[i].Set();
            }
            else
            {
                slots[i].Clear();
            }
        }
    }

    // ������ ������ (�ٴڿ� ���)
    void ThrowItem(ItemData data)
    {
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }

    // ������ ��� ��ư Ŭ�� ��
    public void OnUeseButton()
    {
        if (selectedItem.type == ItemType.Consumable)
        {
            for (int i = 0; i < selectedItem.consumables.Length; i++)
            {
                switch (selectedItem.consumables[i].type)
                {
                    case ConsumableType.Hunger:
                        playerCondition.Eat(selectedItem.consumables[i].value);
                        break;
                    case ConsumableType.Health:
                        playerCondition.Heal(selectedItem.consumables[i].value);
                        break;
                    case ConsumableType.JumpBoost:
                        playerController.AddJumpForce(selectedItem.consumables[i].value, 10f); // ��: 10�� ���� ������ ����.
                        break;
                }
            }
            RemoveSelectedItem(); // ��� �� ����
        }
    }

    // ������ ������ ��ư Ŭ�� ��
    public void OnDropButton()
    {
        ThrowItem(selectedItem);
        RemoveSelectedItem();
    }

    // ���õ� ������ ����
    void RemoveSelectedItem()
    {
        slots[selectedItemIndex].quantity--;
        if (slots[selectedItemIndex].quantity <= 0)
        {
            selectedItem = null;
            slots[selectedItemIndex].item = null;
            selectedItemIndex = -1;
            ClearSelctedItemWindow();
        }
        UpdataUI();
    }
}
