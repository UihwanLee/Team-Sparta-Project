using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPoolManager : MonoBehaviour
{
    // ���� ������Ʈ ���� ���� ��ũ��Ʈ

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
    [SerializeField]
    private GameObject monsterPrefab;
    [SerializeField]
    private int maxMonsterSize;

    // ���� ������Ʈ�� ������ ����Ʈ
    private GameObject[] monsterPoolList;

    private void Start()
    {
        InitMonsterPoolList();
    }

    private void InitMonsterPoolList()
    {
        // ���� ����Ʈ �ʱ�ȭ
        monsterPoolList = new GameObject[maxMonsterSize];

        // ���� ����
        for(int i= 0; i < monsterPoolList.Length; i++)
        {
            monsterPoolList[i] = Instantiate(monsterPrefab, Vector3.zero, Quaternion.identity);
            monsterPoolList[i].GetComponent<MonsterController>().SetID(i);
            monsterPoolList[i].SetActive(false);
        }
    }

    public GameObject GetMonsterFromPool()
    {
        // ���� ���� ������ ����� �� �ִ� ���� ��ȯ
        for(int i=0; i < monsterPoolList.Length; i++)
        {
            if (monsterPoolList[i].activeSelf == false)
            {
                monsterPoolList[i].SetActive(true);
                return monsterPoolList[i];
            }
        }

        return null;
    }

    public GameObject GetMonsterFromPool(int _id)
    {
        return monsterPoolList[_id];
    }

    public void ReleaseMonsterFromPool(int _id)
    {
        monsterPoolList[_id].SetActive(false);
    }
}
