using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Controller")]
    public float moveSpeed;             // 기본 이동 속도
    public float runSpeedMultiplier;    // 달리기 시 이동 속도 배수
    public float staminaRun;            // 달리기 시 소모되는 스태미나량
    public float jumpForce;             // 점프 파워
    public float staminaJump;           // 점프 시 소모되는 스태미나량
    public LayerMask groundLayerMask;   // 지면 판정에 사용할 레이어 마스크
    public LayerMask wallLayerMask;     // 벽 판정에 사용할 레이어 마스크
    public float wallClimbSpeed;        // 벽 타기 속도
    public float wallHangTime;          // 매달리기 시간

    private Vector2 curMovementInput;   // 현재 입력된 이동 방향 (x, y)
    private Rigidbody rb;               // 플레이어의 Rigidbody
    private float originMoveSpeed;      // 초기 이동 속도 (복구용)
    private float originJumpForce;      // 초기 점프력 (복구용)

    private PlayerCondition playerCondition; // PlayerCondition 컴포넌트 (스태미나 등 상태 관리)

    [Header("Look")]
    public Transform cameraContainer;   // 카메라를 감싸는 컨테이너 (회전 및 위치 조정)
    public Camera fppCamera;            // 1인칭(First) 카메라
    public Camera tppCamera;            // 3인칭(Third) 카메라
    private bool isCamera = true;       // true이면 1인칭, false이면 3인칭 모드
    private float curCamXRot;           // 카메라의 X축 회전 값 (상하 회전)
    public float lookSensitivity;       // 마우스 민감도
    private Vector2 mouseDelta;         // 마우스 이동량
    public float maxXLook;              // 카메라 상단 제한 각도
    public float minXLook;              // 카메라 하단 제한 각도

    public bool canLook = true;         // 카메라 회전 가능 여부
    public Action Inventory;            // 인벤토리 열기 이벤트


    [HideInInspector]
    private bool isWallClimbing = false; // 벽 타기 상태
    private bool isWallHanging = false;  // 매달리기 상태


    private void Awake()
    {
        // 컴포넌트 초기화
        rb = GetComponent<Rigidbody>();
        originJumpForce = jumpForce;                // 초기 점프력 저장
        originMoveSpeed = moveSpeed;                // 초기 이동 속도 저장
        playerCondition = GetComponent<PlayerCondition>();
    }

    private void Start()
    {
        // 게임 시작 시 마우스 커서 숨김
        Cursor.lockState = CursorLockMode.Locked;

        // 초기 카메라 위치 설정: 기본은 1인칭 카메라 활성화
        fppCamera.gameObject.SetActive(true);
        tppCamera.gameObject.SetActive(false);
    }

    private void FixedUpdate()  // 물리 연산 업데이트 (프레임 고정 업데이트)
    {
        Move(); // 이동 로직 실행
        CheckWall(); // 벽 타기 및 매달리기 상태 확인
    }

    private void Move()
    {
        // 입력값에 따라 전후 및 좌우 이동 계산
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;           // 이동 속도를 곱해 최종 이동 벡터 계산
        dir.y = rb.velocity.y;      // 점프 등 y축 속도는 기존 rb의 y값 유지
        rb.velocity = dir;          // Rigidbody에 적용하여 이동
    }

    private void LateUpdate()
    {
        // 카메라 회전 처리를 수행 (마우스 입력 반영)
        if (canLook)
        {
            CameraLook();
        }
        // 디버그 용도로 플레이어 아래에 Ray를 그림
        Debug.DrawRay(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down * 0.5f, Color.red);
    }

    // 카메라 회전 처리 (상하/좌우)
    void CameraLook()
    {
        // 마우스의 y축 이동값에 민감도를 곱해 X축 회전을 증가
        curCamXRot += mouseDelta.y * lookSensitivity;
        // 상하 회전이 지정된 제한 각도 내에 있도록 보정
        curCamXRot = Mathf.Clamp(curCamXRot, minXLook, maxXLook);
        // 카메라 컨테이너의 로컬 회전을 보정 (상하 회전)
        cameraContainer.localEulerAngles = new Vector3(-curCamXRot, 0, 0);
        // 플레이어의 y축(좌우) 회전은 직접적으로 처리
        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    // 이동 입력 처리 (컨트롤러 또는 키보드 입력)
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            // 입력된 벡터 값을 저장
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            // 입력 취소 시 제로 벡터로 초기화
            curMovementInput = Vector2.zero;
        }
    }

    // 달리기 입력 처리 (스태미나 소모 및 이동 속도 증가)
    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            // 달리기 시작: 스태미나를 소모하고 이동 속도를 높임
            if (playerCondition.UseStamina(staminaRun))
            {
                moveSpeed *= runSpeedMultiplier;
                StartCoroutine(RunStaminaDrain());
            }
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            // 달리기 중지 시 초기 이동 속도로 복구
            moveSpeed = originMoveSpeed;
        }
    }

    // 달리기 시 지속적으로 스태미나 소모하는 코루틴
    private IEnumerator RunStaminaDrain()
    {
        while (moveSpeed > originMoveSpeed)
        {
            if (!playerCondition.UseStamina(staminaRun * Time.deltaTime))
            {
                // 스태미나 부족 시 이동 속도를 초기화
                moveSpeed = originMoveSpeed;
                break;
            }
            yield return null;
        }
    }

    // 마우스 룩 입력 처리
    public void OnLook(InputAction.CallbackContext context)
    {
        // 입력값을 가져와 mouseDelta에 저장
        mouseDelta = context.ReadValue<Vector2>();
    }

    // 점프 입력 처리 (점프 시 스태미나 소모)
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGround())
        {
            if (playerCondition.UseStamina(staminaJump))
            {
                // 점프 파워를 Impulse 방식으로 적용하여 순간 점프
                rb.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
            }
        }
    }

    // 지면 판정을 위한 Raycast 로직
    bool IsGround()
    {
        // 4방향 (전,후,좌,우)에서 아래 방향으로 Ray를 쏴서 지면 판정
        Ray[] ray = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),   // 전방
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),  // 후방
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),    // 우측
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)   // 좌측
        };

        // 각 Ray를 사용하여 0.5 단위 내에 groundLayerMask에 포함된 오브젝트가 있는지 확인
        for (int i = 0; i < ray.Length; i++)
        {
            if (Physics.Raycast(ray[i], 0.5f, groundLayerMask))
            {
                return true;
            }
        }
        return false;
    }

    // 아이템 효과로 점프력을 일시적으로 증가시키는 메서드
    public void AddJumpForce(float amount, float duration)
    {
        jumpForce += amount;
        StartCoroutine(AfterJumpForce(duration));
    }

    // 일정 시간이 지난 후 점프력을 원래 값으로 복구하는 코루틴
    private IEnumerator AfterJumpForce(float duration)
    {
        yield return new WaitForSeconds(duration);
        jumpForce = originJumpForce;
    }

    // 벽 타기 및 매달리기 상태 확인
    void CheckWall()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 0.5f))
        {
            if (hit.collider != null && ((1 << hit.collider.gameObject.layer) & wallLayerMask) != 0)
            {
                if (Input.GetKey(KeyCode.E)) // 벽 타기 키
                {
                    StartWallClimbing(hit);
                }
                else if (Input.GetKey(KeyCode.F)) // 벽 매달리기 키
                {
                    StartWallHanging(hit);
                }
            }
        }
        else
        {
            StopWallClimbing();
            StopWallHanging();
        }
    }

    // 벽 타기 시작
    void StartWallClimbing(RaycastHit hit)
    {
        isWallClimbing = true;
        rb.useGravity = false;
        transform.position = hit.point; // 벽에 붙음
        rb.velocity = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) // 위로 타기
        {
            transform.position += transform.up * moveSpeed * Time.deltaTime;    // 
        }
        else if (Input.GetKey(KeyCode.S)) // 아래로 타기
        {
            transform.position -= transform.up * moveSpeed * Time.deltaTime;
        }
    }

    // 벽 타기 중지
    void StopWallClimbing()
    {
        if (isWallClimbing)
        {
            isWallClimbing = false;
            rb.useGravity = true;
        }
    }

    // 벽 매달리기 시작
    void StartWallHanging(RaycastHit hit)
    {
        isWallHanging = true;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        transform.position = hit.point; // 벽에 붙음
    }

    // 벽 매달리기 중지
    void StopWallHanging()
    {
        if (isWallHanging)
        {
            isWallHanging = false;
            rb.useGravity = true;
        }
    }

    // 인벤토리 호출 입력 처리 (인벤토리 UI 표시)
    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Inventory?.Invoke();
            ToggleCursor(); // 인벤토리 오픈 시 커서를 표시
        }
    }

    // 커서 토글 및 카메라 회전 제어 (커서를 보이면 카메라 회전 비활성화)
    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }

    // 1인칭과 3인칭 카메라 전환 (입력 시 카메라 모드 토글)
    public void OnToggleCamera(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            // isCamera가 true이면 1인칭, false이면 3인칭 모드
            isCamera = !isCamera;   // 카메라 모드를 토글
            if (isCamera)
            {
                // 1인칭 모드: 1인칭 카메라 활성화, 3인칭 카메라 비활성화
                fppCamera.gameObject.SetActive(true);
                tppCamera.gameObject.SetActive(false);
            }
            else
            {
                // 3인칭 모드: 3인칭 카메라 활성화, 1인칭 카메라 비활성화
                fppCamera.gameObject.SetActive(false);
                tppCamera.gameObject.SetActive(true);
            }
        }
    }
}

