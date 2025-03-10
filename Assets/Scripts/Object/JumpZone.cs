using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpZone : MonoBehaviour
{
    public float jumpForce;  // 플레이어가 점프하는 힘
    public Vector3[] positions;  // 발판이 이동할 위치들 (목표 위치 배열)
    public float speed;  // 발판의 이동 속도

    private int targetIndex = 0;  // 현재 목표 위치의 인덱스
    private Rigidbody rb;  // 플레이어의 Rigidbody 컴포넌트를 저장할 변수

    private void Update()
    {
        MovePlatform();  // 발판의 이동을 처리하는 함수 호출
    }

    // 발판이 설정된 위치들을 따라 이동하는 함수
    private void MovePlatform()
    {
        if (positions.Length == 0) return;  // 위치 배열이 비어있으면 이동하지 않음

        // 현재 위치에서 목표 위치로 이동 (Time.deltaTime을 사용해 프레임 독립적인 이동 구현)
        transform.position = Vector3.MoveTowards(transform.position, positions[targetIndex], speed * Time.deltaTime);

        // 목표 위치와의 거리가 일정 값 이하로 줄어들면 다음 위치로 이동
        if (Vector3.Distance(transform.position, positions[targetIndex]) < 0.1f)
        {
            targetIndex = (targetIndex + 1) % positions.Length;  // 위치 배열의 끝에 도달하면 처음으로 돌아감
        }
    }

    // 충돌이 시작되었을 때 실행되는 함수
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))  // 충돌한 객체가 "Player" 태그를 가진 경우
        {
            rb = collision.gameObject.GetComponent<Rigidbody>();  // 플레이어의 Rigidbody를 가져옴
            if (rb != null)  // Rigidbody가 존재하면
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);  // 플레이어에게 점프 힘을 추가
                collision.transform.SetParent(transform);  // 플레이어를 발판의 자식으로 설정 (발판과 함께 움직임)
            }
        }
    }

    // 충돌이 끝났을 때 실행되는 함수
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))  // 충돌한 객체가 "Player" 태그를 가진 경우
        {
            collision.transform.SetParent(null);  // 플레이어의 부모를 제거하여 원래 위치로 돌아감
        }
    }
}
