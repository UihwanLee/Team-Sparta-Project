using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnManager : MonoBehaviour
{
    // 몬스터 소환 스크립트

    [SerializeField]
    private Transform spawnPos;
    [SerializeField]
    private float spawnTime;

    private float curTime;

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

            SpawnMonsterPoint();
        }
    }

    private void SpawnMonsterPoint()
    {
        int level = 0;
        GameObject monster = MonsterPoolManager.Instance.GetMonsterFromPool(level);

        if (monster == null)
        {
            Debug.LogError("There are no objects available!");
        }

        // 위치 설정
        monster.transform.position = spawnPos.position;

        // 경로 설정
        MonsterPathManager.Instance.SetMonsterPath(monster);
    }
}
