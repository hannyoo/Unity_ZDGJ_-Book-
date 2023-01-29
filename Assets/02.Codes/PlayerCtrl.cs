using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    private Transform tr;
    public float moveSpeed = 10f;
    public float turnSpeed = 200f;

    private Animation anim; //2.애니메이션 변수

    //(Monster)공격 설정
    private readonly float initHp = 100.0f;
    public float currHp;

    //Delegate 활용
    public delegate void PlayerDieHandler();
    public static event PlayerDieHandler OnPlayerDie;

    // Hpbar 변수
    private Image hpBar;

    //(Player 바라보는 방향 수정)
    IEnumerator Start()
    {
        // Hpbar 변수
        hpBar = GameObject.FindGameObjectWithTag("HPbar")?.GetComponent<Image>();
        //(Monster)공격 설정
        currHp = initHp;
        DisplayHealth();

        tr = GetComponent<Transform>();

        anim = GetComponent<Animation>(); //2.변수 할당
        anim.Play("Idle");  //2.Idle 애니메이션 실행

        //(Player 바라보는 방향 수정)
        turnSpeed = 0.0f;
        yield return new WaitForSeconds(0.3f);
        turnSpeed= 200f;

    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float r = Input.GetAxis("Mouse X");

        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);

        tr.Translate(moveDir * moveSpeed * Time.deltaTime);
        tr.Rotate(Vector3.up * turnSpeed * Time.deltaTime * r);

        PlayerAnim(h, v);

    }

    void PlayerAnim(float h,float v)
    {
        if (v >= 0.1f)
        { anim.CrossFade("RunF", 0.25f); }
        else if (v <= -0.1f)
        { anim.CrossFade("RunB", 0.25f); }
        else if (h >= 0.1f)
        { anim.CrossFade("RunR", 0.25f); }
        else if (h <= -0.1f)
        { anim.CrossFade("RunL", 0.25f); }
        else
        { anim.CrossFade("Idle", 0.25f); }
    }

    //(Monster)공격 설정
    void OnTriggerEnter(Collider coll)
    {
        if(currHp >= 0.0f && coll.CompareTag("Punch"))
        {
            currHp -= 10.0f;
            // Hpbar 변수
            Debug.Log($"Player hp = { currHp/initHp}");
            //Debug.Log($"Player hp = {currHp / initHp}");
            
            // Hpbar 변수
            DisplayHealth();



            if (currHp <= 0.0f)
            {
                PlayerDie(); 
            }

        }
    }

    void PlayerDie()
    { 
        Debug.Log("Player Die");

        //Delegate 활용
        // (Tag.Monster에게 Player 죽음을 알리고 공격,추적 중지)
        //Delegate 활용 GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        //Delegate 활용 foreach (GameObject monster in monsters)
        //Delegate 활용 { monster.SendMessage("OnPlayerDie",SendMessageOptions.DontRequireReceiver); }

        //Delegate 활용
        OnPlayerDie();

        //singletone 활용을 위해 변경
        //GameObject.Find("GameMgr").GetComponent<GameManager>().IsGameOver = true;
        GameManager.instance.IsGameOver = true;
    }

    // Hpbar연결
    void DisplayHealth()
    {
        hpBar.fillAmount = currHp / initHp;
    }
}

