using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    // ���� ��Ʈ�ѷ� ��ũ��Ʈ

    // ���� ����
    [Header("Monster Info")]
    [SerializeField] private int id;                                // ���� id
    [SerializeField] private string title;                          // ���� Ÿ��Ʋ
    [SerializeField] private int level;                             // ���� ����
    [SerializeField] private int spawnPath;                         // ���� ���
    [SerializeField] private float speed;                           // ���� ���ǵ�
    [SerializeField] private Vector3 moveDir;                       // ���� �̵� ����
    [SerializeField] private int hp;                                // ���� ü��.
    [SerializeField] private int damage;                            // ���� ���ݷ�
    [SerializeField] private List<SpriteRenderer> spriteRenderers;  // ���� �̹���

    private void Start()
    {

    }

    private void Update()
    {
        // ��ȯ�� �� �⺻������ ������ �̵�
        transform.position += moveDir * speed * Time.deltaTime;
    }

    public void SetPath(int myPath, Collider2D myCollider, Collider2D[] ignorePath1, Collider2D[] ignorePath2)
    {
        // �־��� ��� ����
        spawnPath = myPath;

        // ���� path �� �ٸ� path �浹ó�� ����
        IgnoreCollisionWithPaths(myCollider, ignorePath1);
        IgnoreCollisionWithPaths(myCollider, ignorePath2);
    }

    private void IgnoreCollisionWithPaths(Collider2D myCollider, Collider2D[] ignorePath)
    {
        foreach (Collider2D pathCollider in ignorePath)
        {
            if (pathCollider != null && myCollider != null)
            {
                Physics2D.IgnoreCollision(myCollider, pathCollider);
            }
        }
    }
    public void SetID(int _id) { id = _id; }
    public int GetID() { return id; }
    public void SetSprites(List<Sprite> _sprites)
    {
        if (this.spriteRenderers.Count == 0) { Debug.LogError("SetSprites: This list does not exist!"); }

        for (int i=0; i< _sprites.Count; i++)
        {
            this.spriteRenderers[i].sprite = _sprites[i];
        }
    }
    public void SetInfo(int _id, string _title, int _level, int _hp, int _damage, float _speed) { id = _id; title = _title; level = _level; hp = _hp; damage = _damage; speed = _speed; }

    public void AddOrderLayer(int amount)
    {
        foreach(SpriteRenderer render in spriteRenderers)
        {
            render.sortingOrder += amount;
        }
    }
}
