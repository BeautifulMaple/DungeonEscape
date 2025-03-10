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
    //void Update()
    //{
    //    UpdateAnimation();
    //}
}
