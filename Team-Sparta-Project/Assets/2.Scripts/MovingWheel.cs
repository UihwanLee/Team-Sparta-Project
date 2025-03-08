using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWheel : MonoBehaviour
{
    // ���� ȸ�� �ִϸ��̼� ��ũ��Ʈ

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
            // ���ݹް� �ִٸ� ������ ����
            moveSpeed = Mathf.Lerp(moveSpeed, 0.0f, moveRate * Time.deltaTime);
        }
        else
        {
            // ���ݹ��� �ʴ´ٸ� ������ ����
            moveSpeed = Mathf.Lerp(moveSpeed, maxMoveSpeed, moveRate * Time.deltaTime);
        }
    }


    public bool IsStuck { get { return isStuck; } set { isStuck = value; } }
}
