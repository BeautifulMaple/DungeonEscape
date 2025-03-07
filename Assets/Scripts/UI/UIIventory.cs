using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIIventory : MonoBehaviour
{
    public ItemSlot[] slots;            // 인벤토리 슬롯 배열
    public GameObject inventoryWindow;  // 인벤토리 창
    public Transform slotParent;        // 슬롯 부모 오브젝트
    public Transform dropPosition;      // 아이템을 버릴 위치

    [Header("Select item")]
    public TextMeshProUGUI selectedItemName;        // 선택된 아이템의 이름을 표시할 UI 텍스트
    public TextMeshProUGUI selectedItemDescription; // 선택된 아이템의 설명을 표시할 UI 텍스트
    public TextMeshProUGUI selectedStatName;        // 선택된 아이템의 스탯 이름을 표시할 UI 텍스트
    public TextMeshProUGUI selectedStatValue;       // 선택된 아이템의 스탯 값을 표시할 UI 텍스트

    // 아이템 사용, 장착, 해제, 버리기 버튼
    public Button useButton;    // 아이템 사용 버튼
    public Button equipButton;  // 아이템 장착 버튼
    public Button unequipButton;// 아이템 해제 버튼
    public Button dropButton;   // 아이템 버리기 버튼

    private PlayerController playerController;   // 플레이어 컨트롤러 참조
    private PlayerCondition playerCondition;     // 플레이어 상태 참조

    ItemData selectedItem;    // 선택한 아이템 데이터
    int selectedItemIndex = 0; // 선택한 아이템의 인덱스
    int curEquipIndex;         // 현재 장착한 아이템의 인덱스

    void Start()
    {
        // 플레이어 상태 및 컨트롤러 가져오기
        playerCondition = CharacterManager.Instance.Player.condition;
        playerController = CharacterManager.Instance.Player.controller;
        dropPosition = CharacterManager.Instance.Player.dropPosition;

        // 아이템 추가 이벤트 등록
        CharacterManager.Instance.Player.addItem += AddItem;
        playerController.Inventory += Toggle; // 인벤토리 토글 이벤트 등록

        // 버튼 이벤트 리스너 추가
        useButton.onClick.AddListener(OnUeseButton);
        dropButton.onClick.AddListener(OnDropButton);

        inventoryWindow.SetActive(false); // 시작 시 인벤토리 창 비활성화

        // 슬롯 배열 초기화
        slots = new ItemSlot[slotParent.childCount];
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotParent.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
        }

        ClearSelctedItemWindow(); // 선택된 아이템 정보 초기화
    }

    // 선택된 아이템 정보 초기화
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

    // 인벤토리 창을 여닫기
    public void Toggle()
    {
        inventoryWindow.SetActive(!IsOpen());
    }

    // 인벤토리 창이 열려 있는지 확인
    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    // 아이템 선택
    public void SelectItem(int index)
    {
        if (slots[index].item == null) return;

        selectedItem = slots[index].item;
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.disPlayName;
        selectedItemDescription.text = selectedItem.description;

        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        // 소비 아이템의 능력치를 UI에 표시
        for (int i = 0; i < selectedItem.consumables.Length; i++)
        {
            selectedStatName.text += selectedItem.consumables[i].type.ToString() + "\n";
            selectedStatValue.text += selectedItem.consumables[i].value.ToString() + "\n";
        }

        // 버튼 활성화 여부 설정
        useButton.gameObject.SetActive(selectedItem.type == ItemType.Consumable);
        equipButton.gameObject.SetActive(selectedItem.type == ItemType.Equipable && !slots[index].equipped);
        unequipButton.gameObject.SetActive(selectedItem.type == ItemType.Equipable && slots[index].equipped);
        dropButton.gameObject.SetActive(true);
    }

    // 중첩 가능한 아이템 슬롯 찾기
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

    // 빈 슬롯 찾기
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

    // 아이템 추가
    void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemData;

        if (data.canStack) // 중첩 가능 여부 확인
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

        ItemSlot emptySlot = GetEmptySlot(); // 빈 슬롯 확인

        if (emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdataUI();
            CharacterManager.Instance.Player.itemData = null;
            return;
        }

        ThrowItem(data); // 빈 슬롯이 없으면 바닥에 아이템 드랍
        CharacterManager.Instance.Player.itemData = null;
    }

    // UI 갱신
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

    // 아이템 버리기 (바닥에 드랍)
    void ThrowItem(ItemData data)
    {
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }

    // 아이템 사용 버튼 클릭 시
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
                        playerController.AddJumpForce(selectedItem.consumables[i].value, 10f); // 예: 10초 동안 점프력 증가.
                        break;
                }
            }
            RemoveSelectedItem(); // 사용 후 제거
        }
    }

    // 아이템 버리기 버튼 클릭 시
    public void OnDropButton()
    {
        ThrowItem(selectedItem);
        RemoveSelectedItem();
    }

    // 선택된 아이템 제거
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
