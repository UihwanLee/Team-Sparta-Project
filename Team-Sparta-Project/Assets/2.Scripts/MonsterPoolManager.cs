using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPoolManager : MonoBehaviour
{
    // 몬스터 오브젝트 폴링 관리 스크립트

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

    // 소환할 몬스터 종류
    [SerializeField]
    private GameObject monsterPrefab;
    [SerializeField]
    private int maxMonsterSize;

    // 몬스터 오브젝트를 관리할 리스트
    private GameObject[] monsterPoolList;

    private void Start()
    {
        InitMonsterPoolList();
    }

    private void InitMonsterPoolList()
    {
        // 몬스터 리스트 초기화
        monsterPoolList = new GameObject[maxMonsterSize];

        // 몬스터 세팅
        for(int i= 0; i < monsterPoolList.Length; i++)
        {
            monsterPoolList[i] = Instantiate(monsterPrefab, Vector3.zero, Quaternion.identity);
            monsterPoolList[i].GetComponent<MonsterController>().SetID(i);
            monsterPoolList[i].SetActive(false);
        }
    }

    public GameObject GetMonsterFromPool()
    {
        // 현재 몬스터 폴에서 사용할 수 있는 몬스터 반환
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
