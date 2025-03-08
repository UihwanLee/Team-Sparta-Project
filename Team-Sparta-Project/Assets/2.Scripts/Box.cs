using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [Header("Box Info")]
    [SerializeField] private int hp;
    [SerializeField] private bool isDamage;
    [SerializeField] private bool isDestroy;

    private GameObject boxSprite;
    private GameObject hpPannel;

    // Start is called before the first frame update
    void Start()
    {
        hp = 100;
    }

    public void SetBoxSetting()
    {
        if(this.transform.childCount >= 2)
        {
            // Box Sprite 적용
            boxSprite = this.transform.GetChild(0).GetChild(0).gameObject;

            // Hp Pannel 적용
            hpPannel = this.transform.GetChild(1).gameObject;
        }
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

        // 데미지 적용
        hp -= _dmg;
    }

    private void Destroy()
    {
        isDestroy = true;
    }


    public int Hp { get { return hp; } set { hp = value; }}

    public bool IsDamage { get { return isDamage; } set { isDamage = value; } }
    public bool IsDestroy() { return isDestroy; }
}
