using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 20.0f; //1.�Ѿ� ������
    public float force = 1500.0f; //1.�Ѿ��� ��
    private Rigidbody rd; //1.������

    void Start()
    {
        rd= GetComponent<Rigidbody>();
        rd.AddForce(transform.forward * force); //1. �Ѿ��� forward�� force�� ����
    }

    
}
