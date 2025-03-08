using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;              // 총알 속도
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GunController gunController;

    // 화면 밖 범위 설정 (상하, 좌우)
    private float topBoundary = 10f;      
    private float bottomBoundary = -10f;  
    private float rightBoundary = 5f;     
    private float leftBoundary = -5f;    

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); 
    }

    public void InitBullet(GunController _gunController, float _bulletSpeed)
    {
        bulletSpeed = _bulletSpeed;
        gunController = _gunController;
        this.gameObject.SetActive(false);
    }

    public void Fire(Vector2 direction)
    {
        Vector2 moveDirection = direction.normalized;
        rb.velocity = moveDirection * bulletSpeed;
    }

    private void Update()
    {
        if(CheckOutOfScreen())
        {
            // 화면 밖으로 나가면 비활성화
            gunController.ReturnBulletToPool(gameObject);
        }
    }

    bool CheckOutOfScreen()
    {
        // 화면 밖으로 나갔는지 체크
        Vector3 position = transform.position;

        if (position.y > topBoundary || position.y < bottomBoundary || position.x > rightBoundary || position.x < leftBoundary)
        {
            return true; 
        }

        return false; 
    }
}
