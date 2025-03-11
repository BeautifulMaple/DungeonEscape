using UnityEngine;

public class FootSteps : MonoBehaviour
{

    public AudioClip[] footstepClips;
    private AudioSource audioSource;
    private Rigidbody _rigidbody;
    public float footstepThreshold;
    public float footstepRate;
    private float footStepTime;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // 플레이어가 움직일 때 발소리 재생
        if (Mathf.Abs(_rigidbody.velocity.y) < 0.1f)    //  플레이어가 점프 중이 아닐 때
        {
            if (_rigidbody.velocity.magnitude > footstepThreshold)  // 플레이어가 움직일 때
            {
                if (Time.time - footStepTime > footstepRate)    // 발소리 재생 간격
                {
                    footStepTime = Time.time;   //  발소리 재생 시간 갱신
                    audioSource.PlayOneShot(footstepClips[Random.Range(0, footstepClips.Length)]);
                }
            }
        }
    }
}