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

    private GameObject boxSprite;
    private GameObject hpPannel;

    [Header("Manager")]
    [SerializeField] private TruckManager truckManager;

    public void SetBoxSetting(TruckManager _truckManager, int maxHP)
    {
        if(this.transform.childCount >= 2)
        {
            // Box Sprite 적용
            boxSprite = this.transform.GetChild(0).GetChild(0).gameObject;

            // Hp Pannel 적용
            hpPannel = this.transform.GetChild(1).gameObject;

            // Hp Slider 적용
            hpBar = hpPannel.transform.GetChild(0).GetChild(0).gameObject;
        }

        maxHp = maxHP;
        hp = maxHp;

        truckManager = _truckManager;
    }

    public void Damage(int _dmg)
    {
        // hp가 0 이하 시 파괴
        if(hp - _dmg <= 0)
        {
            hp = 0;
            Destroy();
            return;
        }

        // 공격 받는 모션
        hpPannel.SetActive(true);

        // 데미지 적용
        hp -= _dmg;

        // Hp 슬라이더 업데이트
        UpdateHpSlider();
    }

    private void UpdateHpSlider()
    {
        if (hpBar != null)
        {
            Slider hpSlider = hpBar.GetComponentInChildren<Slider>(); 
            hpSlider.value = (float)hp / maxHp; // HP 값을 0~1 범위로 변환
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
        this.gameObject.transform.position = targetPosition; // 최종 위치 보정
    }

    public int Hp { get { return hp; } set { hp = value; }}

    public bool IsDamage { get { return isDamage; } set { isDamage = value; } }
    public bool IsDestroy() { return isDestroy; }
}
