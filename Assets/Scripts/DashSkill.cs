using System.Collections;
using UnityEngine;

public class DashSkill : MonoBehaviour, ISkill
{
    public float SkillInterval { get; set; } = 0.5f;
    public float LastSkillTime { get; set; }

    public float dashForce = 10f;
    public ParticleSystem dashEffect;

    private Rigidbody rb;
    private PlayerInput playerInput;

    public AudioClip dashClip;
    private AudioSource audioSource;


    private void Awake()
    {
        LastSkillTime = -SkillInterval;
        rb = GetComponentInParent<Rigidbody>();
        playerInput = GetComponentInParent<PlayerInput>();
        audioSource = GetComponentInParent<AudioSource>();
    }

    public void Use()
    {
        if (Time.time > LastSkillTime + SkillInterval)
        {
            LastSkillTime = Time.time;
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        Vector3 dashDirection = new Vector3(playerInput.MoveX, 0f, playerInput.MoveY).normalized;

        // 이동 방향이 없으면 앞방향으로 대시
        if (dashDirection == Vector3.zero)
            dashDirection = transform.parent.forward;

        dashEffect.transform.rotation = Quaternion.LookRotation(-dashDirection);
        dashEffect.transform.position = transform.position + (-dashDirection * 0.4f);
        audioSource.PlayOneShot(dashClip);
        dashEffect.Play();

        rb.AddForce(dashDirection * dashForce, ForceMode.Impulse);

        yield return new WaitForSeconds(0.2f);
        dashEffect.Stop();
        rb.linearVelocity = Vector3.zero;
    }
}