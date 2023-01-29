using UnityEngine;

public class BarrelCtrl : MonoBehaviour
{
    public GameObject expEffect;

    private Transform tr;
    private Rigidbody rd;

    private int hitCount = 0;

    //2. 무작위로 적용할 텍스쳐 배열
    public Texture[] textures;
    //2.하위에 있는 MeshComponent 자장할 변수
    private new MeshRenderer renderer;

    //3. 폭발 반경 
    public float radius = 10.0f;


    void Start()
    {
        tr = GetComponent<Transform>();
        rd = GetComponent<Rigidbody>();

        //2.하위에 MeshComponent 추출
        renderer = rd.GetComponentInChildren<MeshRenderer>();
        //2. 텍스쳐 배열 범위
        int idx = Random.Range(0, textures.Length);
        //2. 텍스쳐 지정
        renderer.material.mainTexture = textures[idx];

    }

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.CompareTag("Bullet")) //1. tag가 Bullet 충돌 했을때
        {
            if (++hitCount == 5)                // hitCount가 5가 되면
            { ExpBarrel(); }                    // ExpBarrel 함수 실행
        }
    }

    void ExpBarrel()
    {
        //1. exp 게임오브젝스 선언 = 생성(생성할 객체. 위치, 회전)
        GameObject exp = Instantiate(expEffect, tr.position, Quaternion.identity);
        //1. 0.5초 후 exp 제거
        Destroy(exp, 5.0f);

        //1. rd를 1로 할당
        // rd.mass = 1.0f;
        //1. rd에 위로 힘을 가함
        // rd.AddForce(Vector3.up * 1500.0f);

        //3. 간접 폭발 물리력 함수
        IndirectDamage(tr.position);

        //1. 3초후 제거
        Destroy(gameObject, 3.0f);
    }

    //3. 간접 폭발 물리력 전달 함수
    void IndirectDamage(Vector3 pos)
    {
        //3. OverlapSphere(원점, 반지름, 검출대상 레이어)
        //   - 범위 내의 검출될 갯수가 명확X - 메모리Garbage발생(최적화X) 
        // Collider[] colls = Physics.OverlapSphere(pos, radius, 1 << 3);

        //4.범위 내의 검출될 갯수가 명확하도록 배열로 선언 - Garbage 컬렉션 발생X(최적화O)
        // OverlapSphereNonAlloc 사용
        Collider[] colls = new Collider[10];

        Physics.OverlapSphereNonAlloc(pos, radius, colls, 1 << 3);

        foreach (var coll in colls)
        {
            rd = coll.GetComponent<Rigidbody>();

            rd.mass = 1.0f;

            rd.constraints = RigidbodyConstraints.None;

            rd.AddExplosionForce(1500.0f, pos, radius, 800.0f);
        }
    }

}