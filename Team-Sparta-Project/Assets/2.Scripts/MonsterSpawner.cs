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

    // 3가지 경로 Collider
    private List<Collider2D[]> pathColliders = new List<Collider2D[]>();

    // Start is called before the first frame update
    void Start()
    {
        curTime = spawnTime;
        InitPathColliders();
        StartCoroutine(SpawnMonsters());
    }

    private void InitPathColliders()
    {
        GameObject[] path1Objects = GameObject.FindGameObjectsWithTag("Path_1");
        GameObject[] path2Objects = GameObject.FindGameObjectsWithTag("Path_2");
        GameObject[] path3Objects = GameObject.FindGameObjectsWithTag("Path_3");

        pathColliders.Add(GetColliders(path1Objects));  
        pathColliders.Add(GetColliders(path2Objects));  
        pathColliders.Add(GetColliders(path3Objects));  
    }

    private Collider2D[] GetColliders(GameObject[] pathObjects)
    {
        Collider2D[] colliders = new Collider2D[pathObjects.Length];
        for (int i = 0; i < pathObjects.Length; i++)
        {
            colliders[i] = pathObjects[i].GetComponent<Collider2D>();
        }
        return colliders;
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
                SetMonsterPath(monster);
            }
        }
    }

    private void SetMonsterPath(GameObject monster)
    {
        // 경로 3개 중 1개를 랜덤으로 선택
        int randomPathIndex = Random.Range(0, 3);  // 0, 1, 2 중 하나를 선택 (Path1, Path2, Path3)

        Collider2D myCollider = monster.GetComponent<Collider2D>();
        Collider2D[] collider1 = null;
        Collider2D[] collider2 = null;

        Debug.Log("선택경로: " + randomPathIndex);

        // 경로 선택
        switch (randomPathIndex)
        {
            case 0:  // Path1 선택
                collider1 = pathColliders[0];
                collider2 = pathColliders[1]; // 나머지 2개 경로에서 선택
                break;

            case 1:  // Path2 선택
                collider1 = pathColliders[1];
                collider2 = pathColliders[2]; // 나머지 2개 경로에서 선택
                break;

            case 2:  // Path3 선택
                collider1 = pathColliders[2];
                collider2 = pathColliders[0]; // 나머지 2개 경로에서 선택
                break;
        }

        // 몬스터에게 경로 전달
        MonsterController monsterController = monster.GetComponent<MonsterController>();
        monsterController.SetPath(randomPathIndex, myCollider, collider1, collider2);
    }
}
