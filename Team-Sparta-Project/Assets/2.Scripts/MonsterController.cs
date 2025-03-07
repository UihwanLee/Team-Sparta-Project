using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class MonsterController : MonoBehaviour
{
    // 몬스터 컨트롤러 스크립트

    // 몬스터 정보
    [Header("Monster Info")]
    [SerializeField] private int id;                                // 몬스터 id
    [SerializeField] private string title;                          // 몬스터 타이틀
    [SerializeField] private int level;                             // 몬스터 레벨
    [SerializeField] private string layer;                          // 몬스터 경로
    [SerializeField] private float speed;                           // 몬스터 스피드
    [SerializeField] private Vector3 moveDir;                       // 몬스터 이동 방향
    [SerializeField] private int hp;                                // 몬스터 체력.
    [SerializeField] private int damage;                            // 몬스터 공격력
    [SerializeField] private List<SpriteRenderer> spriteRenderers;  // 몬스터 이미지

    [SerializeField] private int pathLevel;                        // 몬스터 경로 레벨

    [Header("Monster BT")]
    [SerializeField] private Selector root;
    [SerializeField] private bool isMonsterAround;

    [Header("Monster Physics")]
    private Rigidbody2D rb;
    [SerializeField] public float jumpForce = 5.0f;
    [SerializeField] private bool isJumping = false;

    public float climbHeight = 4.0f;      // 올라갈 높이
    public float slideSpeed = 1f;       // 미끄러지는 속도
    [SerializeField] private bool isClimbing = false;
    [SerializeField] private bool isSliding = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isMonsterAround = false;
        //InitBehaviorTree();
    }

    private void Update()
    {
        // BehaviorTree 동작
        if (isSliding)
        {
            Sliding();
        }
        else if (isClimbing)
        {
            if (isJumping == false) ClimbUp();
        }
        else
        {
            Moving();
        }
    }

    void Moving()
    {
        transform.position += moveDir * speed * Time.deltaTime;
    }

    // 미끄러지는 동작
    void Sliding()
    {
        rb.velocity = new Vector2(3.0f, rb.velocity.y);
    }

    // 올라가는 동작
    void ClimbUp()
    {
        StartCoroutine(ClimbUpCoroutine());
    }

    private IEnumerator ClimbUpCoroutine()
    {
        isJumping = true;

        transform.position += moveDir * speed * Time.deltaTime;

        yield return new WaitForSeconds(0.5f); // 점프 유지 시간

        rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        yield return new WaitForSeconds(0.5f); // 점프 유지 시간

        isClimbing = false;

        yield return new WaitForSeconds(3.0f); // 쿨타임

        isJumping = false; // 점프가 끝나면 다시 점프 가능
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(layer))
        {
            int hitCondition = GetHitPosition(collision);

            switch (hitCondition)
            {
                case 0:
                    isSliding = true;
                    isClimbing = false;
                    break;
                case 1:
                    if (isJumping == false && isSliding == false)
                    {
                        isClimbing = true;
                    }
                    isSliding = false;
                    break;
                case -1:
                    break;
                default:
                    isSliding = false;
                    isClimbing = false;
                    break;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(layer))
        {
            if(collision.transform.position.y > transform.position.y)
            {
                isSliding = false;
            }
        }
    }

    private int GetHitPosition(Collision2D collision)
    {
        float otherMonsterX = collision.transform.position.x;
        float otherMonsterY = collision.transform.position.y;
        float myX = transform.position.x;
        float myY = transform.position.y;

        if (otherMonsterY - myY > 1.0f) return 0;
        else if(myX - otherMonsterX > 0.5f) return 1;
        else return -1;
    }

    public void SetPath(string myLayer, Collider2D _collider, Collider2D[] ignorePath1, Collider2D[] ignorePath2)
    {
        if (myLayer == "") Debug.LogError("There is no path!");

        // 주어진 경로 지정
        layer = myLayer;

        // 정한 path 외 다른 path 충돌처리 무시
        IgnoreCollisionList(_collider, ignorePath1);
        IgnoreCollisionList(_collider, ignorePath2);
    }

    private void IgnoreCollisionList(Collider2D _collider, Collider2D[] ignoreColliders)
    {
        foreach (Collider2D collider in ignoreColliders)
        {
            if (collider != null && _collider != null)
            {
                Physics2D.IgnoreCollision(collider, _collider);
            }
        }
    }

    private void ResetCollisionList(Collider2D _collider, Collider2D[] ignoreColliders)
    {
        foreach (Collider2D collider in ignoreColliders)
        {
            if (collider != null && _collider != null)
            {
                Physics2D.IgnoreCollision(_collider, collider, true);
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

    //private void InitBehaviorTree()
    //{
    //    root = new Selector();
    //    Sequence actionSequence = new Sequence();
    //    Sequence jumpSequence = new Sequence();
    //    Sequence attackSequence = new Sequence();
    //    Sequence chaseSequence = new Sequence();

    //    Condition isPlayerAround = new Condition(IsPlayerAround);
    //    Condition isMonsterAround = new Condition(IsMonsterAround);

    //    Action jumpAction = new Action(TryJumping);
    //    Action attackAction = new Action(Attack);
    //    Action chaseAction = new Action(Chase);

    //    root.AddChild(actionSequence);
    //    root.AddChild(chaseSequence);

    //    actionSequence.AddChild(jumpSequence);
    //    actionSequence.AddChild(attackSequence);
    //    chaseSequence.AddChild(chaseAction);

    //    jumpSequence.AddChild(isMonsterAround);
    //    jumpSequence.AddChild(jumpAction);
    //    attackSequence.AddChild(isPlayerAround);
    //    attackSequence.AddChild(attackAction);

    //    root.Run();
    //}

    //private NodeState TryJumping()
    //{
    //    if (isJumping == false)
    //    {
    //        return Jumping();
    //    }
    //    return NodeState.FAIL;
    //}

    //private NodeState Jumping()
    //{
    //    //Debug.Log("Jummping!");

    //    // 점프 실행
    //    StartCoroutine(SmoothJump());
    //    return NodeState.SUCCESS;
    //}

    //private NodeState Attack()
    //{
    //    Debug.Log("Attack Player!");
    //    return NodeState.SUCCESS;
    //}

    //private NodeState Chase()
    //{
    //    //Debug.Log("Chase Player!");
    //    Vector2 moveDirection = Vector2.left;  // 왼쪽으로 이동
    //    rb.velocity = new Vector2(moveDirection.x * speed, rb.velocity.y);
    //    return NodeState.SUCCESS;
    //}

    //private bool IsPlayerAround()
    //{
    //    return false;
    //}

    //private bool IsMonsterAround()
    //{
    //    return isJumping ? true : isMonsterAround;
    //}
}
