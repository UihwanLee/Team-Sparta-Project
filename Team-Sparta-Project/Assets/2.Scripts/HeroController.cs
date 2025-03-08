using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroController : MonoBehaviour
{
    // 히어로 컨트롤러 스크립트

    // 몬스터 정보
    [Header("Hero Info")]
    [SerializeField] private SpriteRenderer heroSpriteRender;       // 히어로 이미지
    [SerializeField] private int maxHp;                             // 히어로 최대 최력
    [SerializeField] private int hp;                                // 히어로 체력.
    [SerializeField] private int damage;                            // 히어로 공격력

    [SerializeField] private bool isAttacking;
    [SerializeField] private bool isDamage;
    [SerializeField] private bool isDead;

    [SerializeField] private GameObject hpBar;                        // 히어로 hp바
    [SerializeField] private GunController gunController;             // 총 컨트롤러

    public Slider hpSlider;

    // Start is called before the first frame update
    void Start()
    {
        InitValue();
    }

    private void InitValue()
    {
        maxHp = GameData.Instance.HeroMaxHP;
        hp = maxHp;
    }

    public void Damage(int _dmg)
    {
        // hp가 0 이하 시 파괴
        if (hp - _dmg <= 0)
        {
            hp = 0;
            Dead();
            return;
        }

        // 공격 받는 모션
        hpBar.SetActive(true);

        // 데미지 적용
        hp -= _dmg;
        StartCoroutine(DamageEffect());

        // Hp 슬라이더 업데이트
        UpdateHpSlider();
    }

    private IEnumerator DamageEffect()
    {
        if (heroSpriteRender != null)
        {
            Color damageColor;
            if (ColorUtility.TryParseHtmlString("#E0E0E0", out damageColor))
            {
                heroSpriteRender.color = damageColor;
                yield return new WaitForSeconds(0.1f);
                heroSpriteRender.color = Color.white;
            }
        }

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

        Destroy(gameObject);
    }

    public bool IsDamage { get { return isDamage; } set { isDamage = value; } }
}
