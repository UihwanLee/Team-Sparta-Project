using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    // 몬스터 소환 스크립트

    [SerializeField]
    private float spawnTime;
    [SerializeField]
    private GameObject monsterPrefab;

    private float curTime;

    // 몬스터 오브젝트를 관리할 리스트
    private List<GameObject> monsterList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        InitMonster();
        StartCoroutine(SpawnMonsters());
    }

    private void InitMonster()
    {
        curTime = spawnTime;
    }

    IEnumerator SpawnMonsters()
    {
        while (true)
        {
            // 특정 주기 동안 몬스터 소환
            yield return new WaitForSeconds(spawnTime);

            var monster = Instantiate(monsterPrefab, this.transform.position, Quaternion.identity);   // 몬스터 소환

            if(monster != null)
            {
                MonsterPathManager.Instance.SetMonsterPath(monster);
            }
        }
    }

    private void SetMonsterType()
    {
        // 몬스터 타입 설정
    }

    private void SetMonsterLayer()
    {
        // 몬스터 레이아웃 설정
    }
}
