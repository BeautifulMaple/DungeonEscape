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
    // 플레이어 캐릭터
    private Player _player;
    public Player player
    {
        get { return _player; }
        set { _player = value; }
    }

    public void Awake()
    {
        if (_instance == null)  // 인스턴스가 없다면
        {
            _instance = this;   // 이 객체를 인스턴스로 지정
            DontDestroyOnLoad(gameObject);  // 씬이 변경되어도 파괴되지 않게 설정
        }
        else if (_instance != this)     // 인스턴스가 이미 존재하면
        {   
            Destroy(gameObject);    // 새로 생성된 객체를 파괴
        }
    }

}
