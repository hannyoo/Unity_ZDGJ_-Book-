using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelegatePRTC : MonoBehaviour
{
    // delegate선언 / 매개변수(float a, float b)
    delegate float SumHandler(float a, float b);
    // delegate타입 변수 선언
    SumHandler sumHandler;

    // 덧셈 연산 함수
    float Sum(float a,float b)
    { return a * b; }

    void Start()
    {
        // delegate 변수에 Sum함수 연결
        sumHandler = Sum;
        // delegate 실행
        float sum = sumHandler(10.0f, 5.0f);
        // 출력
        Debug.Log($"Sum = {sum}");

        // delegate에 람다식 선언
        sumHandler = (float a, float b) => (a + b);
        float sum2 = sumHandler(10.0f,5.0f);
        Debug.Log($"Sum2 = {sum2}");

        // delegate 변수에 무명 메서드 연결
        sumHandler = delegate(float a,float b) { return a + b; };
        float sum3 = sumHandler(2.0f, 3.0f);
        Debug.Log($"Sum3 = {sum3}");

    }
}
