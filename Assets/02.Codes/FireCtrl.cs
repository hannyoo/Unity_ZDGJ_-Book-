using System.Collections;
using UnityEngine;

//2. ������Ʈ ���� ���� ��Ʈ����Ʈ 
[RequireComponent(typeof(AudioSource))]
public class FireCtrl : MonoBehaviour
{
    //(Bullet) Bullet ������ / Bullet �߻� ��ġ
    public GameObject bullet;
    public Transform firePos;

    //(�����) ����� ����
    public AudioClip fireSfx;

    //(�����) AudioSource ������Ʈ�� ������ ����
    private new AudioSource audio;

    //(�ѱ� ȭ��)
    private MeshRenderer muzzleFlash;

    void Start()
    {
        audio = GetComponent<AudioSource>();

        //(�ѱ� ȭ��) FirePos ���� muzzleFlash�� Material ������Ʈ �Ҵ�/ ��Ȱ��ȭ
        muzzleFlash = firePos.GetComponentInChildren<MeshRenderer>();
        muzzleFlash.enabled = false;
    }
    void Update()
    {
        //(Bullet) ���콺 0�� ��ư(��Ŭ��) ������ Fire�Լ� ����
        if (Input.GetMouseButtonDown(0))
        { Fire(); }
    }
    void Fire()
    {
        //(Bullet)  Instantiate ����(������ ��ü, ��ġ, ȸ��)
        Instantiate(bullet, firePos.position, firePos.rotation);

        //(�����) ����� �ҽ� ���
        // AudioSource.PlayOneShot(�����Ŭ��, ����);
        audio.PlayOneShot(fireSfx, 1.0f);

        //(coroutine�� Ȱ�� ȭ�� ��ũ) �ڷ�ƾ �Լ� ȣ��
        StartCoroutine(BlinkMuzzleFlash());
    }
    //(coroutine�� Ȱ�� ȭ�� ��ũ) �ڷ�ƾ �Լ� 
    IEnumerator BlinkMuzzleFlash()
    {
        //(�ѱ� ȭ�� ������ �ұ�Ģȭ_�ǰ�������) ��ǥ���� ���� �� �������� ����
        Vector2 offset = new Vector2(Random.Range(0, 2), Random.Range(0, 2) * 0.5f);
        muzzleFlash.material.mainTextureOffset = offset;
        //(�ѱ� ȭ�� ������ �ұ�Ģȭ_�ǰ�������) ȸ�� ����
        float angle = Random.Range(0, 360);
        muzzleFlash.transform.localRotation = Quaternion.Euler(0, 0, angle);
        //(�ѱ� ȭ�� ������ �ұ�Ģȭ_�ǰ�������) ũ�� ����
        float scale = Random.Range(1.0f, 2.0f);
        muzzleFlash.transform.localScale = Vector3.one * scale;

        //(coroutine�� Ȱ�� ȭ�� ��ũ) muzzleFlash Ȱ��ȭ
        muzzleFlash.enabled = true;
        //(coroutine�� Ȱ�� ȭ�� ��ũ) 0.2�� ����ϴ� ���� Message Loop�� ����� �纸
        yield return new WaitForSeconds(0.2f);
        //(coroutine�� Ȱ�� ȭ�� ��ũ) ��Ȱ��ȭ
        muzzleFlash.enabled = false;
    }
}
