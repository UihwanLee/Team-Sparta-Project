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
    }

    // Update is called once per frame
    void Update()
    {
        curTime -= Time.deltaTime;

        if(curTime < 0)
        {
            Instantiate(monsterPrefab, this.transform.position, Quaternion.identity);   // ���� ��ȯ
            curTime = spawnTime;
        }
    }
}
