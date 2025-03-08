using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class BoxTag
{
    public const string boxTag = "Box";
}

public class BoxManager : MonoBehaviour
{
    // 박스 오브젝트들을 관리하는 스크립트

    [SerializeField] private Transform boxTrans;
    [SerializeField] private List<GameObject> boxList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        InitBoxList();
    }

    void InitBoxList()
    {
        foreach(Transform boxObj in boxTrans)
        {
            if(boxObj.name.Contains("Box"))
            {
                boxList.Add(boxObj.gameObject);

                if(boxObj.gameObject.GetComponent<Box>() == null)
                {
                    // box tag 변경
                    boxObj.gameObject.tag = BoxTag.boxTag;

                    // box에 스크립트 추가
                    Box box = boxObj.gameObject.AddComponent<Box>();
                    box.SetBoxSetting();
                }
            }
        }

    }
}
