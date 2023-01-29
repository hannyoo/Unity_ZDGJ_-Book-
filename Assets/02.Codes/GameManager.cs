using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Transform> points = new List<Transform>();

    //Object Pooling
    public List<GameObject> monsterPool = new List<GameObject>();

    //Object Pooling ������ �ִ� monster ��
    public int maxMonsters = 10;

    public GameObject monster;
    public float createTime = 3.0f;
    private bool isGameOver;

    public bool IsGameOver
    {
        get{ return isGameOver; }
        set
        { 
            isGameOver = value;
            if(isGameOver)
            { CancelInvoke("CreateMonster"); }
        }
    }

    // Singltone Ȱ��
    // ���� instance ����
    public static GameManager instance = null;

    //���ھ� �ؽ�Ʈ ����
    public TMP_Text scoreText;
    //���ھ� �ؽ�Ʈ ���� _ ���� ���� ����
    private int totScore = 0;

    // Singltone Ȱ��    
    void Awake()
    // ���� ���� ����Ǵ� Awake �Լ��� �Ҵ�
    {
        if (instance== null)
        // instance�� �Ҵ���� ���� ��
        { instance = this; }
        // this�� �Ҵ��Ѵ�.   

        else if (instance != this)
        // instance�� �ٸ� ��(���� ������ Ŭ�����϶�)
        { Destroy(this.gameObject); }
        // ������

        //�ٸ� ������ �Ѿ�� �������� �ʰ� ������
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        //Object Pooling ����
        CreateMonsterPool();

        // Hierachy Component ����
        Transform spawnPointGroup = GameObject.Find("SpawnPointGrp")?.transform;
        // Hierachy SpawnPointGroup ���� points ����
        // spawnPointGroup?.GetComponentsInChildren < Transform>(true,points);

        foreach (Transform point in spawnPointGroup)
        { points.Add(point); }
       
        InvokeRepeating("CreateMonster", 2.0f, createTime);

        // playerPrefs ������ ����
        totScore = PlayerPrefs.GetInt("TOT_SCORE,0");

        DisplayScore(0);
    }

    void CreateMonster()
    {
        int idx = Random.Range(0, points.Count);

        // ���� ������ ����
        // Instantiate(monster, points[idx].position, points[idx].rotation);

        //Object Pool���� ���� ����
        GameObject _monster = GetMonsterInPool();
        // ������ ������ ��ġ�� ȸ�� ����
        _monster?.transform.SetPositionAndRotation(points[idx].position, points[idx].rotation);

        _monster?.SetActive(true);

    }

    //Object Pool ��� ������ ���� �����Ͽ� ��ȯ
    public GameObject GetMonsterInPool()
    {
        foreach(var _monster in monsterPool)
        { if(_monster.activeSelf == false)
            { return _monster; }
        }
        return null;
    }
    //Object Pooling
    void CreateMonsterPool()
    {
        for(int i=0; i<maxMonsters; i++)
        {
            // ���� ����
            var _monster = Instantiate<GameObject>(monster);
            // ���� �̸� ����
            _monster.name = $"Monster_{i:00}";
            // ���� ��Ȱ��ȭ
            _monster.SetActive(false);

            // ������ ���� Object Pool�� �߰�dd
            monsterPool.Add(_monster);
        }
    }

    //���ھ� �ؽ�Ʈ ����
    public void DisplayScore(int score)
    {
        totScore += score;
        scoreText.text = $"<color=#00ff00>SCORE  : </color><color=#ff0000> {totScore:#,##0} </color>";

        // playerPrefs ������ ����
        PlayerPrefs.SetInt("TOT_SCORE", totScore);
    }
}
