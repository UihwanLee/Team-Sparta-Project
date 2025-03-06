using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    // 몬스터 컨트롤러 스크립트

    // 몬스터 정보
    [Header("Monster Info")]
    private int id;                                     // 몬스터 id
    private int type;                                   // 몬스터 타입
    private int spawnPath;                              // 몬스터 경로
    [SerializeField] private float moveSpeed;           // 몬스터 스피드
    [SerializeField] private Vector3 moveDir;           // 몬스터 이동 방향
    [SerializeField] private int hp;                    // 몬스터 체력.
    [SerializeField] private int damage;                // 몬스터 공격력

    [Header("Monster Part")]
    [SerializeField]
    private List<GameObject> monsterParts;

    private void Start()
    {

    }

    private void Update()
    {
        // 소환될 시 기본적으로 옆으로 이동
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    public void SetPath(int myPath, Collider2D myCollider, Collider2D[] ignorePath1, Collider2D[] ignorePath2)
    {
        // 주어진 경로 지정
        spawnPath = myPath;

        // 정한 path 외 다른 path 충돌처리 무시
        IgnoreCollisionWithPaths(myCollider, ignorePath1);
        IgnoreCollisionWithPaths(myCollider, ignorePath2);
    }

    private void IgnoreCollisionWithPaths(Collider2D myCollider, Collider2D[] ignorePath)
    {
        foreach (Collider2D pathCollider in ignorePath)
        {
            if (pathCollider != null && myCollider != null)
            {
                Physics2D.IgnoreCollision(myCollider, pathCollider);
            }
        }
    }

    public void SetID(int _id) { id = _id; }
    public int GetID() { return id; }
}
