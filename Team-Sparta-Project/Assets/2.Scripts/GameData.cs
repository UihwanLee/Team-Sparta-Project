using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TagData
{
    public const string HERO = "Hero";
    public const string TAG_BOX = "Box";
    public const string TAG_TRUCK = "Truck";
    public const string TAG_MONSTER = "Monster";
    public const string TAG_BULLET = "Bullet";

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

    [Header("Hero")]
    [SerializeField] private int heroMaxHP = 100;                           // 히어로 최대체력
    [SerializeField] private int heroDamage = 20;                           // 히어로 데미지

    [Header("Gun")]
    [SerializeField] private GameObject bulletPrefab;                       // 총알 프리팹
    [SerializeField] private float minGunRotation = -100.0f;                // 총 최소 각도
    [SerializeField] private float maxGunRotation = 10.0f;                  // 총 최대 각도
    [SerializeField] private float gunFireRate = 0.2f;                      // 총알 발사 간격
    [SerializeField] private int gunMaxBullets = 10;                        // 최대 총알 개수
    [SerializeField] private float bulletSpeed = 10.0f;                     // 총알 스피드
   
    [Header("TruckManager")]
    [SerializeField] private int boxMaxHP = 100;                            // Box 최대 체력
    [SerializeField] private float boxDropSpeed = 1000.0f;                  // Box drop 속도

    [Header("Monster")]
    [SerializeField] private List<Monster> monsterDataList;                 // Monster 리스트
    [SerializeField] private GameObject monsterPrefab;                      // Monster 프리팹
    [SerializeField] private float monsterSpawnTime = 3.0f;                 // Monster 스폰 시간
    [SerializeField] public float monsterJumpForce = 5.0f;                  // Monster 점프 속도

    [Header("MovingWheel")]
    [SerializeField] private float wheelMaxMoveSpeed = 200.0f;              // 트럭 바퀴 최대 속도
    [SerializeField] private float wheelMoveRate = 1.0f;                    // 트럭 바퀴 가속도

    // hero
    public int HeroMaxHP { get { return heroMaxHP; } }
    public int HeroDamage { get { return heroDamage; } }

    // Gun
    public GameObject BulletPrefab { get { return bulletPrefab; } }
    public float MinGunRotation { get { return minGunRotation; } }
    public float MaxGunRotation { get { return maxGunRotation; } }
    public float GunFireRate { get { return gunFireRate; } }
    public int GunMaxBullets { get { return gunMaxBullets; } }
    public float BulletSpeed { get { return bulletSpeed; } }

    // TruckManager
    public int BoxMaxHP { get { return boxMaxHP; } }
    public float BoxDropSpeed { get { return boxDropSpeed; } }

    // Monster
    public List<Monster> MonsterDataList { get { return monsterDataList; } }
    public GameObject MonsterPrefab { get { return monsterPrefab; } }
    public float MonsterJumpForce { get { return monsterJumpForce; } }
    public float MonsterSpawnTime { get { return monsterSpawnTime; } }

    // Background & Wheel
    public float WheelMaxMoveSpeed { get { return wheelMaxMoveSpeed; } }
    public float WheelMoveRate { get { return wheelMoveRate; } }


}
