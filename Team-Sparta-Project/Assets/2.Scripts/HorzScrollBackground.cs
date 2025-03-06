using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorzScrollBackground : MonoBehaviour
{
    // 수평 구조로 움직이는 애니메이션 스크립트

    [SerializeField]
    private Transform target;
    [SerializeField]
    private float scrollAmount;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private Vector3 moveDir;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 설정 방향대로 배경 이동
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        // 배경이 카메라 범위 내 벗어나면 재설정
        if(transform.position.x <= -scrollAmount)
        {
            Debug.Log(target.position);
            Debug.Log(moveDir * scrollAmount * 2);
            this.transform.position = target.position - (moveDir * scrollAmount * 2);
        }
    }
}
