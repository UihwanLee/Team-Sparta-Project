using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    // Gun �����ϴ� ��ũ��Ʈ

    [SerializeField] private GameObject bulletPrefab;   // �Ѿ� ������
    [SerializeField] private float fireRate;            // �߻� ����
    [SerializeField] private bool isFiring;             // �߻� ������ Ȯ��
    [SerializeField] private int maxBullets;            // �ִ� �Ѿ� ����
    [SerializeField] private float bulletSpeed;         // �Ѿ� ���ǵ�
    [SerializeField] private Transform gunMuzzle;       // �Ѿ� �ѱ� ��ġ
    [SerializeField] private int bulletDamage;          // �Ѿ� ������

    [SerializeField] private List<GameObject> bulletList;  // ������Ʈ Ǯ


    // Start is called before the first frame update
    void Start()
    {
        InitValue();
        InitBullet();
    }

    private void InitValue()
    {
        // GUN ���� �ʱ�ȭ
        bulletPrefab = GameData.Instance.BulletPrefab;
        fireRate = GameData.Instance.GunFireRate;
        maxBullets = GameData.Instance.GunMaxBullets;
        bulletSpeed = GameData.Instance.BulletSpeed;
    }

    private void InitBullet()
    {
        // ������Ʈ Ǯ �ʱ�ȭ
        bulletList = new List<GameObject>();

        // �ִ� ������ŭ �Ѿ� ����
        for (int i = 0; i < maxBullets; i++)
        {
            // �Ѿ˸��� ������ ������ ���� ���´�.
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
        // �߻� �õ�
        if(!isFiring)
        {
            FireBullets();
        }
    }

    private void FireBullets()
    {
        // �߻� �ڷ�ƾ ����
        StartCoroutine(FireBulletsCoroutine());
    }

    IEnumerator FireBulletsCoroutine()
    {
        isFiring = true;

        yield return new WaitForSeconds(1.0f);

        // �Ѿ� �߻�
        Fire();

        // �߻� ���ݸ�ŭ ���
        yield return new WaitForSeconds(fireRate);

        isFiring = false;
    }

    private void Fire()
    {
        // �Ѿ� POOL���� �Ѿ� ������ �߻�
        GameObject bullet = GetBulletFromPool();

        if(bullet == null) { Debug.Log("�Ѿ��� �����ϴ�!"); return; }

        if (bullet != null)
        {
            bullet.SetActive(true);

            // ��ġ ����
            bullet.transform.position = gunMuzzle.position;
            bullet.transform.rotation = transform.rotation;

            // �߻�
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.Fire(gunMuzzle.right);
        }
    }

    public GameObject GetBulletFromPool()
    {
        // �־��� �Ѿ� ������ ����� �� �ִ� �Ѿ� ��ȯ
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
        // �Ѿ��� ��Ȱ��ȭ�Ͽ� Ǯ�� ��ȯ
        _bullet.SetActive(false); 
    }
}
