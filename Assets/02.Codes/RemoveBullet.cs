using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
    public GameObject sparkEffect; //2.파티클 프리팹 선언
    private void OnCollisionEnter(Collision coll)
    {
        //1. =(coll.gameObject.tag)
        //if (coll.collider.tag == "Bullet")
        //{ Destroy(coll.gameObject); }

        //2.CompareTag - 추천! 최적화 용이
        if (coll.collider.CompareTag("Bullet"))
        {
            //3. 처음 충돌지점 정보 추출
            ContactPoint cp = coll.GetContact(0);
            //3. 충돌한 총알의 법선 벡터를 쿼터니언 타입으로 변환
            Quaternion rot = Quaternion.LookRotation(-cp.normal);

            //4.spark 프리팹 생성
            GameObject spark = Instantiate(sparkEffect, cp.point, rot);
            //4.spark 0.5초후 제거
            Destroy(spark.gameObject, 0.5f);

            // 2.생성한다(객체 파티클, coll 위치, 회전없이 )
            // Instantiate(sparkEffect, coll.transform.position, Quaternion.identity);
            // 1. 충돌한 오브젝트 제거
            Destroy(coll.gameObject);         
        }
    }
}
