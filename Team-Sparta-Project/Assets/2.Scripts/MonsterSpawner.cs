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
            Instantiate(monsterPrefab, this.transform.position, Quaternion.identity);   // 몬스터 소환
            curTime = spawnTime;
        }
    }
}
