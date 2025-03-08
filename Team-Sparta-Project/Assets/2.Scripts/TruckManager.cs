using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

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
                    // box tag ����
                    boxObj.gameObject.tag = TagData.TAG_BOX;

                    // box�� ������Ʈ �߰�
                    Box box = boxObj.gameObject.AddComponent<Box>();
                   
                    // box ����
                    box.SetBoxSetting(this, boxMaxHP);
                    
                    // boxList�� �߰�
                    boxList.Add(box);
                }
            }
        }

    }

    public void OnBoxDestroyed(Box destroyedBox)
    {
        // ����Ʈ���� ����
        boxList.Remove(destroyedBox);

        // �ı��� �ڽ����� ���� �ִ� �ڽ����� �������� ��
        Vector3 targetPosition = destroyedBox.gameObject.transform.position;
        foreach (Box box in boxList)
        {
            Vector3 newTargetPosition = box.transform.position; // ���� �ڽ� ��ġ ����
            box.Drop(targetPosition, boxDropSpeed);
            targetPosition = newTargetPosition;                 // ���� �ڽ� targetPosition ����
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
