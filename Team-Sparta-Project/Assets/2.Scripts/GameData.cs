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
    /* ���� �� ������ ������ �����ϴ� ��ũ��Ʈ
     *
     * ���� ���� �� ���� ������ ���� �ٲ� �� �ִ� ������ �����Ѵ�.
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
    [SerializeField] private int heroMaxHP = 100;                           // ����� �ִ�ü��
    [SerializeField] private int heroDamage = 20;                           // ����� ������

    [Header("Gun")]
    [SerializeField] private GameObject bulletPrefab;                       // �Ѿ� ������
    [SerializeField] private float minGunRotation = -100.0f;                // �� �ּ� ����
    [SerializeField] private float maxGunRotation = 10.0f;                  // �� �ִ� ����
    [SerializeField] private float gunFireRate = 0.2f;                      // �Ѿ� �߻� ����
    [SerializeField] private int gunMaxBullets = 10;                        // �ִ� �Ѿ� ����
    [SerializeField] private float bulletSpeed = 10.0f;                     // �Ѿ� ���ǵ�
   
    [Header("TruckManager")]
    [SerializeField] private int boxMaxHP = 100;                            // Box �ִ� ü��
    [SerializeField] private float boxDropSpeed = 1000.0f;                  // Box drop �ӵ�

    [Header("Monster")]
    [SerializeField] private List<Monster> monsterDataList;                 // Monster ����Ʈ
    [SerializeField] private GameObject monsterPrefab;                      // Monster ������
    [SerializeField] private float monsterSpawnTime = 3.0f;                 // Monster ���� �ð�
    [SerializeField] public float monsterJumpForce = 5.0f;                  // Monster ���� �ӵ�

    [Header("MovingWheel")]
    [SerializeField] private float wheelMaxMoveSpeed = 200.0f;              // Ʈ�� ���� �ִ� �ӵ�
    [SerializeField] private float wheelMoveRate = 1.0f;                    // Ʈ�� ���� ���ӵ�

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
