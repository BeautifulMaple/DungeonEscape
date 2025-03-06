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
    public TextMeshProUGUI quantityText;    // ���� üũ
    private Outline outline;

    public UIIventory inventory;

    public int index;       // �κ��丮 ���� �ε���
    public bool equipped;   // ���� ����
    public int quantity;    // ����


    // Start is called before the first frame update
    void Awake()
    {
        outline = GetComponent<Outline>();
    }

    public void Set()
    {
        // �������� ���ٸ�
        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;    // ������ ����
        quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty;  // ������ 1���� ũ�ٸ� ������ ǥ��

        if (outline != null)
        {
            outline.enabled = enabled;
        }
    }
    public void Clear()
    {
        item = null;    //  ������ �ʱ�ȭ
        icon.gameObject.SetActive(false);   // ������ ��Ȱ��ȭ
        quantityText.text = string.Empty;   // ���� ǥ�� �ʱ�ȭ
    }

    public void OnClickButton()
    {
        inventory.SelectItem(index);
    }
}
