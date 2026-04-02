using System;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class Monster : LivingEntity
{
    public enum Status
    {
        Idle,
        Trace,
        Attack,
        Die,
    }

    public Transform target;
    public ParticleSystem hurtEffect;
    private NavMeshAgent agent;
    private Animator monsterAnimator;
    public Collider monsterCollider;
    public LayerMask targetLayer;

    public float attackDistance;
    public float attackInterval = 1f;
    private float lastAttackTime;

    private float damage;

    private Status currentStatus;

    private AudioSource monsterAudioSource;

    public Status CurrentStatus
    {
        get { return currentStatus; }
        set
        {
            var prevStatus = currentStatus;
            currentStatus = value;
            Debug.Log(currentStatus);

            switch (currentStatus)
            {
                case Status.Idle:
                    monsterAnimator.SetBool("HasTarget", false);
                    agent.isStopped = true;
                    break;
                case Status.Trace:
                    monsterAnimator.SetBool("HasTarget", true);
                    agent.isStopped = false;
                    break;
                case Status.Attack:
                    monsterAnimator.SetBool("HasTarget", true);
                    agent.isStopped = false;
                    break;
                case Status.Die:
                    break;
            }
        }
    }

    public void Setup(MonsterData data)
    {
        gameObject.SetActive(false);

        startingHealth = data.maxHP;
        damage = data.damage;
        agent.speed = data.speed;
        attackDistance = data.attackDistance;
        hurtClip = data.hurtClip;
        deathClip = data.deathClip;

        gameObject.SetActive(true);
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        monsterAnimator = GetComponent<Animator>();
        monsterAudioSource = GetComponent<AudioSource>();
        monsterCollider = GetComponent<Collider>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        // NavMesh를 사용할거면 이걸 추가해야한다
        agent.enabled = true;
        agent.isStopped = false;
        agent.ResetPath();
        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position);
        }

        monsterCollider.enabled = true;

        CurrentStatus = Status.Idle;
    }

    private void Update()
    {
        // 상태가 업데이트 될때마다 실행해야될 함수 -> 메서드로 만들어서 실행
        switch (currentStatus)
        {
            case Status.Idle:
                UpdateIdle();
                break;
            case Status.Trace:
                UpdateTrace();
                break;
            case Status.Attack:
                UpdateAttack();
                break;
            case Status.Die:
                UpdateDie();
                break;
        }
    }

    private void UpdateIdle()
    {
        if (target != null)
        {
            CurrentStatus = Status.Trace;
            return;
        }
    }

    private void UpdateTrace()
    {
        if (target == null)
        {
            CurrentStatus = Status.Idle;
            return;
        }

        var LookAt = target.position;
        LookAt.y = transform.position.y;
        transform.LookAt(LookAt);

        if (target != null && Vector3.Distance(target.position, transform.position) < attackDistance)
        {
            CurrentStatus = Status.Attack;
            return;
        }

        agent.SetDestination(target.position);
    }

    private void UpdateAttack()
    {
        if (target == null)
        {
            CurrentStatus = Status.Idle;
            return;
        }

        if (Vector3.Distance(target.position, transform.position) >= attackDistance)
        {
            CurrentStatus = Status.Trace;
            return;
        }

        var lookAt = target.position;
        lookAt.y = transform.position.y;
        transform.LookAt(lookAt);

        if (Time.time > lastAttackTime + attackInterval)
        {
            lastAttackTime = Time.time;
            var livingEntity = target.GetComponent<LivingEntity>();
            if (livingEntity != null)
            {
                if (!livingEntity.IsDead)
                {
                    Debug.Log("Attacking");
                }
            }
        }
    }

    private void UpdateDie()
    {
        throw new NotImplementedException();
    }
}
