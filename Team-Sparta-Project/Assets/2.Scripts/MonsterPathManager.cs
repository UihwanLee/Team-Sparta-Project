using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPathManager : MonoBehaviour
{
    // ���� ��� ���� ��ũ��Ʈ

    private static MonsterPathManager instance = null;

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

    public static MonsterPathManager Instance
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

    // 3���� ��� Collider
    private List<Collider2D[]> pathColliders = new List<Collider2D[]>();

    private void Start()
    {
        InitPathColliders();
    }

    private void InitPathColliders()
    {
        GameObject[] path1Objects = GameObject.FindGameObjectsWithTag(TagData.TAG_PATH_1);
        GameObject[] path2Objects = GameObject.FindGameObjectsWithTag(TagData.TAG_PATH_2);
        GameObject[] path3Objects = GameObject.FindGameObjectsWithTag(TagData.TAG_PATH_3);

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

    public void SetMonsterPath(GameObject monster)
    {
        // ���� ��� ����

        // ��� 3�� �� 1���� �������� ����
        int path = Random.Range(0, 3);

        string pathLayer = "";

        Collider2D myCollider = monster.GetComponent<Collider2D>();
        Collider2D[] collider1 = null;
        Collider2D[] collider2 = null;

        // ��� ����
        switch (path)
        {
            case 0:  // Path1 ����
                collider1 = pathColliders[1];
                collider2 = pathColliders[2];
                pathLayer = "Monster Path1";
                monster.layer = LayerMask.NameToLayer("Monster Path1");
                break;

            case 1:  // Path2 ����
                collider1 = pathColliders[0];
                collider2 = pathColliders[2];
                pathLayer = "Monster Path2";
                monster.layer = LayerMask.NameToLayer("Monster Path2");
                break;

            case 2:  // Path3 ����
                collider1 = pathColliders[0];
                collider2 = pathColliders[1];
                pathLayer = "Monster Path3";
                monster.layer = LayerMask.NameToLayer("Monster Path3");
                break;
        }

        // ���Ϳ��� ��� ����
        MonsterController monsterController = monster.GetComponent<MonsterController>();
        monsterController.SetPath(pathLayer, myCollider, collider1, collider2);
        monsterController.AddOrderLayer((4-path)*10);
    }
}
