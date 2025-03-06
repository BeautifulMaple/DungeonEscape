using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Controller")]
    public float moveSpeed;             // 
    public float jumpForce;             // 점프 파워
    public LayerMask groundLayerMask;   // 땅 레이어 마스크
    private Vector2 curMovementInput;   // 현재 입력된 이동 방향
    private Rigidbody rb;   // 리지드바디 컴포넌트

    [Header("Look")]
    public Transform cameraContainer;
    private float curCamXRot;    // 현재 카메라 x축 회전값
    public float lookSensitivity;   // 카메라 민감도
    private Vector2 mouseDelta;  // 마우스 이동량
    public float maxXLook;  // 최대 시야각
    public float minXLook;  // 최소 시야각

    public bool canLook = true;
    public Action Inventory;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        // 마우스 커서 숨기기
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()  // 물리 연산 업데이트 
    {
        Move(); // 이동
    }

    private void Move()
    {
        // 현재 입력의 y 값은 z 축(forward, 앞뒤)에 곱한다.
        // 현재 입력의 x 값은 x 축(right, 좌우)에 곱한다
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;   // 이동 방향에 이동 속도를 곱함
        dir.y = rb.velocity.y;  // 리지드바디의 y축 속도를 그대로 유지
        rb.velocity = dir; // 리지드바디에 방향을 적용
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
        // 레이선 그리기
        //Debug.DrawRay(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down * 0.5f, Color.red);

    }
    void CameraLook()
    {
        curCamXRot += mouseDelta.y * lookSensitivity; // 마우스 y축 이동량에 민감도를 곱해 x축 회전값에 더함
        curCamXRot = Mathf.Clamp(curCamXRot, minXLook, maxXLook); // x축 회전값을 최대 최소 시야각으로 제한
        cameraContainer.localEulerAngles = new Vector3(-curCamXRot, 0, 0); // Rotation : + 아래로, - 위로 올라감
        // eulerAngles : 오일러 각도로 회전값을 나타내는 변수
        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0); // 플레이어 캐릭터의 y축 회전값을 마우스 x축 이동량에 민감도를 곱해 더함
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // phase : 입력 상태 | Performed : 눌림 Canceled : 떼어짐
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        // ReadValue<Vector2>() : 입력값을 벡터2로 반환
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started  && IsGround())
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
        }
    }

    bool IsGround()
    {
        #region
        ///	첫 번째 Ray: 전방으로 0.2 단위, 위로 0.01 단위 이동.
        ///	두 번째 Ray: 뒤로 0.2 단위, 위로 0.01 단위 이동.
        ///	세 번째 Ray: 오른쪽으로 0.2 단위, 위로 0.01 단위 이동.
        ///	네 번째 Ray: 왼쪽으로 0.2 단위, 위로 0.01 단위 이동.
        # endregion 
        Ray[] ray = new Ray[4]
        {
            // Vector3.down : 아래 방향으로 레이를 쏨 
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),   // 전방
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),  // 뒤
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),    // 오른쪽
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)  // 왼쪽
        };

        for (int i = 0; i < ray.Length; i++)
        {
            // Physics.Raycast : 레이캐스트를 쏨
            if (Physics.Raycast(ray[i], 0.5f, groundLayerMask))
            {
                return true;
            }
        }
        return false;
    }
    
    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Inventory?.Invoke();
            ToggleCursor(); //  인벤토리를 열면 커서를 보이게 함
        }
    }

    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;    // 커서가 보이는지 여부를 확인
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;    //
        canLook = !toggle;  // 커서가 보이면 카메라 회전을 막음
    }
}

