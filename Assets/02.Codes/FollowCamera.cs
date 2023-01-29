using UnityEngine;


public class FollowCamera : MonoBehaviour
{
    public Transform targetTr;
    private Transform camTr;

    [Range(2.0f, 20.0f)]
    public float distance = 10.0f;

    [Range(0.0f, 10.0f)]
    public float height = 2.0f;

    public float damping = 10.0f; //2.마케라 반응속도 변수

    private Vector3 velocity = Vector3.zero; //3.부드럽게 이동하기 위한 Vector3.SmoothDamp에서 사용할 변수

    public float targetOffset; //4.카메라 LookAt의 Offset
    void Start()
    {
        camTr = GetComponent<Transform>();
    }

    void LateUpdate() // 1.Update 함수 이후에 실행
    {
        // 2. Vector3 pos 변수 선언 및 할당으로 targetTr의 거리설정
        // Vector3 pos = tar.position = transform.position
        //               + (-targetTr.forward * distance)
        //               + (Vector3.up * height);

        //3.
        Vector3 pos = targetTr.position
                      + (-targetTr.forward * distance)
                      + (Vector3.up * height);

        //3. SmoothDamp 함수를 이용한 위치 보정
        camTr.position = Vector3.SmoothDamp(camTr.position, pos, ref velocity, damping);

        //2.Slerp 구면 선형 보간 함수 사용 - 부드럽게 위치변경
        // camTr.position = Vector3.Slerp(camTr.position, pos, Time.deltaTime * damping);


        camTr.LookAt(targetTr.position + (targetTr.up * targetOffset)); //2.피벗 좌표로 회전
                                                                        //4. + (targetTr.up * targetOffset) 피벗 좌표를 향해서 회전

    }
}
