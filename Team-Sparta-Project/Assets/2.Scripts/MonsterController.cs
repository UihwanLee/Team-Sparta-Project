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
    [SerializeField] private string layer;                           // ���� ���
    [SerializeField] private float speed;                           // ���� ���ǵ�
    [SerializeField] private Vector3 moveDir;                       // ���� �̵� ����
    [SerializeField] private int hp;                                // ���� ü��.
    [SerializeField] private int damage;                            // ���� ���ݷ�
    [SerializeField] private List<SpriteRenderer> spriteRenderers;  // ���� �̹���

    [Header("Monster BT")]
    [SerializeField] private Selector root;
    [SerializeField] private bool isMonsterAround;

    [Header("Monster Physics")]
    private Rigidbody2D rb;
    [SerializeField] public float jumpForce = 5f;
    [SerializeField] private bool isJumping = false;
    [SerializeField] Collider2D myCollider;
    Collider2D[] hitColliders;

    public float climbHeight = 4.0f;      // �ö� ����
    public float slideSpeed = 1f;       // �̲������� �ӵ�
    [SerializeField] private bool isClimbing = false;
    [SerializeField] private bool isSliding = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isMonsterAround = false;
        //InitBehaviorTree();
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

    private void Update()
    {
        // BehaviorTree ����
        if(isSliding)
        {
            SlideDown();
        }
        else if (isClimbing)
        {
            if(isJumping==false) ClimbUp();
        }
        else
        {
            Moving();
        }
        //root.Run();
    }

    void Moving()
    {
        //if(isSliding) { Debug.Log("���� ���� ����!"); }
        transform.position += moveDir * speed * Time.deltaTime;
        //Vector2 moveDirection = Vector2.left;  // �������� �̵�
        //rb.velocity = new Vector2(moveDirection.x * speed, rb.velocity.y);
    }

    // �̲������� ����
    void SlideDown()
    {
        // �浹 �� �ڿ� �ִ� ���Ͱ� �з��� �� �ֵ��� �ӵ��� ������ �� ����
        //transform.position += moveDir * speed * -0.1f * Time.deltaTime;

        StartCoroutine(SlideDownCoroutine());

        //Vector2 pushDirection = Vector2.right;
        //rb.AddForce(pushDirection * speed, ForceMode2D.Force);
    }

    private IEnumerator SlideDownCoroutine()
    {
        rb.velocity = new Vector2(1.0f, rb.velocity.y);

        yield return new WaitForSeconds(1.0f); // �и��� �ð�

        isSliding = false;
    }

    // �ö󰡴� ����
    void ClimbUp()
    {
        StartCoroutine(ClimbUpCoroutine());
    }

    private IEnumerator ClimbUpCoroutine()
    {
        isJumping = true;

        // ������ ���� ���� �� �浹 ����
        hitColliders = Physics2D.OverlapCircleAll(transform.position, 1.5f, LayerMask.NameToLayer(layer));

        rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        yield return new WaitForSeconds(0.5f); // ���� ���� �ð�

        isClimbing = false;

        yield return new WaitForSeconds(5.0f); // ��Ÿ��

        isJumping = false; // ������ ������ �ٽ� ���� ����
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer(layer))
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
            //Debug.Log("���");

            //if (isSliding) isSliding = false; 
        }
    }

    private int GetHitPosition(Collision2D collision)
    {
        float otherMonsterX = collision.transform.position.x;
        float otherMonsterY = collision.transform.position.y;
        float myX = transform.position.x;
        float myY = transform.position.y;

        //if(otherMonsterY - myY > 0) Debug.Log(otherMonsterY - myY);

        if (otherMonsterY - myY > 1.0f)
        {
            return 0;
        }
        else if(myX - otherMonsterX > 0.5f)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }

    public void SetPath(string myLayer, Collider2D _collider, Collider2D[] ignorePath1, Collider2D[] ignorePath2)
    {
        if (myLayer == "") Debug.LogError("There is no path!");

        // �־��� ��� ����
        layer = myLayer;

        // ���� path �� �ٸ� path �浹ó�� ����
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

    //    // ���� ����
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
    //    Vector2 moveDirection = Vector2.left;  // �������� �̵�
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
