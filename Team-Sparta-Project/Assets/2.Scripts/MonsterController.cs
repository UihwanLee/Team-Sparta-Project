using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private int maxHp;                             // ���� �ִ� ü��
    [SerializeField] private int hp;                                // ���� ü��
    [SerializeField] private int damage;                            // ���� ���ݷ�
    [SerializeField] private List<SpriteRenderer> spriteRenderers;  // ���� �̹���
    [SerializeField] private GameObject hpBar;                      // ���� hp��

    [Header("Monster Physics")]
    [SerializeField] public float jumpForce;        // ���� �ӵ�
    [SerializeField] public float slideSpeed;       // �̲������� �ӵ�
    [SerializeField] private Rigidbody2D rb;

    [Header("Monster State")]
    [SerializeField] private bool isDead;
    [SerializeField] private bool isDamaged;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isClimbing;
    [SerializeField] private bool isSliding;
    [SerializeField] private bool isAttacking;

    [Header("UI")]
    [SerializeField] private GameObject ui_Damage;
    [SerializeField] private TextMeshProUGUI ui_Damage_Text;
    private Color textColor;

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

        ui_Damage.SetActive(false);
        textColor = Color.white;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == TagData.TAG_BULLET)
        {
            // �Ѿ� ���� �� �������� �Դ´�.
            Damage();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == TagData.TAG_BOX || 
            collision.gameObject.tag == TagData.HERO)
        {
            TryAttack(collision.gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer(layer))
        {
            HandleCollision(collision);
        }
    }

    private void TryAttack(GameObject _obj)
    { 
        // ������ ������ �� ����
        if(!isAttacking)
        {
            Attack(_obj);
        }
    }

    private void Attack(GameObject _obj)
    {
        StartCoroutine(AttackCoroutine(_obj));
    }

    private IEnumerator AttackCoroutine(GameObject _obj)
    { 
        isAttacking = true;

        // ������ ���� ��ü �Ǻ�
        if(_obj.tag == TagData.TAG_BOX)
        {
            Box box = _obj.GetComponent<Box>();
            box.Damage(damage); box.IsDamage = true;
        }
        else if (_obj.tag == TagData.HERO)
        {
            HeroController heroController = _obj.GetComponent<HeroController>();
            heroController.Damage(damage); heroController.IsDamage = true;
        }
        else
        {
            Debug.LogError("This are no conflct");
        }

        // ���� �ִϸ��̼� ����
        anim.SetBool("IsAttacking", isAttacking);

        yield return new WaitForSeconds(0.5f);

        isAttacking = false;
        anim.SetBool("IsAttacking", isAttacking);
    }

    private void Damage()
    {
        // �޴� ���͸��� �������� �޸��Ѵ�.
        int damage = Random.Range(GameData.Instance.HeroMinDamage, GameData.Instance.HeroMaxDamage);

        // hp�� 0 ���� �� �ı�
        if (hp - damage <= 0)
        {
            hp = 0;
            Dead();
            return;
        }

        // ���� �޴� ���
        hpBar.SetActive(true);

        // ������ ����
        hp -= damage;
        StartCoroutine(DamageEffect());
        StartCoroutine(DamageShow(damage));

        // Hp �����̴� ������Ʈ
        UpdateHpSlider();
    }

    private IEnumerator DamageEffect()
    {
        // ���� ����
        if (spriteRenderers.Count != 0)
        {
            foreach(SpriteRenderer spriteRenderer in spriteRenderers)
            {
                Color damageColor;
                if (ColorUtility.TryParseHtmlString("#E0E0E0", out damageColor))
                {
                    spriteRenderer.color = damageColor;
                    yield return new WaitForSeconds(0.1f);
                    spriteRenderer.color = Color.white;
                }
            }
        }
    }

    IEnumerator DamageShow(int _dmg)
    {
        ui_Damage.SetActive(true);
        ui_Damage_Text.text = _dmg.ToString();
        yield return StartCoroutine(FadeText(ui_Damage_Text, 0, 1, 0.5f)); 
        yield return StartCoroutine(FadeText(ui_Damage_Text, 1, 0, 0.5f));
        ui_Damage.SetActive(false);
    }

    private IEnumerator FadeText(TextMeshProUGUI text, float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            textColor.a = alpha;
            text.color = textColor;
            yield return null;
        }
        textColor.a = endAlpha;
        text.color = textColor;
    }



    private void UpdateHpSlider()
    {
        if (hpBar != null)
        {
            Slider hpSlider = hpBar.GetComponentInChildren<Slider>();
            hpSlider.value = (float)hp / maxHp; // HP ���� 0~1 ������ ��ȯ
        }
    }

    private void Dead()
    {
        isDead = true;

        StartCoroutine(DeadCoroutine());    
    }

    private IEnumerator DeadCoroutine()
    {
        // �״� �ִϸ��̼� ����
        anim.SetBool("IsIdle", false);
        anim.SetBool("IsDead", isDead);
        
        yield return new WaitForSeconds(2.0f);

        // PoolManager���� �׾��ٰ� �˸�
        MonsterPoolManager.Instance.ReturnMonsterToPool(this.level, this.id);
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
    public void SetInfo(int _id, string _title, int _level, int _maxHp, int _damage, float _speed) { id = _id; title = _title; level = _level; maxHp = _maxHp; hp = _maxHp; damage = _damage; speed = _speed; }

    public void AddOrderLayer(int amount)
    {
        foreach(SpriteRenderer render in spriteRenderers)
        {
            render.sortingOrder += amount;
        }
    }
}
