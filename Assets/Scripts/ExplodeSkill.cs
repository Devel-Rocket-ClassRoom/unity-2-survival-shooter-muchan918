using System.Collections;
using UnityEngine;

public class ExplodeSkill : MonoBehaviour, ISkill
{
    public float SkillInterval { get; set; } = 5f;
    public float LastSkillTime { get; set; } = 0f;

    public ParticleSystem explodeEffect;
    public float explodeRadius = 10f;
    public float explodeDamage = 300f;
    public LayerMask targetLayer;

    private AudioSource audioSource;
    public AudioClip explodeClip;

    private void Awake()
    {
        LastSkillTime = -SkillInterval;
        audioSource = GetComponentInParent<AudioSource>();
    }

    public void Use()
    {
        if (Time.time > LastSkillTime + SkillInterval)
        {
            LastSkillTime = Time.time;
            StartCoroutine(Explode());
        }
    }

    private IEnumerator Explode()
    {
        explodeEffect.Play();
        audioSource.PlayOneShot(explodeClip);

        var colliders = Physics.OverlapSphere(transform.position, explodeRadius, targetLayer);
        foreach (var col in colliders)
        {
            var target = col.GetComponent<LivingEntity>();
            if (target != null)
            {
                target.OnDamage(explodeDamage, transform.position, Vector3.up);
            }
        }

        yield return new WaitForSeconds(1f);
        explodeEffect.Stop();
    }
}
