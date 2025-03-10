using UnityEngine;

// 인터랙션을 위한 인터페이스
public interface IInteractable
{
    public string GetInteractPrompt();  // UI에 표시할 정보
    public void OnInteract();   // 인터랙션 호출
}
public class ItemObject : MonoBehaviour, IInteractable
{

    public ItemData _itemdata;
    // 아이템과 설명을 반환
    public string GetInteractPrompt()
    {
        string str = ($"{_itemdata.disPlayName} \n {_itemdata.description}");
        return str;
    }
    // 아이템을 획득하고 삭제
    public void OnInteract()
    {
        if (!(_itemdata.type == ItemType.interactable))
        {
            // Player 스크립트 먼저 수정
            // Player 스크립트에 상호작용 아이템 data 넘기기
            CharacterManager.Instance.Player.itemData = _itemdata;
            CharacterManager.Instance.Player.addItem?.Invoke();
            Destroy(gameObject);
        }
        else
        {
            // 상호작용 하기
        }
    }
}