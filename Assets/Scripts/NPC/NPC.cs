using System.Collections;
using UnityEngine;
using UnityEngine.AI;

// AI의 상태를 나타내는 열거형
public enum AIState
{
    Idle,       // 대기 상태
    Wandering,  // 배회 상태 (랜덤 이동)
    Attacking   // 공격 상태
}

// NPC 클래스: 네비게이션, 배회, 전투 기능을 수행
public class NPC : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    public float health;            // 체력
    public float walkSpeed;       // 걷기 속도
    public float runSpeed;        // 뛰기 속도
    public ItemData[] dropOnDeath; // 사망 시 드롭할 아이템 목록

    [Header("AI")]
    private NavMeshAgent agent;    // 네비게이션 에이전트 (이동 제어)
    public float detectDistance;   // 플레이어 감지 거리
    private AIState aiState;       // 현재 AI 상태

    [Header("Wandering")]
    public float minWanderDistance;  // 배회 시 최소 이동 거리
    public float maxWanderDistance;  // 배회 시 최대 이동 거리
    public float minWanderWaitTime;  // 배회 대기 최소 시간
    public float maxWanderWaitTime;  // 배회 대기 최대 시간

    [Header("Combat")]
    public float damage;             // 공격력
    public float attackRate;        // 공격 속도 (공격 간격)
    private float lastAttackTime;   // 마지막 공격 시간
    public float attackDistance;    // 공격 거리

    private float playerDistance;   // 플레이어와의 거리
    public float fieldOfView = 120f; // NPC의 시야각

    private Animator animator;                // 애니메이터
    private SkinnedMeshRenderer[] meshRenderers; // 캐릭터의 스킨 메쉬 렌더러 (피격 효과용)

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();        // 네비게이션 에이전트 가져오기
        animator = GetComponent<Animator>();        // 애니메이터 가져오기
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>(); // 캐릭터의 메쉬 렌더러 가져오기
    }

    void Start()
    {
        SetState(AIState.Wandering); // 시작할 때 배회 상태로 설정
    }

    void Update()
    {
        // 플레이어와의 거리 계산
        playerDistance = Vector3.Distance(transform.position, CharacterManager.Instance.Player.transform.position);
        // 이동 여부를 애니메이터에 전달 (Idle 상태 제외)
        animator.SetBool("Moving", aiState != AIState.Idle);

        // 현재 AI 상태에 따라 동작 분기
        switch (aiState)
        {
            case AIState.Idle:
            case AIState.Wandering:
                PassiveUpdate(); // 감지 및 배회 로직 실행
                break;
            case AIState.Attacking:
                AtttackingUpdate(); // 전투 로직 실행
                break;
        }
    }

    // AI 상태 변경 함수
    public void SetState(AIState state)
    {
        aiState = state;

        switch (aiState)
        {
            case AIState.Idle:
                agent.speed = walkSpeed;
                agent.isStopped = true; // 이동 중지
                break;
            case AIState.Wandering:
                agent.speed = walkSpeed;
                agent.isStopped = false; // 이동 시작
                break;
            case AIState.Attacking:
                agent.speed = runSpeed;
                agent.isStopped = true; // 공격 시 이동 정지
                break;
        }

        // 애니메이션 속도를 걷기 속도 기준으로 조절
        animator.speed = agent.speed / walkSpeed;
    }

    // 플레이어 감지 및 배회 관련 업데이트
    void PassiveUpdate()
    {
        // 배회 중이며 목표 지점에 도착했을 경우 일정 시간 후 새로운 위치로 이동
        if (aiState == AIState.Wandering && agent.remainingDistance < 0.1f)
        {
            SetState(AIState.Idle);
            Invoke("WanderToNewLocation", Random.Range(minWanderWaitTime, maxWanderWaitTime));
        }

        // 플레이어가 감지 거리 내에 있으면 공격 상태로 전환
        if (playerDistance < detectDistance)
        {
            SetState(AIState.Attacking);
        }
    }

    // 일정 시간 후 새로운 배회 위치로 이동
    void WanderToNewLocation()
    {
        if (aiState != AIState.Idle) return;

        SetState(AIState.Wandering);
        agent.SetDestination(GetWanderLocation());
    }

    // 랜덤한 배회 위치 반환
    Vector3 GetWanderLocation()
    {
        NavMeshHit hit;
        int i = 0;
        ///while (Vector3.Distance(transform.position, hit.position) < detectDistance)
        ///{
        ///    NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);
        ///    i++;
        ///   if (i == 30) break;
        ///}
        do
        {
            // 현재 위치에서 랜덤한 방향으로 이동할 위치 설정
            NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);
            i++;
        }
        while (Vector3.Distance(transform.position, hit.position) < detectDistance && i < 30);

        return hit.position;
    }

    // 공격 관련 업데이트
    void AtttackingUpdate()
    {
        if (playerDistance < attackDistance && IsPlayerInFieldOfView())
        {
            agent.isStopped = true;

            // 공격 가능 시간인지 체크
            if (Time.time - lastAttackTime > attackRate)
            {
                lastAttackTime = Time.time;
                CharacterManager.Instance.Player.condition.GetComponent<IDamageable>().TakeDamage(damage);
                animator.speed = 1f;
                animator.SetTrigger("Attack");
            }
        }
        else
        {
            // 플레이어가 감지 거리 내에 있다면 따라가기
            if (playerDistance < detectDistance)
            {
                agent.isStopped = false;
                agent.SetDestination(CharacterManager.Instance.Player.transform.position);
            }
            else
            {
                // 플레이어를 놓쳤을 경우 다시 배회 상태로 전환
                SetState(AIState.Wandering);
            }
        }
    }

    // 플레이어가 NPC의 시야 내에 있는지 확인
    bool IsPlayerInFieldOfView()
    {
        Vector3 directionToPlayer = CharacterManager.Instance.Player.transform.position - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        return angle < fieldOfView * 0.5f;
    }

    // 데미지를 받았을 때 처리
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }

        StartCoroutine(DamageFlash()); // 피격 효과
    }

    // 사망 처리
    void Die()
    {
        // 사망 시 아이템 드롭
        foreach (var item in dropOnDeath)
        {
            Instantiate(item.dropPrefab, transform.position + Vector3.up * 2, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    // 피격 시 빨갛게 변했다가 다시 원래 색으로 복귀
    IEnumerator DamageFlash()
    {
        // 모든 스킨 메쉬 렌더러에 대해 색상 변경
        foreach (var renderer in meshRenderers)
        {
            renderer.material.color = new Color(1.0f, 0.6f, 0.6f);  // 빨갛게 변경
        }
        yield return new WaitForSeconds(0.1f);
        // 원래 색상으로 변경
        foreach (var renderer in meshRenderers)
        {
            renderer.material.color = Color.white;
        }
    }
}