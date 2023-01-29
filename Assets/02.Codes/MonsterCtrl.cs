using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MonsterCtrl : MonoBehaviour
{
    //(유한 상태 머신 구현(+enum))
    // Monster 상태State
    public enum State
    { IDLE, TRACE, ATTACK, DIE }

    //Monster의 현상태
    public State state = State.IDLE;

    //Monster의 추적 사정거리
    public float traceDist = 10.0f;
    //Monster의 공격 사정거리
    public float attackDist = 2.0f;
    //Monster의 사망여부
    public bool isDie = false;

    private Transform monsterTr;
    private Transform playerTr;
    private NavMeshAgent agent;
    // (애니메이션 연결)
    private Animator anim;

    //(Monster 공격 루틴)
    private readonly int hashTrace = Animator.StringToHash("IsTrace");
    private readonly int hashAttack = Animator.StringToHash("IsAttack");

    //(Monset 혈흔,총 맞는 효과)
    private readonly int hashHit = Animator.StringToHash("Hit");

    //(Monset 혈흔,총 맞는 효과)
    private GameObject bloodEffect;
   
    //Delegate 활용
    private readonly int hashPlayerDie = Animator.StringToHash("PlayerDie");
    private readonly int hashSpeed = Animator.StringToHash("Speed");
    private readonly int hashDie = Animator.StringToHash("Die");

    private int hp = 100;

    void OnEnable()
    {
        //Delegate 활용
        PlayerCtrl.OnPlayerDie += this.OnPlayerDie;

        //(유한 상태 머신 구현(+enum))
        //Monster 상태를 체크하는 코루틴 함수 호출
        StartCoroutine(CheckMonsterState());

        //(Monster 행동 구현)
        //Monster의 상태에 따라 행동을 수행하는 함수 호출
        StartCoroutine(MonsterAction());
    }

    private void OnDisable()
    {
        //Delegate 활용
        PlayerCtrl.OnPlayerDie -= this.OnPlayerDie;
    }
    void Awake()
    {
        monsterTr = GetComponent<Transform>();
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        //(NavMeshAgent)
        //monster가 가진 NavMeshAgent 컴포넌트에서 추격할 대상 = Player의 위치
        // agent.destination = playerTr.position;
        // = agent.SetDestination(playerTr.position);
        // 바로 추적 시작

        // (애니메이션 연결)
        anim = GetComponent<Animator>();

        //(Monset 혈흔,총 맞는 효과)
        bloodEffect = Resources.Load<GameObject>("BloodSprayEffect");
    }

    //(유한 상태 머신 구현(+enum))
    // 0.3f 시간 만큼(일정 간격) 몬스터 행동 상태를 체크
    IEnumerator CheckMonsterState()
    {
        // 0.3초 동안 중지하는 동안 제어권 양보
        while (!isDie)
        {
            yield return new WaitForSeconds(0.3f);
            
            //Monster와 Player사이의 거리 측정
            float distance = Vector3.Distance(playerTr.position, monsterTr.position);

            //Delegate 활용
            if (state == State.DIE) yield break;

            //공격 사정거리 범위로 들어왔는지 체크
            if (distance <= attackDist)
            { state = State.ATTACK; }

            //추적 사정거리 범위로 들어왔는지 체크
            else if (distance <= traceDist)
            { state = State.TRACE; }

            //추적 사정거리 범위로 들어왔는지 체크
            else
            { state = State.IDLE; }
        }
    }
    //(Monster 행동 구현)
    //상태에 따라 행동을 수행하는 함수
    IEnumerator MonsterAction()
    {
        while (!isDie)
        {
            switch (state)
            {
                // IDLE상태일때 추적 중지
                // isStopped 추적 중지여부 true = 추적 중지
                case State.IDLE:
                    agent.isStopped = true;

                    // (애니메이션 연결)
                    // anim.SetBool("IsTrace", false);

                    //(Monster 공격 루틴)
                    anim.SetBool(hashTrace, false);
                    break;

                // TRACE 추적 상태
                // 추적대상의 위치로 이동
                // isStopped 추적 중지여부 false = 추적 실행
                case State.TRACE:
                    agent.SetDestination(playerTr.position);
                    agent.isStopped = false;
                    // (애니메이션 연결)
                    // anim.SetBool("IsTrace", true);

                    //(Monster 공격 루틴)
                    anim.SetBool(hashTrace, true);
                    anim.SetBool(hashAttack, false);
                    break;

                // ATTACK 상태
                case State.ATTACK:
                    //(Monster 공격 루틴)
                    anim.SetBool(hashAttack, true);
                    break;

                // DIE 상태
                case State.DIE:
                    //Delegate 활용
                    isDie = true;
                    agent.isStopped = true;
                    anim.SetTrigger(hashDie);
                    GetComponent<CapsuleCollider>().enabled = false;

                    // 오브젝트 풀 활용
                    yield return new WaitForSeconds(3.0f);
                    hp = 100;
                    isDie = false;
                    GetComponent<CapsuleCollider>().enabled = true;
                    this.gameObject.SetActive(false);

                    break;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    private void OnCollisionEnter(Collision coll)
    {
        //(Monset 혈흔,총 맞는 효과)
        if (coll.collider.CompareTag("Bullet"))
        {
            Destroy(coll.gameObject);

            //(Monset 혈흔,총 맞는 효과)
            anim.SetTrigger(hashHit);

            //(Monset 혈흔,총 맞는 효과)
            //Bullet충돌 지점
            Vector3 pos = coll.GetContact(0).point;
            //Bullet 충돌 지점의 법선 벡터
            Quaternion rot = Quaternion.LookRotation(-coll.GetContact(0).normal);
            //혈흔 효과 생성 함수
            ShowBloodEffect(pos, rot);

            //Delegate 활용
            hp -= 10;
            if(hp<=0)
            { 
                state = State.DIE; 
                GameManager.instance.DisplayScore(50);
            }
        }
    }

    private void OnTriggerEnter(Collider coll)
    {
        Debug.Log(coll.gameObject.name);

    }

    //(Monset 혈흔,총 맞는 효과)
    void ShowBloodEffect(Vector3 pos, Quaternion rot)
    {
        GameObject blood = Instantiate<GameObject>(bloodEffect, pos, rot, monsterTr);
        Destroy(blood, 1.0f);
    }

    //(유한 상태 머신 구현(+enum))
    void OnDrawGizmos()
    {
        // 추적 사정거리 표시
        if (state == State.TRACE)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, traceDist);
        }

        //공격 사정거리 표시
        if (state == State.ATTACK)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackDist);
        }
    }

    void OnPlayerDie()
    {
        StopAllCoroutines();

        agent.isStopped= true;
        anim.SetFloat(hashSpeed, Random.Range(0.8f,1.2f));

        anim.SetTrigger(hashPlayerDie);
    }

}
