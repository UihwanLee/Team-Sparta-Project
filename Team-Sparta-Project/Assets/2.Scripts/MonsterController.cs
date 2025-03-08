using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private int maxHp;                             // 몬스터 최대 체력
    [SerializeField] private int hp;                                // 몬스터 체력
    [SerializeField] private int damage;                            // 몬스터 공격력
    [SerializeField] private List<SpriteRenderer> spriteRenderers;  // 몬스터 이미지
    [SerializeField] private GameObject hpBar;                      // 몬스터 hp바

    [Header("Monster Physics")]
    [SerializeField] public float jumpForce;        // 점프 속도
    [SerializeField] public float slideSpeed;       // 미끄러지는 속도
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

    // 기본 동작
    void Move()
    {
        transform.position += (Vector3)(moveDir * speed * Time.deltaTime);
    }

    // 미끄러지는 동작
    void Slide()
    {
        rb.velocity = new Vector2(3.0f, rb.velocity.y);
    }

    // 점프 동작
    void ClimbUp()
    {
        StartCoroutine(ClimbUpCoroutine());
    }

    private IEnumerator ClimbUpCoroutine()
    {
        // 점프 코루틴
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
            // 총알 맞을 시 데미지를 입는다.
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
        // 공격이 가능할 시 공격
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

        // 데미지 적용 주체 판별
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

        // 공격 애니메이션 실행
        anim.SetBool("IsAttacking", isAttacking);

        yield return new WaitForSeconds(0.5f);

        isAttacking = false;
        anim.SetBool("IsAttacking", isAttacking);
    }

    private void Damage()
    {
        // 받는 몬스터마다 데미지를 달리한다.
        int damage = Random.Range(GameData.Instance.HeroMinDamage, GameData.Instance.HeroMaxDamage);

        // hp가 0 이하 시 파괴
        if (hp - damage <= 0)
        {
            hp = 0;
            Dead();
            return;
        }

        // 공격 받는 모션
        hpBar.SetActive(true);

        // 데미지 적용
        hp -= damage;
        StartCoroutine(DamageEffect());
        StartCoroutine(DamageShow(damage));

        // Hp 슬라이더 업데이트
        UpdateHpSlider();
    }

    private IEnumerator DamageEffect()
    {
        // 색상 변경
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
            hpSlider.value = (float)hp / maxHp; // HP 값을 0~1 범위로 변환
        }
    }

    private void Dead()
    {
        isDead = true;

        StartCoroutine(DeadCoroutine());    
    }

    private IEnumerator DeadCoroutine()
    {
        // 죽는 애니메이션 실행
        anim.SetBool("IsIdle", false);
        anim.SetBool("IsDead", isDead);
        
        yield return new WaitForSeconds(2.0f);

        // PoolManager에게 죽었다고 알림
        MonsterPoolManager.Instance.ReturnMonsterToPool(this.level, this.id);
    }

    private void HandleCollision(Collision2D collision)
    {
        // 충돌 객체의 위치에 따라 동작 수행
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
