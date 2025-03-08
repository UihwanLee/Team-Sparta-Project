using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWheel : MonoBehaviour
{
    // 바퀴 회전 애니메이션 스크립트

    [SerializeField] private float maxMoveSpeed;
    [SerializeField] private float moveRate;
    [SerializeField] private float moveSpeed;
    private bool isStuck;

    private void Start()
    {
        maxMoveSpeed = GameData.Instance.WheelMaxMoveSpeed;
        moveRate = GameData.Instance.WheelMoveRate;
        isStuck = false;
    }

    private void Update()
    {
        CheckStuck();
        this.transform.Rotate(new Vector3(0f, 0f, -moveSpeed) * Time.deltaTime);
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
