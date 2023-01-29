using UnityEngine;

public class BarrelCtrl : MonoBehaviour
{
    public GameObject expEffect;

    private Transform tr;
    private Rigidbody rd;

    private int hitCount = 0;

    //2. �������� ������ �ؽ��� �迭
    public Texture[] textures;
    //2.������ �ִ� MeshComponent ������ ����
    private new MeshRenderer renderer;

    //3. ���� �ݰ� 
    public float radius = 10.0f;


    void Start()
    {
        tr = GetComponent<Transform>();
        rd = GetComponent<Rigidbody>();

        //2.������ MeshComponent ����
        renderer = rd.GetComponentInChildren<MeshRenderer>();
        //2. �ؽ��� �迭 ����
        int idx = Random.Range(0, textures.Length);
        //2. �ؽ��� ����
        renderer.material.mainTexture = textures[idx];

    }

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.CompareTag("Bullet")) //1. tag�� Bullet �浹 ������
        {
            if (++hitCount == 5)                // hitCount�� 5�� �Ǹ�
            { ExpBarrel(); }                    // ExpBarrel �Լ� ����
        }
    }

    void ExpBarrel()
    {
        //1. exp ���ӿ������� ���� = ����(������ ��ü. ��ġ, ȸ��)
        GameObject exp = Instantiate(expEffect, tr.position, Quaternion.identity);
        //1. 0.5�� �� exp ����
        Destroy(exp, 5.0f);

        //1. rd�� 1�� �Ҵ�
        // rd.mass = 1.0f;
        //1. rd�� ���� ���� ����
        // rd.AddForce(Vector3.up * 1500.0f);

        //3. ���� ���� ������ �Լ�
        IndirectDamage(tr.position);

        //1. 3���� ����
        Destroy(gameObject, 3.0f);
    }

    //3. ���� ���� ������ ���� �Լ�
    void IndirectDamage(Vector3 pos)
    {
        //3. OverlapSphere(����, ������, ������ ���̾�)
        //   - ���� ���� ����� ������ ��ȮX - �޸�Garbage�߻�(����ȭX) 
        // Collider[] colls = Physics.OverlapSphere(pos, radius, 1 << 3);

        //4.���� ���� ����� ������ ��Ȯ�ϵ��� �迭�� ���� - Garbage �÷��� �߻�X(����ȭO)
        // OverlapSphereNonAlloc ���
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