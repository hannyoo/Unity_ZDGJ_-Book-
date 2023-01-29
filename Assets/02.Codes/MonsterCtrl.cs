using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MonsterCtrl : MonoBehaviour
{
    //(���� ���� �ӽ� ����(+enum))
    // Monster ����State
    public enum State
    { IDLE, TRACE, ATTACK, DIE }

    //Monster�� ������
    public State state = State.IDLE;

    //Monster�� ���� �����Ÿ�
    public float traceDist = 10.0f;
    //Monster�� ���� �����Ÿ�
    public float attackDist = 2.0f;
    //Monster�� �������
    public bool isDie = false;

    private Transform monsterTr;
    private Transform playerTr;
    private NavMeshAgent agent;
    // (�ִϸ��̼� ����)
    private Animator anim;

    //(Monster ���� ��ƾ)
    private readonly int hashTrace = Animator.StringToHash("IsTrace");
    private readonly int hashAttack = Animator.StringToHash("IsAttack");

    //(Monset ����,�� �´� ȿ��)
    private readonly int hashHit = Animator.StringToHash("Hit");

    //(Monset ����,�� �´� ȿ��)
    private GameObject bloodEffect;
   
    //Delegate Ȱ��
    private readonly int hashPlayerDie = Animator.StringToHash("PlayerDie");
    private readonly int hashSpeed = Animator.StringToHash("Speed");
    private readonly int hashDie = Animator.StringToHash("Die");

    private int hp = 100;

    void OnEnable()
    {
        //Delegate Ȱ��
        PlayerCtrl.OnPlayerDie += this.OnPlayerDie;

        //(���� ���� �ӽ� ����(+enum))
        //Monster ���¸� üũ�ϴ� �ڷ�ƾ �Լ� ȣ��
        StartCoroutine(CheckMonsterState());

        //(Monster �ൿ ����)
        //Monster�� ���¿� ���� �ൿ�� �����ϴ� �Լ� ȣ��
        StartCoroutine(MonsterAction());
    }

    private void OnDisable()
    {
        //Delegate Ȱ��
        PlayerCtrl.OnPlayerDie -= this.OnPlayerDie;
    }
    void Awake()
    {
        monsterTr = GetComponent<Transform>();
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        //(NavMeshAgent)
        //monster�� ���� NavMeshAgent ������Ʈ���� �߰��� ��� = Player�� ��ġ
        // agent.destination = playerTr.position;
        // = agent.SetDestination(playerTr.position);
        // �ٷ� ���� ����

        // (�ִϸ��̼� ����)
        anim = GetComponent<Animator>();

        //(Monset ����,�� �´� ȿ��)
        bloodEffect = Resources.Load<GameObject>("BloodSprayEffect");
    }

    //(���� ���� �ӽ� ����(+enum))
    // 0.3f �ð� ��ŭ(���� ����) ���� �ൿ ���¸� üũ
    IEnumerator CheckMonsterState()
    {
        // 0.3�� ���� �����ϴ� ���� ����� �纸
        while (!isDie)
        {
            yield return new WaitForSeconds(0.3f);
            
            //Monster�� Player������ �Ÿ� ����
            float distance = Vector3.Distance(playerTr.position, monsterTr.position);

            //Delegate Ȱ��
            if (state == State.DIE) yield break;

            //���� �����Ÿ� ������ ���Դ��� üũ
            if (distance <= attackDist)
            { state = State.ATTACK; }

            //���� �����Ÿ� ������ ���Դ��� üũ
            else if (distance <= traceDist)
            { state = State.TRACE; }

            //���� �����Ÿ� ������ ���Դ��� üũ
            else
            { state = State.IDLE; }
        }
    }
    //(Monster �ൿ ����)
    //���¿� ���� �ൿ�� �����ϴ� �Լ�
    IEnumerator MonsterAction()
    {
        while (!isDie)
        {
            switch (state)
            {
                // IDLE�����϶� ���� ����
                // isStopped ���� �������� true = ���� ����
                case State.IDLE:
                    agent.isStopped = true;

                    // (�ִϸ��̼� ����)
                    // anim.SetBool("IsTrace", false);

                    //(Monster ���� ��ƾ)
                    anim.SetBool(hashTrace, false);
                    break;

                // TRACE ���� ����
                // ��������� ��ġ�� �̵�
                // isStopped ���� �������� false = ���� ����
                case State.TRACE:
                    agent.SetDestination(playerTr.position);
                    agent.isStopped = false;
                    // (�ִϸ��̼� ����)
                    // anim.SetBool("IsTrace", true);

                    //(Monster ���� ��ƾ)
                    anim.SetBool(hashTrace, true);
                    anim.SetBool(hashAttack, false);
                    break;

                // ATTACK ����
                case State.ATTACK:
                    //(Monster ���� ��ƾ)
                    anim.SetBool(hashAttack, true);
                    break;

                // DIE ����
                case State.DIE:
                    //Delegate Ȱ��
                    isDie = true;
                    agent.isStopped = true;
                    anim.SetTrigger(hashDie);
                    GetComponent<CapsuleCollider>().enabled = false;

                    // ������Ʈ Ǯ Ȱ��
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
        //(Monset ����,�� �´� ȿ��)
        if (coll.collider.CompareTag("Bullet"))
        {
            Destroy(coll.gameObject);

            //(Monset ����,�� �´� ȿ��)
            anim.SetTrigger(hashHit);

            //(Monset ����,�� �´� ȿ��)
            //Bullet�浹 ����
            Vector3 pos = coll.GetContact(0).point;
            //Bullet �浹 ������ ���� ����
            Quaternion rot = Quaternion.LookRotation(-coll.GetContact(0).normal);
            //���� ȿ�� ���� �Լ�
            ShowBloodEffect(pos, rot);

            //Delegate Ȱ��
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

    //(Monset ����,�� �´� ȿ��)
    void ShowBloodEffect(Vector3 pos, Quaternion rot)
    {
        GameObject blood = Instantiate<GameObject>(bloodEffect, pos, rot, monsterTr);
        Destroy(blood, 1.0f);
    }

    //(���� ���� �ӽ� ����(+enum))
    void OnDrawGizmos()
    {
        // ���� �����Ÿ� ǥ��
        if (state == State.TRACE)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, traceDist);
        }

        //���� �����Ÿ� ǥ��
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
