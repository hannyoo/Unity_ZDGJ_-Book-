using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    private Transform tr;
    public float moveSpeed = 10f;
    public float turnSpeed = 200f;

    private Animation anim; //2.�ִϸ��̼� ����

    //(Monster)���� ����
    private readonly float initHp = 100.0f;
    public float currHp;

    //Delegate Ȱ��
    public delegate void PlayerDieHandler();
    public static event PlayerDieHandler OnPlayerDie;

    // Hpbar ����
    private Image hpBar;

    //(Player �ٶ󺸴� ���� ����)
    IEnumerator Start()
    {
        // Hpbar ����
        hpBar = GameObject.FindGameObjectWithTag("HPbar")?.GetComponent<Image>();
        //(Monster)���� ����
        currHp = initHp;
        DisplayHealth();

        tr = GetComponent<Transform>();

        anim = GetComponent<Animation>(); //2.���� �Ҵ�
        anim.Play("Idle");  //2.Idle �ִϸ��̼� ����

        //(Player �ٶ󺸴� ���� ����)
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

    //(Monster)���� ����
    void OnTriggerEnter(Collider coll)
    {
        if(currHp >= 0.0f && coll.CompareTag("Punch"))
        {
            currHp -= 10.0f;
            // Hpbar ����
            Debug.Log($"Player hp = { currHp/initHp}");
            //Debug.Log($"Player hp = {currHp / initHp}");
            
            // Hpbar ����
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

        //Delegate Ȱ��
        // (Tag.Monster���� Player ������ �˸��� ����,���� ����)
        //Delegate Ȱ�� GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        //Delegate Ȱ�� foreach (GameObject monster in monsters)
        //Delegate Ȱ�� { monster.SendMessage("OnPlayerDie",SendMessageOptions.DontRequireReceiver); }

        //Delegate Ȱ��
        OnPlayerDie();

        //singletone Ȱ���� ���� ����
        //GameObject.Find("GameMgr").GetComponent<GameManager>().IsGameOver = true;
        GameManager.instance.IsGameOver = true;
    }

    // Hpbar����
    void DisplayHealth()
    {
        hpBar.fillAmount = currHp / initHp;
    }
}

