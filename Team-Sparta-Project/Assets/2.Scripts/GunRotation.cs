using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRotation : MonoBehaviour
{
    // 마우스 이벤트에 따른 Gun 회전 스크립트

    [SerializeField] private GameObject gunVision;

    private bool isDragging = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 마우스 클릭 시 gunVision 키기
            isDragging = true;
            gunVision.SetActive(true);
        }
        if (Input.GetMouseButtonUp(0))
        {
            // 마우스 땔 시 gunVision 끄기
            isDragging = false;
            gunVision.SetActive(false);
        }
        if (isDragging)
        {
            // 마우스 드래그 시 총 각도 조정
            RotateGunTowardsMouse();
        }

        void RotateGunTowardsMouse()
        {
            // 마우스 위치를 월드 좌표로 변환
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
            mousePos.z = 0; 

            // 각도 계산
            Vector3 gunPos = transform.position;
            Vector3 direction = mousePos - gunPos;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // 회전 각도 제한
            angle = Mathf.Clamp(angle, GameData.Instance.MinGunRotation, GameData.Instance.MaxGunRotation);

            // 마우스를 바라보는 방향으로 각도 조정
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
