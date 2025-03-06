using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;     // �÷��̾� ��Ʈ�ѷ�
    public PlayerCondition condition;       // �÷��̾� ����
    public Transform dropPosition;          // ���� ��ġ

    public ItemData itemData;
    public Action addItem;
    public void Awake()
    {
        CharacterManager.Instance.Player = this; // �÷��̾� ĳ���͸� ĳ���� �Ŵ����� ���
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
    }
}
