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
        isDamage = false;
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
        // 공격 받는 모션


        if(!isDestroy)
        {
            hp -= _dmg;

            if(hp <= 0)
            {
                Destroy();
                return;
            }
        }
    }

    private void Destroy()
    {
        isDestroy = true;
    }
}
