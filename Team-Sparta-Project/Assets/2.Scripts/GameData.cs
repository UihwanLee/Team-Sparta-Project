using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    // 게임 내 변수나 설정을 관리하는 스크립트

    private static GameData instance = null;

    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static GameData Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    [Header("MonsterSpawnManager")]
    [SerializeField] public float monsterSpawnTime = 3.0f;
    public float MonsterSpawnTime { get { return monsterSpawnTime; } }

    [Header("Monster")]
    [SerializeField] public float monsterJumpForce = 5.0f;
    public float MonsterJumpForce { get { return monsterJumpForce; } }

    [Header("MovingWheel")]
    [SerializeField] private float wheelMaxMoveSpeed = 200.0f;
    [SerializeField] private float wheelMoveRate = 1.0f;
    public float WheelMaxMoveSpeed { get { return wheelMaxMoveSpeed; }}
    public float WheelMoveRate { get { return wheelMoveRate; } }
}
