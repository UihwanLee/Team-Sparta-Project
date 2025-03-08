using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Box : MonoBehaviour
{
    [Header("Box Info")]
    [SerializeField] private int hp;
    [SerializeField] private bool isDamage;
    [SerializeField] private bool isDestroy;

    private GameObject boxSprite;
    private GameObject hpPannel;

    private TruckManager truckManager;

    // Start is called before the first frame update
    void Start()
    {
        hp = 100;
    }

    public void SetBoxSetting(TruckManager _truckManager)
    {
        if(this.transform.childCount >= 2)
        {
            // Box Sprite ����
            boxSprite = this.transform.GetChild(0).GetChild(0).gameObject;

            // Hp Pannel ����
            hpPannel = this.transform.GetChild(1).gameObject;
        }

        truckManager = _truckManager;
    }

    public void Damage(int _dmg)
    {
        // hp�� 0 ���� �� �ı�
        if(hp - _dmg <= 0)
        {
            hp = 0;
            Destroy();
            return;
        }

        // ���� �޴� ���

        // ������ ����
        hp -= _dmg;
    }

    private void Destroy()
    {
        if(truckManager == null)
        {
            Debug.LogError("There is no truckManager");
            return;
        }

        isDestroy = true;

        truckManager.OnBoxDestroyed(this);

        Destroy(gameObject);
    }

    public void Drop(Vector3 targetPosition, float dropSpeed)
    {
        StartCoroutine(DropCoroutine(targetPosition, dropSpeed));
    }

    IEnumerator DropCoroutine(Vector3 targetPosition, float dropSpeed)
    {
        this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, dropSpeed * Time.deltaTime);
        yield return new WaitForSeconds(0.1f);
        this.gameObject.transform.position = targetPosition; // ���� ��ġ ����
    }

    public int Hp { get { return hp; } set { hp = value; }}

    public bool IsDamage { get { return isDamage; } set { isDamage = value; } }
    public bool IsDestroy() { return isDestroy; }
}
