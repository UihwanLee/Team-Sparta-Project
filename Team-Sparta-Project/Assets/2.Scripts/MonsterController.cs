using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    // 몬스터 컨트롤러 스크립트

    // 몬스터 정보
    [Header("Monster Info")]
    [SerializeField] private int id;                                // 몬스터 id
    [SerializeField] private string title;                          // 몬스터 타이틀
    [SerializeField] private int level;                             // 몬스터 레벨
    [SerializeField] private int spawnPath;                         // 몬스터 경로
    [SerializeField] private float speed;                           // 몬스터 스피드
    [SerializeField] private Vector3 moveDir;                       // 몬스터 이동 방향
    [SerializeField] private int hp;                                // 몬스터 체력.
    [SerializeField] private int damage;                            // 몬스터 공격력
    [SerializeField] private List<SpriteRenderer> spriteRenderers;  // 몬스터 이미지

    private void Start()
    {

    }

    private void Update()
    {
        // 소환될 시 기본적으로 옆으로 이동
        transform.position += moveDir * speed * Time.deltaTime;
    }

    public void SetPath(int myPath, Collider2D myCollider, Collider2D[] ignorePath1, Collider2D[] ignorePath2)
    {
        // 주어진 경로 지정
        spawnPath = myPath;

        // 정한 path 외 다른 path 충돌처리 무시
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
