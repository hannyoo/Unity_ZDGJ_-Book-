using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 20.0f; //1.총알 데미지
    public float force = 1500.0f; //1.총알의 힘
    private Rigidbody rd; //1.물리력

    void Start()
    {
        rd= GetComponent<Rigidbody>();
        rd.AddForce(transform.forward * force); //1. 총알이 forward로 force를 가함
    }

    
}
