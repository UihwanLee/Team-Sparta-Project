using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    // ���� ��Ʈ�ѷ� ��ũ��Ʈ

    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private Vector3 moveDir;

    private int spawnPath;      // �־��� path ���

    private void Start()
    {

    }

    private void Update()
    {
        // ��ȯ�� �� �⺻������ ������ �̵�
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    public void SetPath(int myPath, Collider2D myCollider, Collider2D[] ignorePath1, Collider2D[] ignorePath2)
    {
        // �־��� ��� ����
        spawnPath = myPath;

        // ���� path �� �ٸ� path �浹ó�� ����
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
