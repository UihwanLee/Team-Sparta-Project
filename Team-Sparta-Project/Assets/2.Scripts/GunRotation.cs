using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRotation : MonoBehaviour
{
    // ���콺 �̺�Ʈ�� ���� Gun ȸ�� ��ũ��Ʈ

    [SerializeField] private GameObject gunVision;

    private bool isDragging = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // ���콺 Ŭ�� �� gunVision Ű��
            isDragging = true;
            gunVision.SetActive(true);
        }
        if (Input.GetMouseButtonUp(0))
        {
            // ���콺 �� �� gunVision ����
            isDragging = false;
            gunVision.SetActive(false);
        }
        if (isDragging)
        {
            // ���콺 �巡�� �� �� ���� ����
            RotateGunTowardsMouse();
        }

        void RotateGunTowardsMouse()
        {
            // ���콺 ��ġ�� ���� ��ǥ�� ��ȯ
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
            mousePos.z = 0; 

            // ���� ���
            Vector3 gunPos = transform.position;
            Vector3 direction = mousePos - gunPos;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // ȸ�� ���� ����
            angle = Mathf.Clamp(angle, GameData.Instance.MinGunRotation, GameData.Instance.MaxGunRotation);

            // ���콺�� �ٶ󺸴� �������� ���� ����
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
