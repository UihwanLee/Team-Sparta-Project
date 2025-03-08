using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorzScrollBackground : MonoBehaviour
{
    // 수평 구조로 움직이는 애니메이션 스크립트

    [SerializeField] private Transform target;
    [SerializeField] private float scrollAmount;
    [SerializeField] private float maxMoveSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveRate;
    [SerializeField] private Vector3 moveDir;
    [SerializeField] private bool isStuck;

    void Awake()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        // 데미지 체크
        CheckStuck();

        // 설정 방향대로 배경 이동
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        // 배경이 카메라 범위 내 벗어나면 재설정
        if(transform.position.x <= -scrollAmount)
        {
            this.transform.position = target.position - (moveDir * scrollAmount * 2);
        }
    }

    private void CheckStuck()
    {
        if (isStuck)
        {
            // 공격받고 있다면 서서히 감속
            moveSpeed = Mathf.Lerp(moveSpeed, 0.0f, moveRate * Time.deltaTime);
        }
        else
        {
            // 공격받지 않는다면 서서히 가속
            moveSpeed = Mathf.Lerp(moveSpeed, maxMoveSpeed, moveRate * Time.deltaTime);
        }
    }


    public bool IsStuck { get { return isStuck; } set { isStuck = value; } }
}
