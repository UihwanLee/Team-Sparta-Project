using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class TruckManager : MonoBehaviour
{
    // 트럭 내 오브젝트들을 관리하는 스크립트

    [Header("BackgroundObject")]
    [SerializeField] private List<HorzScrollBackground> backgrounds = new List<HorzScrollBackground>();

    [Header("WheelObject")]
    [SerializeField] private List<MovingWheel> wheels = new List<MovingWheel>();

    [Header("BoxObject")]
    [SerializeField] private Transform boxTrans;
    [SerializeField] private List<Box> boxList = new List<Box>();

    public static bool isStuck;

    // Start is called before the first frame update
    private void Start()
    {
        InitBoxList();
    }

    private void Update()
    {
        
    }

    void InitBoxList()
    {
        foreach(Transform boxObj in boxTrans)
        {
            if(boxObj.name.Contains("Box"))
            {       
                if(boxObj.gameObject.GetComponent<Box>() == null)
                {
                    // box tag 변경
                    boxObj.gameObject.tag = TagData.TAG_BOX;

                    // box에 스크립트 추가
                    Box box = boxObj.gameObject.AddComponent<Box>();
                    box.SetBoxSetting();

                    // boxList에 추가
                    boxList.Add(box);
                }
            }
        }

    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == TagData.TAG_MONSTER)
        {
            // 몬스터와 충돌 시 배경을 서서히 멈춘다
            foreach (MovingWheel wheel in wheels)
            {
                wheel.IsStuck = true;
            }

            foreach (HorzScrollBackground bg in backgrounds)
            {
                bg.IsStuck = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == TagData.TAG_MONSTER)
        {
            // 몬스터와 충돌 시 배경을 서서히 멈춘다
            foreach (MovingWheel wheel in wheels)
            {
                wheel.IsStuck = false;
            }

            foreach (HorzScrollBackground bg in backgrounds)
            {
                bg.IsStuck = false;
            }
        }
    }
}
