using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    // Gun 관리하는 스크립트

    [SerializeField] private GameObject bulletPrefab;   // 총알 프리팹
    [SerializeField] private float fireRate;            // 발사 간격
    [SerializeField] private bool isFiring;             // 발사 중인지 확인
    [SerializeField] private int maxBullets;            // 최대 총알 개수
    [SerializeField] private float bulletSpeed;         // 총알 스피드
    [SerializeField] private Transform gunMuzzle;       // 총알 총구 위치
    [SerializeField] private int bulletDamage;          // 총알 데미지

    [SerializeField] private List<GameObject> bulletList;  // 오브젝트 풀


    // Start is called before the first frame update
    void Start()
    {
        InitValue();
        InitBullet();
    }

    private void InitValue()
    {
        // GUN 정보 초기화
        bulletPrefab = GameData.Instance.BulletPrefab;
        fireRate = GameData.Instance.GunFireRate;
        maxBullets = GameData.Instance.GunMaxBullets;
        bulletSpeed = GameData.Instance.BulletSpeed;
    }

    private void InitBullet()
    {
        // 오브젝트 풀 초기화
        bulletList = new List<GameObject>();

        // 최대 개수만큼 총알 생성
        for (int i = 0; i < maxBullets; i++)
        {
            // 총알마다 랜덤한 데미지 값을 갖는다.
            bulletDamage = Random.Range(GameData.Instance.HeroMinDamage, GameData.Instance.HeroMaxDamage);

            GameObject bullet = Instantiate(bulletPrefab, gunMuzzle.position, Quaternion.identity);
            bullet.SetActive(false);  
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.InitBullet(this, bulletSpeed); 
            bulletList.Add(bullet);
        }
    }

    // Update is called once per frame
    void Update()
    {
        TryFireBullet();
    }

    private void TryFireBullet()
    {
        // 발사 시도
        if(!isFiring)
        {
            FireBullets();
        }
    }

    private void FireBullets()
    {
        // 발사 코루틴 시작
        StartCoroutine(FireBulletsCoroutine());
    }

    IEnumerator FireBulletsCoroutine()
    {
        isFiring = true;

        yield return new WaitForSeconds(1.0f);

        // 총알 발사
        Fire();

        // 발사 간격만큼 대기
        yield return new WaitForSeconds(fireRate);

        isFiring = false;
    }

    private void Fire()
    {
        // 총알 POOL에서 총알 가져와 발사
        GameObject bullet = GetBulletFromPool();

        if(bullet == null) { Debug.Log("총알이 없습니다!"); return; }

        if (bullet != null)
        {
            bullet.SetActive(true);

            // 위치 조정
            bullet.transform.position = gunMuzzle.position;
            bullet.transform.rotation = transform.rotation;

            // 발사
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.Fire(gunMuzzle.right);
        }
    }

    public GameObject GetBulletFromPool()
    {
        // 주어진 총알 폴에서 사용할 수 있는 총알 반환
        if (bulletList == null) { Debug.LogError("That bullet list does not exist!"); return null; }

        for (int i = 0; i < bulletList.Count; i++)
        {
            if (bulletList[i].gameObject.activeSelf == false)
            {
                bulletList[i].SetActive(true);
                return bulletList[i];
            }
        }

        return null;
    }

    public void ReturnBulletToPool(GameObject _bullet)
    {
        // 총알을 비활성화하여 풀에 반환
        _bullet.SetActive(false); 
    }
}
