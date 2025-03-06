using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    // ���� ��ȯ ��ũ��Ʈ

    [SerializeField]
    private float spawnTime;
    [SerializeField]
    private GameObject monsterPrefab;

    private float curTime;

    // Start is called before the first frame update
    void Start()
    {
        curTime = spawnTime;
        StartCoroutine(SpawnMonsters());
    }

    IEnumerator SpawnMonsters()
    {
        while (true)
        {
            // Ư�� �ֱ� ���� ���� ��ȯ
            yield return new WaitForSeconds(spawnTime);

            Instantiate(monsterPrefab, this.transform.position, Quaternion.identity);   // ���� ��ȯ
        }
    }
}
