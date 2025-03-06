using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public ItemData item;
    public Button button;
    public Image icon;
    public TextMeshProUGUI quantityText;    // 수량 체크
    private Outline outline;

    public UIIventory inventory;

    public int index;       // 인벤토리 슬롯 인덱스
    public bool equipped;   // 장착 여부
    public int quantity;    // 수량


    // Start is called before the first frame update
    void Awake()
    {
        outline = GetComponent<Outline>();
    }

    public void Set()
    {
        // 아이템이 없다면
        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;    // 아이콘 설정
        quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty;  // 수량이 1보다 크다면 수량을 표시

        if (outline != null)
        {
            outline.enabled = enabled;
        }
    }
    public void Clear()
    {
        item = null;    //  아이템 초기화
        icon.gameObject.SetActive(false);   // 아이콘 비활성화
        quantityText.text = string.Empty;   // 수량 표시 초기화
    }

    public void OnClickButton()
    {
        inventory.SelectItem(index);
    }
}
