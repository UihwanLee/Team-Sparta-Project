using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class BoxData
{
    public const string boxTag = "Box";
}

public class TruckManager : MonoBehaviour
{
    // 트럭 내 오브젝트들을 관리하는 스크립트

    [Header("WheelObject")]
    [SerializeField] private List<MovingWheel> wheels = new List<MovingWheel>();

    [Header("BoxObject")]
    [SerializeField] private Transform boxTrans;
    [SerializeField] private List<Box> boxList = new List<Box>();

    // Start is called before the first frame update
    void Start()
    {
        InitBoxList();
    }

    private void Update()
    {
        CheckDamage();
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
                    boxObj.gameObject.tag = BoxData.boxTag;

                    // box에 스크립트 추가
                    Box box = boxObj.gameObject.AddComponent<Box>();
                    box.SetBoxSetting();

                    // boxList에 추가
                    boxList.Add(box);
                }
            }
        }

    }

    private void CheckDamage()
    {
        bool isDamage = false;

        // 데미지 검사
        foreach (Box box in boxList)
        {
            if (box.IsDamage)
            {
                isDamage = true;
            }
        }

        // BoxList에서 Box가 공격받고 있다면 트럭을 서서히 멈춘다
        foreach (MovingWheel wheel in wheels)
        {
            wheel.IsDamage = isDamage;
        }
    }
}
