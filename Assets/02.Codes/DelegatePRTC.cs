using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelegatePRTC : MonoBehaviour
{
    // delegate���� / �Ű�����(float a, float b)
    delegate float SumHandler(float a, float b);
    // delegateŸ�� ���� ����
    SumHandler sumHandler;

    // ���� ���� �Լ�
    float Sum(float a,float b)
    { return a * b; }

    void Start()
    {
        // delegate ������ Sum�Լ� ����
        sumHandler = Sum;
        // delegate ����
        float sum = sumHandler(10.0f, 5.0f);
        // ���
        Debug.Log($"Sum = {sum}");

        // delegate�� ���ٽ� ����
        sumHandler = (float a, float b) => (a + b);
        float sum2 = sumHandler(10.0f,5.0f);
        Debug.Log($"Sum2 = {sum2}");

        // delegate ������ ���� �޼��� ����
        sumHandler = delegate(float a,float b) { return a + b; };
        float sum3 = sumHandler(2.0f, 3.0f);
        Debug.Log($"Sum3 = {sum3}");

    }
}
