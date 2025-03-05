using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWheel : MonoBehaviour
{
    // 바퀴 회전 애니메이션 스크립트

    [SerializeField]
    private float moveSpeed;

    private void Update()
    {
        this.transform.Rotate(new Vector3(0f, 0f, -moveSpeed) * Time.deltaTime);
    }
}
