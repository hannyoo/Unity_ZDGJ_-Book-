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

    //Object Pooling 생성할 최대 monster 수
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

    // Singltone 활용
    // 변수 instance 선언
    public static GameManager instance = null;

    //스코어 텍스트 연결
    public TMP_Text scoreText;
    //스코어 텍스트 연결 _ 누적 점수 변수
    private int totScore = 0;

    // Singltone 활용    
    void Awake()
    // 제일 먼저 실행되는 Awake 함수에 할당
    {
        if (instance== null)
        // instance가 할당되지 않을 때
        { instance = this; }
        // this에 할당한다.   

        else if (instance != this)
        // instance가 다를 때(새로 생성된 클래스일때)
        { Destroy(this.gameObject); }
        // 제거함

        //다른 씬으로 넘어가도 삭제되지 않고 유지됨
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        //Object Pooling 생성
        CreateMonsterPool();

        // Hierachy Component 추출
        Transform spawnPointGroup = GameObject.Find("SpawnPointGrp")?.transform;
        // Hierachy SpawnPointGroup 하위 points 추출
        // spawnPointGroup?.GetComponentsInChildren < Transform>(true,points);

        foreach (Transform point in spawnPointGroup)
        { points.Add(point); }
       
        InvokeRepeating("CreateMonster", 2.0f, createTime);

        // playerPrefs 데이터 저장
        totScore = PlayerPrefs.GetInt("TOT_SCORE,0");

        DisplayScore(0);
    }

    void CreateMonster()
    {
        int idx = Random.Range(0, points.Count);

        // 몬스터 프리팹 생성
        // Instantiate(monster, points[idx].position, points[idx].rotation);

        //Object Pool에서 몬스터 추출
        GameObject _monster = GetMonsterInPool();
        // 추출한 몬스터의 위치와 회전 설정
        _monster?.transform.SetPositionAndRotation(points[idx].position, points[idx].rotation);

        _monster?.SetActive(true);

    }

    //Object Pool 사용 가능한 몬스터 추출하여 반환
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
            // 몬스터 생성
            var _monster = Instantiate<GameObject>(monster);
            // 몬스터 이름 지정
            _monster.name = $"Monster_{i:00}";
            // 몬스터 비활성화
            _monster.SetActive(false);

            // 생성한 몬스터 Object Pool에 추가dd
            monsterPool.Add(_monster);
        }
    }

    //스코어 텍스트 연결
    public void DisplayScore(int score)
    {
        totScore += score;
        scoreText.text = $"<color=#00ff00>SCORE  : </color><color=#ff0000> {totScore:#,##0} </color>";

        // playerPrefs 데이터 저장
        PlayerPrefs.SetInt("TOT_SCORE", totScore);
    }
}
