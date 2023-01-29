using UnityEngine;


public class FollowCamera : MonoBehaviour
{
    public Transform targetTr;
    private Transform camTr;

    [Range(2.0f, 20.0f)]
    public float distance = 10.0f;

    [Range(0.0f, 10.0f)]
    public float height = 2.0f;

    public float damping = 10.0f; //2.���ɶ� �����ӵ� ����

    private Vector3 velocity = Vector3.zero; //3.�ε巴�� �̵��ϱ� ���� Vector3.SmoothDamp���� ����� ����

    public float targetOffset; //4.ī�޶� LookAt�� Offset
    void Start()
    {
        camTr = GetComponent<Transform>();
    }

    void LateUpdate() // 1.Update �Լ� ���Ŀ� ����
    {
        // 2. Vector3 pos ���� ���� �� �Ҵ����� targetTr�� �Ÿ�����
        // Vector3 pos = tar.position = transform.position
        //               + (-targetTr.forward * distance)
        //               + (Vector3.up * height);

        //3.
        Vector3 pos = targetTr.position
                      + (-targetTr.forward * distance)
                      + (Vector3.up * height);

        //3. SmoothDamp �Լ��� �̿��� ��ġ ����
        camTr.position = Vector3.SmoothDamp(camTr.position, pos, ref velocity, damping);

        //2.Slerp ���� ���� ���� �Լ� ��� - �ε巴�� ��ġ����
        // camTr.position = Vector3.Slerp(camTr.position, pos, Time.deltaTime * damping);


        camTr.LookAt(targetTr.position + (targetTr.up * targetOffset)); //2.�ǹ� ��ǥ�� ȸ��
                                                                        //4. + (targetTr.up * targetOffset) �ǹ� ��ǥ�� ���ؼ� ȸ��

    }
}
