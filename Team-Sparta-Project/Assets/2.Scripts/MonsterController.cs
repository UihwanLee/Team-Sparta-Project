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

    private void Start()
    {
        
    }

    private void Update()
    {
        // ��ȯ�� �� �⺻������ ������ �̵�
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }
}
