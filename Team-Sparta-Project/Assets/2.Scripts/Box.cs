using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Box : MonoBehaviour
{
    [Header("Box Info")]
    [SerializeField] private int maxHp;
    [SerializeField] private int hp;
    [SerializeField] private bool isDamage;
    [SerializeField] private bool isDestroy;
    [SerializeField] private GameObject hpBar;
    [SerializeField] private SpriteRenderer boxSpriteRender;

    private GameObject boxSprite;
    private GameObject hpPannel;

    [Header("Manager")]
    [SerializeField] private TruckManager truckManager;

    public void SetBoxSetting(TruckManager _truckManager, int maxHP)
    {
        if(this.transform.childCount >= 2)
        {
            // Box Sprite ����
            boxSprite = this.transform.GetChild(0).GetChild(0).gameObject;

            // Hp Pannel ����
            hpPannel = this.transform.GetChild(1).gameObject;

            // Hp Slider ����
            hpBar = hpPannel.transform.GetChild(0).GetChild(0).gameObject;

            // SpriteRender ����
            boxSpriteRender = this.transform.GetChild(0).GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        }

        maxHp = maxHP;
        hp = maxHp;

        truckManager = _truckManager;
    }

    public void Damage(int _dmg)
    {
        // hp�� 0 ���� �� �ı�
        if(hp - _dmg <= 0)
        {
            hp = 0;
            Destroy();
            return;
        }

        // ���� �޴� ���
        hpPannel.SetActive(true);

        // ������ ����
        hp -= _dmg;
        StartCoroutine(DamageEffect());

        // Hp �����̴� ������Ʈ
        UpdateHpSlider();
    }

    private IEnumerator DamageEffect()
    {
        if(boxSpriteRender != null)
        {
            Color damageColor;
            if (ColorUtility.TryParseHtmlString("#E0E0E0", out damageColor))
            {
                boxSpriteRender.color = damageColor; 
                yield return new WaitForSeconds(0.1f);
                boxSpriteRender.color = Color.white; 
            }
        }

    }

    private void UpdateHpSlider()
    {
        if (hpBar != null)
        {
            Slider hpSlider = hpBar.GetComponentInChildren<Slider>(); 
            hpSlider.value = (float)hp / maxHp; // HP ���� 0~1 ������ ��ȯ
        }
    }

    private void Destroy()
    {
        if(truckManager == null)
        {
            Debug.LogError("There is no truckManager");
            return;
        }

        isDestroy = true;

        truckManager.OnBoxDestroyed(this);

        Destroy(gameObject);
    }

    public void Drop(Vector3 targetPosition, float dropSpeed)
    {
        StartCoroutine(DropCoroutine(targetPosition, dropSpeed));
    }

    IEnumerator DropCoroutine(Vector3 targetPosition, float dropSpeed)
    {
        this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, dropSpeed * Time.deltaTime);
        yield return new WaitForSeconds(0.1f);
        this.gameObject.transform.position = targetPosition; // ���� ��ġ ����
    }

    public int Hp { get { return hp; } set { hp = value; }}

    public bool IsDamage { get { return isDamage; } set { isDamage = value; } }
    public bool IsDestroy() { return isDestroy; }
}
