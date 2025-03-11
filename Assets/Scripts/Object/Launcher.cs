using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{

    public float jumpForce;
    public Rigidbody rb;
    private bool isPlayerOnPlatform = false;  // 플레이어가 플랫폼 위에 있는지 여부를 저장하는 플래그
    private Coroutine jumpCoroutine;

    // 충돌이 시작되었을 때 실행되는 함수
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))  // 충돌한 객체가 "Player" 태그를 가진 경우
        {
            rb = collision.gameObject.GetComponent<Rigidbody>();  // 플레이어의 Rigidbody를 가져옴
            if (rb != null)  // Rigidbody가 존재하면
            {
                isPlayerOnPlatform = true;
                collision.transform.SetParent(transform);  // 플레이어를 발판의 자식으로 설정 (발판과 함께 움직임)
                jumpCoroutine = StartCoroutine(JumpAfterDelay(3f));  // 3초 후에 점프하는 코루틴 시작
            }
        }
    }

    // 충돌이 끝났을 때 실행되는 함수
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))  // 충돌한 객체가 "Player" 태그를 가진 경우
        {
            isPlayerOnPlatform = false;  // 플레이어가 플랫폼을 떠남
            collision.transform.SetParent(null);  // 플레이어의 부모를 제거하여 원래 위치로 돌아감
            if (jumpCoroutine != null)
            {
                StopCoroutine(jumpCoroutine);  // 코루틴 중지
                jumpCoroutine = null;
            }
        }
    }
    private IEnumerator JumpAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (isPlayerOnPlatform && rb != null)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isPlayerOnPlatform = false;  // 플레이어가 발사되었으므로 플랫폼 위에 있지 않음
        }
    }
}
