using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorzScrollBackground : MonoBehaviour
{
    // ���� ������ �����̴� �ִϸ��̼� ��ũ��Ʈ

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
        // ���� ������ ��� �̵�
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        // ����� ī�޶� ���� �� ����� �缳��
        if(transform.position.x <= -scrollAmount)
        {
            Debug.Log(target.position);
            Debug.Log(moveDir * scrollAmount * 2);
            this.transform.position = target.position - (moveDir * scrollAmount * 2);
        }
    }
}
