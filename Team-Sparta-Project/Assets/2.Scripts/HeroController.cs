using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroController : MonoBehaviour
{
    // ����� ��Ʈ�ѷ� ��ũ��Ʈ

    // ���� ����
    [Header("Hero Info")]
    [SerializeField] private SpriteRenderer heroSpriteRender;       // ����� �̹���
    [SerializeField] private int maxHp;                             // ����� �ִ� �ַ�
    [SerializeField] private int hp;                                // ����� ü��.
    [SerializeField] private int damage;                            // ����� ���ݷ�

    [SerializeField] private bool isAttacking;
    [SerializeField] private bool isDamage;
    [SerializeField] private bool isDead;

    [SerializeField] private GameObject hpBar;                        // ����� hp��
    [SerializeField] private GunController gunController;             // �� ��Ʈ�ѷ�

    public Slider hpSlider;

    // Start is called before the first frame update
    void Start()
    {
        InitValue();
    }

    private void InitValue()
    {
        // ����� ���� �ʱ�ȭ
        maxHp = GameData.Instance.HeroMaxHP;
        hp = maxHp;
    }

    public void Damage(int _dmg)
    {
        // hp�� 0 ���� �� �ı�
        if (hp - _dmg <= 0)
        {
            hp = 0;
            Dead();
            return;
        }

        // ���� �޴� ���
        hpBar.SetActive(true);

        // ������ ����
        hp -= _dmg;
        StartCoroutine(DamageEffect());

        // Hp �����̴� ������Ʈ
        UpdateHpSlider();
    }

    private IEnumerator DamageEffect()
    {
        // Ÿ�� ���� ��ȭ
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
        // �����̴� value�� HP ������ ������Ʈ
        if (hpBar != null)
        {
            Slider hpSlider = hpBar.GetComponentInChildren<Slider>();
            hpSlider.value = (float)hp / maxHp;
        }
    }

    private void Dead()
    {
        // ����� ���� �� ������Ʈ �ı�
        isDead = true;

        Destroy(gameObject);
    }

    public bool IsDamage { get { return isDamage; } set { isDamage = value; } }
}
