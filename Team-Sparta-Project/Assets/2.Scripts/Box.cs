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
            // Box Sprite ����
            boxSprite = this.transform.GetChild(0).GetChild(0).gameObject;

            // Hp Pannel ����
            hpPannel = this.transform.GetChild(1).gameObject;
        }
    }

    public void Damage(int _dmg)
    {
        // ���� �޴� ���


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
