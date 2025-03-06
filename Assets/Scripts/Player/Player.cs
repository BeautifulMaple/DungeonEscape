using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;     // 플레이어 컨트롤러
    public PlayerCondition condition;       // 플레이어 상태


    public void Awake()
    {
        CharacterManager.Instance.Player = this; // 플레이어 캐릭터를 캐릭터 매니저에 등록
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
    }
}
