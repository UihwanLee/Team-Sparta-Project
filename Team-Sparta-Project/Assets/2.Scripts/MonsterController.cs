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

    private void Start()
    {
        
    }

    private void Update()
    {
        // 소환될 시 기본적으로 옆으로 이동
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }
}
