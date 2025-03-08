using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TagData
{
    public const string TAG_BOX = "Box";
    public const string TAG_TRUCK = "Truck";
    public const string TAG_MONSTER = "Monster";

    public const string TAG_PATH_1 = "Path_1";
    public const string TAG_PATH_2 = "Path_2";
    public const string TAG_PATH_3 = "Path_3";
}

public static class LayerData
{
    public const string LAYER_PATH_1 = "Monster Path1";
    public const string LAYER_PATH_2 = "Monster Path2";
    public const string LAYER_PATH_3 = "Monster Path3";
}

public class GameData : MonoBehaviour
{
    /* 게임 내 변수나 설정을 관리하는 스크립트
     *
     * 게임 실행 시 변수 조정을 통해 바뀔 수 있는 변수만 관리한다.
     */

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
