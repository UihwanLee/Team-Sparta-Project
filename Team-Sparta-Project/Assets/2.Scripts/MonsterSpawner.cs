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

    // 3���� ��� Collider
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
            // Ư�� �ֱ� ���� ���� ��ȯ
            yield return new WaitForSeconds(spawnTime);

            var monster = Instantiate(monsterPrefab, this.transform.position, Quaternion.identity);   // ���� ��ȯ

            if(monster != null)
            {
                SetMonsterPath(monster);
            }
        }
    }

    private void SetMonsterPath(GameObject monster)
    {
        // ��� 3�� �� 1���� �������� ����
        int randomPathIndex = Random.Range(0, 3);  // 0, 1, 2 �� �ϳ��� ���� (Path1, Path2, Path3)

        Collider2D myCollider = monster.GetComponent<Collider2D>();
        Collider2D[] collider1 = null;
        Collider2D[] collider2 = null;

        Debug.Log("���ð��: " + randomPathIndex);

        // ��� ����
        switch (randomPathIndex)
        {
            case 0:  // Path1 ����
                collider1 = pathColliders[0];
                collider2 = pathColliders[1]; // ������ 2�� ��ο��� ����
                break;

            case 1:  // Path2 ����
                collider1 = pathColliders[1];
                collider2 = pathColliders[2]; // ������ 2�� ��ο��� ����
                break;

            case 2:  // Path3 ����
                collider1 = pathColliders[2];
                collider2 = pathColliders[0]; // ������ 2�� ��ο��� ����
                break;
        }

        // ���Ϳ��� ��� ����
        MonsterController monsterController = monster.GetComponent<MonsterController>();
        monsterController.SetPath(randomPathIndex, myCollider, collider1, collider2);
    }
}
