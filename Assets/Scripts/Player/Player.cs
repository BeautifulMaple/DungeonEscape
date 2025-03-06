using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;

    public void Awake()
    {
        CharaterManager.Instance.player = this; // 플레이어 캐릭터를 캐릭터 매니저에 등록
        controller = GetComponent<PlayerController>();
    }
}
