using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{

    private static CharacterManager _instance;
    public static CharacterManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("CharacterManager").AddComponent<CharacterManager>();
            }
            return _instance;
        }
    }
    // �÷��̾� ĳ����
    private Player _player;
    public Player Player
    {
        get { return _player; }
        set { _player = value; }
    }

    public void Awake()
    {
        if (_instance == null)  // �ν��Ͻ��� ���ٸ�
        {
            _instance = this;   // �� ��ü�� �ν��Ͻ��� ����
            DontDestroyOnLoad(gameObject);  // ���� ����Ǿ �ı����� �ʰ� ����
        }
        else if (_instance != this)     // �ν��Ͻ��� �̹� �����ϸ�
        {   
            Destroy(gameObject);    // ���� ������ ��ü�� �ı�
        }
    }

}
