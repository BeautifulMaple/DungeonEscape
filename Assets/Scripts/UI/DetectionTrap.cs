using System;
using System.Collections;
using UnityEngine;

public class DetectionTrap : MonoBehaviour
{
    public float detectionRange = 5f; // 감지 범위
    public LayerMask warningLayer; // 위험 레이어
    public GameObject warningMessage; // 경고 메시지 오브젝트
    public Transform cameraContainer; // 카메라 컨테이너

    private void Update()
    {
        DetectPlayer();
    }

    // 플레이어 감지 메서드
    private void DetectPlayer()
    {
        RaycastHit hit;

        // CameraContainer의 위치와 방향을 사용
        Vector3 rayOrigin = cameraContainer.position;
        Vector3 rayDirection = cameraContainer.forward;

        // 레이캐스트를 수행하고 결과를 hit에 저장
        if (Physics.Raycast(rayOrigin, rayDirection, out hit, detectionRange, warningLayer))
        {
            // 레이를 그려서 시각적으로 확인
            Debug.DrawRay(rayOrigin, rayDirection * detectionRange, Color.red);

            if (hit.collider != null)
            {
                // 플레이어가 감지 범위 내에 있을 때 경고 메시지 표시
                ShowWarningMessage();
            }
        }
        else
        {
            // 레이를 그려서 시각적으로 확인
            Debug.DrawRay(rayOrigin, rayDirection * detectionRange, Color.green);

            // 플레이어가 감지 범위 내에 없을 때 경고 메시지 숨김
            HideWarningMessage();
        }
    }

    // 경고 메시지 표시 메서드
    private void ShowWarningMessage()
    {
        if (warningMessage != null)
        {
            warningMessage.SetActive(true);
        }
    }

    // 경고 메시지 숨김 메서드
    private void HideWarningMessage()
    {
        if (warningMessage != null)
        {
            warningMessage.SetActive(false);
        }
    }
}
