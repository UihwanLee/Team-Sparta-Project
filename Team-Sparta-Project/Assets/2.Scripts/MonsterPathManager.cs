using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PathTag
{
    public const string PATH_1 = "Path_1";
    public const string PATH_2 = "Path_2";
    public const string PATH_3 = "Path_3";
}

public static class PathLayer
{
    public const string PATH_1 = "Monster Path1";
    public const string PATH_2 = "Monster Path2";
    public const string PATH_3 = "Monster Path3";
}

public class MonsterPathManager : MonoBehaviour
{
    // 몬스터 경로 관리 스크립트

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

    // 3가지 경로 Collider
    private List<Collider2D[]> pathColliders = new List<Collider2D[]>();

    private void Start()
    {
        InitPathColliders();
    }

    private void InitPathColliders()
    {
        GameObject[] path1Objects = GameObject.FindGameObjectsWithTag(PathTag.PATH_1);
        GameObject[] path2Objects = GameObject.FindGameObjectsWithTag(PathTag.PATH_2);
        GameObject[] path3Objects = GameObject.FindGameObjectsWithTag(PathTag.PATH_3);

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
        // 몬스터 경로 설정

        // 경로 3개 중 1개를 랜덤으로 선택
        int path = Random.Range(0, 3);

        string pathLayer = "";

        Collider2D myCollider = monster.GetComponent<Collider2D>();
        Collider2D[] collider1 = null;
        Collider2D[] collider2 = null;

        // 경로 선택
        //switch (path)
        //{
        //    case 0:  // Path1 선택
        //        collider1 = pathColliders[1];
        //        collider2 = pathColliders[2];
        //        pathLayer = PathLayer.PATH_1;
        //        monster.layer = LayerMask.NameToLayer(PathLayer.PATH_1);
        //        break;

        //    case 1:  // Path2 선택
        //        collider1 = pathColliders[0];
        //        collider2 = pathColliders[2];
        //        pathLayer = PathLayer.PATH_2;
        //        monster.layer = LayerMask.NameToLayer(PathLayer.PATH_2);
        //        break;

        //    case 2:  // Path3 선택
        //        collider1 = pathColliders[0];
        //        collider2 = pathColliders[1];
        //        pathLayer = PathLayer.PATH_3;
        //        monster.layer = LayerMask.NameToLayer(PathLayer.PATH_3);
        //        break;
        //}

        collider1 = pathColliders[1];
        collider2 = pathColliders[2];
        pathLayer = PathLayer.PATH_1;
        monster.layer = LayerMask.NameToLayer(PathLayer.PATH_1);

        // 몬스터에게 경로 전달
        MonsterController monsterController = monster.GetComponent<MonsterController>();
        monsterController.SetPath(pathLayer, myCollider, collider1, collider2);
        monsterController.AddOrderLayer((4-path)*10);
    }
}
