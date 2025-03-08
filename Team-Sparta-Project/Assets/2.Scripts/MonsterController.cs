using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class MonsterController : MonoBehaviour
{
    // ���� ��Ʈ�ѷ� ��ũ��Ʈ

    // ���� ����
    [Header("Monster Info")]
    [SerializeField] private int id;                                // ���� id
    [SerializeField] private string title;                          // ���� Ÿ��Ʋ
    [SerializeField] private int level;                             // ���� ����
    [SerializeField] private string layer;                          // ���� ���
    [SerializeField] private float speed;                           // ���� ���ǵ�
    [SerializeField] private Vector3 moveDir;                       // ���� �̵� ����
    [SerializeField] private int hp;                                // ���� ü��.
    [SerializeField] private int damage;                            // ���� ���ݷ�
    [SerializeField] private List<SpriteRenderer> spriteRenderers;  // ���� �̹���

    [Header("Monster Physics")]
    [SerializeField] public float jumpForce;        // ���� �ӵ�
    [SerializeField] public float slideSpeed;       // �̲������� �ӵ�
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isClimbing;
    [SerializeField] private bool isSliding;
    [SerializeField] private bool isAttacking;
    [SerializeField] private Rigidbody2D rb;

    [Header("Monster Animation")]
    [SerializeField] private Animator anim;

    private void Awake()
    {
        InitValue();
    }

    private void Update()
    {
        if (isSliding)
            Slide();
        else if (isClimbing && !isJumping)
            ClimbUp();
        else
            Move();
    }

    private void InitValue()
    {
        rb = GetComponent<Rigidbody2D>();
        isJumping = false;
        isClimbing = false;
        isSliding = false;
        isAttacking = false;

        anim = GetComponent<Animator>();

        jumpForce = GameData.Instance.MonsterJumpForce;
    }

    // �⺻ ����
    void Move()
    {
        transform.position += (Vector3)(moveDir * speed * Time.deltaTime);
    }

    // �̲������� ����
    void Slide()
    {
        rb.velocity = new Vector2(3.0f, rb.velocity.y);
    }

    // ���� ����
    void ClimbUp()
    {
        StartCoroutine(ClimbUpCoroutine());
    }

    private IEnumerator ClimbUpCoroutine()
    {
        // ���� �ڷ�ƾ
        isJumping = true;
        transform.position += (Vector3)(moveDir * speed * Time.deltaTime);
        yield return new WaitForSeconds(0.5f);
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        yield return new WaitForSeconds(0.5f);
        isClimbing = false;
        yield return new WaitForSeconds(3.0f);
        isJumping = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == TagData.TAG_BOX)
        {
            TryAttack(collision.gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer(layer))
        {
            HandleCollision(collision);
        }
    }

    private void TryAttack(GameObject _boxObj)
    { 
        Box box = _boxObj.GetComponent<Box>();

        // ������ ������ �� ����
        if(!isAttacking && box.IsDestroy() == false)
        {
            Attack(box);
        }
    }

    private void Attack(Box _box)
    {
        StartCoroutine(AttackCoroutine(_box));
    }

    private IEnumerator AttackCoroutine(Box _box)
    {
        Debug.Log("����!");

        isAttacking = true;

        // ������ ����
        _box.Damage(this.damage);
        _box.IsDamage = true;

        // ���� �ִϸ��̼� ����
        anim.SetBool("IsAttacking", isAttacking);

        yield return new WaitForSeconds(0.5f);

        isAttacking = false;
        _box.IsDamage = false;
        anim.SetBool("IsAttacking", isAttacking);
    }

    private void HandleCollision(Collision2D collision)
    {
        // �浹 ��ü�� ��ġ�� ���� ���� ����
        switch (GetHitPosition(collision))
        {
            case 0:
                isSliding = true;
                isClimbing = false;
                break;
            case 1:
                if (!isJumping && !isSliding)
                    isClimbing = true;
                isSliding = false;
                break;
            default:
                isSliding = false;
                isClimbing = false;
                break;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(layer) && collision.transform.position.y > transform.position.y)
        {
            isSliding = false;
        }
    }

    private int GetHitPosition(Collision2D collision)
    {
        Vector2 otherPos = collision.transform.position;
        Vector2 myPos = transform.position;

        if (otherPos.y - myPos.y > 1.0f) return 0;
        if (myPos.x - otherPos.x > 0.5f) return 1;
        return -1;
    }

    public void SetPath(string myLayer, Collider2D _collider, Collider2D[] ignorePath1, Collider2D[] ignorePath2)
    {
        if (myLayer == "")
        {
            Debug.LogError("There is no path!");
            return;
        }

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
