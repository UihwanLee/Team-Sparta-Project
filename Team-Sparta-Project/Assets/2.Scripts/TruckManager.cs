using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

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

    private int boxMaxHP;
    private float boxDropSpeed;

    public static bool isStuck;

    // Start is called before the first frame update
    private void Start()
    {
        InitValue();
        InitBoxList();
    }

    void InitValue()
    {
        boxMaxHP = GameData.Instance.BoxMaxHP;
        boxDropSpeed = GameData.Instance.BoxDropSpeed;
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

                    // box에 컴포넌트 추가
                    Box box = boxObj.gameObject.AddComponent<Box>();
                   
                    // box 세팅
                    box.SetBoxSetting(this, boxMaxHP);
                    
                    // boxList에 추가
                    boxList.Add(box);
                }
            }
        }

    }

    public void OnBoxDestroyed(Box destroyedBox)
    {
        // 리스트에서 제거
        boxList.Remove(destroyedBox);

        // 파괴된 박스보다 위에 있는 박스들을 내려오게 함
        Vector3 targetPosition = destroyedBox.gameObject.transform.position;
        foreach (Box box in boxList)
        {
            Vector3 newTargetPosition = box.transform.position; // 현재 박스 위치 저장
            box.Drop(targetPosition, boxDropSpeed);
            targetPosition = newTargetPosition;                 // 다음 박스 targetPosition 갱신
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
