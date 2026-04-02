using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    public Image damaged;
    public Slider hpSlider;

    private AudioSource playerAudioSource;
    private Animator playerAnimator;

    public AudioClip hurtClip;
    public AudioClip deathClip;

    private PlayerMovement playerMovement;
    private PlayerShooter playerShooter;

    private void Awake()
    {
        playerAudioSource = GetComponent<AudioSource>();
        playerAnimator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShooter = GetComponent<PlayerShooter>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        damaged.gameObject.SetActive(false);
        playerMovement.enabled = true;
        playerShooter.enabled = true;
        hpSlider.value = Health / startingHealth;
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (!IsDead)
        {
            playerAudioSource.PlayOneShot(hurtClip);
        }

        StartCoroutine(Damaged());
        base.OnDamage(damage, hitPoint, hitNormal);
        hpSlider.value = Health / startingHealth;
    }

    private IEnumerator Damaged()
    {
        damaged.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.12f);
        damaged.gameObject.SetActive(false);
    }

    public override void Die()
    {
        if (IsDead)
        {
            return;
        }

        base.Die();

        playerAudioSource.PlayOneShot(deathClip);
        playerAnimator.SetTrigger("Die");

        playerMovement.enabled = false;
        playerShooter.enabled = false;
    }

    private void RestartLevel()
    {
        // 레벨 재시작 로직
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}

