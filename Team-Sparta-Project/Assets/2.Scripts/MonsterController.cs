using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    // 좀비 컨트롤러 스크립트

    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private Vector3 moveDir;

    private int spawnPath;      // 주어진 path 경로

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
}
