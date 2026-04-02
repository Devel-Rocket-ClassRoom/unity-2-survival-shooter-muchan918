using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform fireTransform;
    public ParticleSystem gunParticles;
    public LayerMask targetLayer;

    public AudioClip gunShotClip;

    private LineRenderer bulletLineEffect;
    private AudioSource gunAudioPlayer;

    private float fireDistance = 50f;
    private float lastFireTime = 0f;
    private float fireInterval = 0.12f;

    private Coroutine coShot;

    private void Awake()
    {
        bulletLineEffect = GetComponent<LineRenderer>();
        gunAudioPlayer = GetComponent<AudioSource>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // 발사 관리
    public void Fire()
    {
        if (Time.time > lastFireTime + fireInterval)
        {
            Shot();
            lastFireTime = Time.time;
        }
    }

    // 총알 발사
    public void Shot()
    {
        Vector3 hitPosition = Vector3.zero;

        Ray ray = new Ray(fireTransform.position, fireTransform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, fireDistance, targetLayer))
        {
            hitPosition = hit.point;

            // 데미지 처리하기
            var target = hit.collider.GetComponent<LivingEntity>();
            if (target != null)
            {
                // ray로 충돌하는 충돌체의 법선은 충돌체의 모양에 따라 달라진다.
                target.OnDamage(20f, hit.point, hit.normal);
            }
        }
        else // 안맞았을때
        {
            hitPosition = fireTransform.position + fireTransform.forward * fireDistance;
        }

        if (coShot != null)
        {
            StopCoroutine(coShot);
            coShot = null;
        }

        coShot = StartCoroutine(CoShotEffect(hitPosition));
    }

    // 총알 발사 효과
    private IEnumerator CoShotEffect(Vector3 hitPosition)
    {
        gunParticles.Play();
        gunAudioPlayer.PlayOneShot(gunShotClip);

        bulletLineEffect.SetPosition(0, fireTransform.position);
        bulletLineEffect.SetPosition(1, hitPosition);
        bulletLineEffect.enabled = true;

        yield return new WaitForSeconds(0.03f);

        bulletLineEffect.enabled = false;
        coShot = null;
    }
}
