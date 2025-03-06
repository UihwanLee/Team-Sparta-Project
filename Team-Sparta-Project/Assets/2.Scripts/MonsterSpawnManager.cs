using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnManager : MonoBehaviour
{
    // ���� ��ȯ ��ũ��Ʈ

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
            // Ư�� �ֱ� ���� ���� ��ȯ
            yield return new WaitForSeconds(spawnTime);

            SpawnMonsterPoint();
        }
    }

    private void SpawnMonsterPoint()
    {
        GameObject monster = MonsterPoolManager.Instance.GetMonsterFromPool();

        if (monster == null)
        {
            Debug.Log("There are no objects available!");
        }

        // ��ġ ����
        monster.transform.position = spawnPos.position;

        // ��� ����
        MonsterPathManager.Instance.SetMonsterPath(monster);
    }

    private void SetMonsterType()
    {
        // ���� Ÿ�� ����
    }

    private void SetMonsterLayer()
    {
        // ���� ���̾ƿ� ����
    }
}
