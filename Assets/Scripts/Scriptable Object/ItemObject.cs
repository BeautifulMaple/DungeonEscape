using UnityEngine;

// ���ͷ����� ���� �������̽�
public interface IInteractable
{
    public string GetInteractPrompt();  // UI�� ǥ���� ����
    public void OnInteract();   // ���ͷ��� ȣ��
}
public class ItemObject : MonoBehaviour, IInteractable
{

    public ItemData _itemdata;
    // �����۰� ������ ��ȯ
    public string GetInteractPrompt()
    {
        string str = ($"{_itemdata.disPlayName} \n {_itemdata.description}");
        return str;
    }
    // �������� ȹ���ϰ� ����
    public void OnInteract()
    {
        // Player ��ũ��Ʈ ���� ����
        // Player ��ũ��Ʈ�� ��ȣ�ۿ� ������ data �ѱ��
        CharacterManager.Instance.Player.itemData = _itemdata;
        CharacterManager.Instance.Player.addItem?.Invoke();
        Destroy(gameObject);
    }
}