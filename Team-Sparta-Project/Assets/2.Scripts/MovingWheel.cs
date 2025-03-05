using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWheel : MonoBehaviour
{
    // ���� ȸ�� �ִϸ��̼� ��ũ��Ʈ

    [SerializeField]
    private float moveSpeed;

    private void Update()
    {
        this.transform.Rotate(new Vector3(0f, 0f, -moveSpeed) * Time.deltaTime);
    }
}
