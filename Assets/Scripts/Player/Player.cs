using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;

    public void Awake()
    {
        CharaterManager.Instance.player = this; // �÷��̾� ĳ���͸� ĳ���� �Ŵ����� ���
        controller = GetComponent<PlayerController>();
    }
}
