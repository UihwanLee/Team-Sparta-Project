using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPoolManager : MonoBehaviour
{
    /* 
     * ���� ������Ʈ ���� ���� ��ũ��Ʈ
     *
     * ���� Ÿ�� �� �迭�� ����
    */

    private static MonsterPoolManager instance = null;

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

    public static MonsterPoolManager Instance
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

    // ��ȯ�� ���� ����
    [SerializeField] private List<Monster> monsters;
    [SerializeField] private GameObject monsterPrefab;

    // ���� ������Ʈ�� ������ ����Ʈ
    private Dictionary<int, GameObject[]> monsterPoolList = new Dictionary<int, GameObject[]>();

    private void Start()
    {
        InitMonsterPoolList();
    }

    private void InitMonsterPoolList()
    {
        // ���� Ÿ�� �� ����Ʈ �ʱ�ȭ
        foreach(Monster monster in monsters)
        {
            GameObject[] monsterList = new GameObject[monster.maxCount];

            // ���� ����
            for (int i = 0; i < monsterList.Length; i++)
            {
                monsterList[i] = Instantiate(monsterPrefab, Vector3.zero, Quaternion.identity);

                monsterList[i].GetComponent<MonsterController>().SetInfo(i, monster.title, monster.level, monster.hp, monster.damage, monster.speed);
                monsterList[i].GetComponent<MonsterController>().SetSprites(monster.sprites);
                monsterList[i].SetActive(false);
            }

            monsterPoolList[monster.level] = monsterList;
        }
    }

    public GameObject GetMonsterFromPool(int _level)
    {
        // �־��� ���� ���� ������ ����� �� �ִ� ���� ��ȯ
        GameObject[] monsterList = monsterPoolList[_level];
        if(monsterList == null) { Debug.LogError("That monster list does not exist!"); return null; }

        for (int i=0; i < monsterList.Length; i++)
        {
            if (monsterList[i].activeSelf == false)
            {
                monsterList[i].SetActive(true);
                return monsterList[i];
            }
        }

        return null;
    }

    public GameObject GetMonsterFromPool(int _level, int _id)
    {
        GameObject[] monsterList = monsterPoolList[_level];
        if (monsterList == null) { Debug.LogError("That monster list does not exist!"); return null; }

        return monsterList[_id];
    }

    public void ReleaseMonsterFromPool(int _level, int _id)
    {
        GameObject[] monsterList = monsterPoolList[_level];
        if (monsterList == null) { Debug.LogError("That monster list does not exist!"); return; }

        monsterList[_id].SetActive(false);
    }
}
