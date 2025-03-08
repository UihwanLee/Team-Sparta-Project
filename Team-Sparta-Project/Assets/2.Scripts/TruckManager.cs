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
    // Ʈ�� �� ������Ʈ���� �����ϴ� ��ũ��Ʈ

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
                    // box tag ����
                    boxObj.gameObject.tag = BoxData.boxTag;

                    // box�� ��ũ��Ʈ �߰�
                    Box box = boxObj.gameObject.AddComponent<Box>();
                    box.SetBoxSetting();

                    // boxList�� �߰�
                    boxList.Add(box);
                }
            }
        }

    }

    private void CheckDamage()
    {
        bool isDamage = false;

        // ������ �˻�
        foreach (Box box in boxList)
        {
            if (box.IsDamage)
            {
                isDamage = true;
            }
        }

        // BoxList���� Box�� ���ݹް� �ִٸ� Ʈ���� ������ �����
        foreach (MovingWheel wheel in wheels)
        {
            wheel.IsDamage = isDamage;
        }
    }
}
