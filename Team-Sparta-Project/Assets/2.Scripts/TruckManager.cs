using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class TruckManager : MonoBehaviour
{
    // Ʈ�� �� ������Ʈ���� �����ϴ� ��ũ��Ʈ

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
                    // box tag ����
                    boxObj.gameObject.tag = TagData.TAG_BOX;

                    // box�� ��ũ��Ʈ �߰�
                    Box box = boxObj.gameObject.AddComponent<Box>();
                    box.SetBoxSetting();

                    // boxList�� �߰�
                    boxList.Add(box);
                }
            }
        }

    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == TagData.TAG_MONSTER)
        {
            // ���Ϳ� �浹 �� ����� ������ �����
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
            // ���Ϳ� �浹 �� ����� ������ �����
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
