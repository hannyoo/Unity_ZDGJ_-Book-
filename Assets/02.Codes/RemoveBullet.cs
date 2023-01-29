using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
    public GameObject sparkEffect; //2.��ƼŬ ������ ����
    private void OnCollisionEnter(Collision coll)
    {
        //1. =(coll.gameObject.tag)
        //if (coll.collider.tag == "Bullet")
        //{ Destroy(coll.gameObject); }

        //2.CompareTag - ��õ! ����ȭ ����
        if (coll.collider.CompareTag("Bullet"))
        {
            //3. ó�� �浹���� ���� ����
            ContactPoint cp = coll.GetContact(0);
            //3. �浹�� �Ѿ��� ���� ���͸� ���ʹϾ� Ÿ������ ��ȯ
            Quaternion rot = Quaternion.LookRotation(-cp.normal);

            //4.spark ������ ����
            GameObject spark = Instantiate(sparkEffect, cp.point, rot);
            //4.spark 0.5���� ����
            Destroy(spark.gameObject, 0.5f);

            // 2.�����Ѵ�(��ü ��ƼŬ, coll ��ġ, ȸ������ )
            // Instantiate(sparkEffect, coll.transform.position, Quaternion.identity);
            // 1. �浹�� ������Ʈ ����
            Destroy(coll.gameObject);         
        }
    }
}
