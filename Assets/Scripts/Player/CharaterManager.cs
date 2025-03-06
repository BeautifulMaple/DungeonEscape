using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaterManager : MonoBehaviour
{

    private static CharaterManager _instance;
    public static CharaterManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("CharaterManager").AddComponent<CharaterManager>();
            }
            return _instance;
        }
    }
    // �÷��̾� ĳ����
    private Player _player;
    public Player player
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
