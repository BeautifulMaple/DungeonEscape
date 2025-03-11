using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private static readonly int IsRun = Animator.StringToHash("IsRun");
    private static readonly int IsJump = Animator.StringToHash("IsJump");

    private Animator animator;
    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimation();
    }

    // 애니메이션 상태를 업데이트하는 함수
    private void UpdateAnimation()
    {
        if (playerController != null)
        {
            // 이동 중인지 확인하여 IsRun 파라미터 설정
            bool isRunning = playerController.curMovementInput != Vector2.zero;
            animator.SetBool(IsRun, isRunning);

            // 점프 중인지 확인하여 IsJump 파라미터 설정
            bool isJumping = !playerController.IsGround();
            animator.SetBool(IsJump, isJumping);
        }
    }
}
