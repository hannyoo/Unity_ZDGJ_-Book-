using System.Collections;
using UnityEngine;

//2. 컴포넌트 삭제 방지 어트리뷰트 
[RequireComponent(typeof(AudioSource))]
public class FireCtrl : MonoBehaviour
{
    //(Bullet) Bullet 프리팹 / Bullet 발사 위치
    public GameObject bullet;
    public Transform firePos;

    //(오디오) 오디오 음원
    public AudioClip fireSfx;

    //(오디오) AudioSource 컴포넌트를 저장할 변수
    private new AudioSource audio;

    //(총구 화염)
    private MeshRenderer muzzleFlash;

    void Start()
    {
        audio = GetComponent<AudioSource>();

        //(총구 화염) FirePos 하위 muzzleFlash에 Material 컴포넌트 할당/ 비활성화
        muzzleFlash = firePos.GetComponentInChildren<MeshRenderer>();
        muzzleFlash.enabled = false;
    }
    void Update()
    {
        //(Bullet) 마우스 0번 버튼(좌클릭) 했을때 Fire함수 실행
        if (Input.GetMouseButtonDown(0))
        { Fire(); }
    }
    void Fire()
    {
        //(Bullet)  Instantiate 생성(생성할 객체, 위치, 회전)
        Instantiate(bullet, firePos.position, firePos.rotation);

        //(오디오) 오디오 소스 재생
        // AudioSource.PlayOneShot(오디오클립, 볼륨);
        audio.PlayOneShot(fireSfx, 1.0f);

        //(coroutine을 활용 화염 블링크) 코루틴 함수 호출
        StartCoroutine(BlinkMuzzleFlash());
    }
    //(coroutine을 활용 화염 블링크) 코루틴 함수 
    IEnumerator BlinkMuzzleFlash()
    {
        //(총구 화염 오프셋 불규칙화_실감나도록) 좌표값을 범위 내 랜덤으로 생성
        Vector2 offset = new Vector2(Random.Range(0, 2), Random.Range(0, 2) * 0.5f);
        muzzleFlash.material.mainTextureOffset = offset;
        //(총구 화염 오프셋 불규칙화_실감나도록) 회전 변경
        float angle = Random.Range(0, 360);
        muzzleFlash.transform.localRotation = Quaternion.Euler(0, 0, angle);
        //(총구 화염 오프셋 불규칙화_실감나도록) 크기 변경
        float scale = Random.Range(1.0f, 2.0f);
        muzzleFlash.transform.localScale = Vector3.one * scale;

        //(coroutine을 활용 화염 블링크) muzzleFlash 활성화
        muzzleFlash.enabled = true;
        //(coroutine을 활용 화염 블링크) 0.2초 대기하는 동안 Message Loop로 제어권 양보
        yield return new WaitForSeconds(0.2f);
        //(coroutine을 활용 화염 블링크) 비활성화
        muzzleFlash.enabled = false;
    }
}
