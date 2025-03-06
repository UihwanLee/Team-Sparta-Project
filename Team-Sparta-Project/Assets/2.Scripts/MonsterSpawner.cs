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

    // ���� ������Ʈ�� ������ ����Ʈ
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
            // Ư�� �ֱ� ���� ���� ��ȯ
            yield return new WaitForSeconds(spawnTime);

            var monster = Instantiate(monsterPrefab, this.transform.position, Quaternion.identity);   // ���� ��ȯ

            if(monster != null)
            {
                MonsterPathManager.Instance.SetMonsterPath(monster);
            }
        }
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
