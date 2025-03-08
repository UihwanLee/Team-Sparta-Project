using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorzScrollBackground : MonoBehaviour
{
    // ���� ������ �����̴� �ִϸ��̼� ��ũ��Ʈ

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
        // ������ üũ
        CheckStuck();

        // ���� ������ ��� �̵�
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        // ����� ī�޶� ���� �� ����� �缳��
        if(transform.position.x <= -scrollAmount)
        {
            this.transform.position = target.position - (moveDir * scrollAmount * 2);
        }
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
