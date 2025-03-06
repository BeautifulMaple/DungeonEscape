using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Controller")]
    public float moveSpeed;             // 
    public float jumpForce;             // ���� �Ŀ�
    public LayerMask groundLayerMask;   // �� ���̾� ����ũ
    private Vector2 curMovementInput;   // ���� �Էµ� �̵� ����
    private Rigidbody rb;   // ������ٵ� ������Ʈ

    [Header("Look")]
    public Transform cameraContainer;
    private float curCamXRot;    // ���� ī�޶� x�� ȸ����
    public float lookSensitivity;   // ī�޶� �ΰ���
    private Vector2 mouseDelta;  // ���콺 �̵���
    public float maxXLook;  // �ִ� �þ߰�
    public float minXLook;  // �ּ� �þ߰�

    public bool canLook = true;
    public Action Inventory;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        // ���콺 Ŀ�� �����
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()  // ���� ���� ������Ʈ 
    {
        Move(); // �̵�
    }

    private void Move()
    {
        // ���� �Է��� y ���� z ��(forward, �յ�)�� ���Ѵ�.
        // ���� �Է��� x ���� x ��(right, �¿�)�� ���Ѵ�
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;   // �̵� ���⿡ �̵� �ӵ��� ����
        dir.y = rb.velocity.y;  // ������ٵ��� y�� �ӵ��� �״�� ����
        rb.velocity = dir; // ������ٵ� ������ ����
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
        // ���̼� �׸���
        //Debug.DrawRay(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down * 0.5f, Color.red);

    }
    void CameraLook()
    {
        curCamXRot += mouseDelta.y * lookSensitivity; // ���콺 y�� �̵����� �ΰ����� ���� x�� ȸ������ ����
        curCamXRot = Mathf.Clamp(curCamXRot, minXLook, maxXLook); // x�� ȸ������ �ִ� �ּ� �þ߰����� ����
        cameraContainer.localEulerAngles = new Vector3(-curCamXRot, 0, 0); // Rotation : + �Ʒ���, - ���� �ö�
        // eulerAngles : ���Ϸ� ������ ȸ������ ��Ÿ���� ����
        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0); // �÷��̾� ĳ������ y�� ȸ������ ���콺 x�� �̵����� �ΰ����� ���� ����
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // phase : �Է� ���� | Performed : ���� Canceled : ������
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
        // ReadValue<Vector2>() : �Է°��� ����2�� ��ȯ
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
        ///	ù ��° Ray: �������� 0.2 ����, ���� 0.01 ���� �̵�.
        ///	�� ��° Ray: �ڷ� 0.2 ����, ���� 0.01 ���� �̵�.
        ///	�� ��° Ray: ���������� 0.2 ����, ���� 0.01 ���� �̵�.
        ///	�� ��° Ray: �������� 0.2 ����, ���� 0.01 ���� �̵�.
        # endregion 
        Ray[] ray = new Ray[4]
        {
            // Vector3.down : �Ʒ� �������� ���̸� �� 
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),   // ����
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),  // ��
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),    // ������
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)  // ����
        };

        for (int i = 0; i < ray.Length; i++)
        {
            // Physics.Raycast : ����ĳ��Ʈ�� ��
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
            ToggleCursor(); //  �κ��丮�� ���� Ŀ���� ���̰� ��
        }
    }

    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;    // Ŀ���� ���̴��� ���θ� Ȯ��
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;    //
        canLook = !toggle;  // Ŀ���� ���̸� ī�޶� ȸ���� ����
    }
}

