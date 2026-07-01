using UnityEngine;
using UnityEngine.AI;

public class RandomMeleeNavMeshEnemy : MonoBehaviour
{
    private enum EnemyAction
    {
        Idle,
        ChasePlayer,
        WanderRandomly,
        DodgeSideways
    }

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Animator animator;
    [SerializeField] private DeltaTransform dt;

    [Header("Movement")]
    [SerializeField] private float detectionRange = 12f;
    [SerializeField] private float attackRange = 1.7f;
    [SerializeField] private float decisionInterval = 1.2f;
    [SerializeField] private float randomMoveRadius = 6f;
    [SerializeField] private float dodgeDistance = 3f;

    [Header("Attack")]
    [SerializeField] private float attackCooldown = 1.2f;
    [SerializeField] private float attackRadius = 1.2f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private LayerMask playerLayer;

    private NavMeshAgent agent;
    private float decisionTimer;
    private float attackTimer;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }
    }

    private void Update()
    {
        if (player == null || agent == null) return;

        decisionTimer -= Time.deltaTime;
        attackTimer -= Time.deltaTime;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            agent.isStopped = true;
            LookAtPlayer();
            TryAttack();
            return;
        }

        agent.isStopped = false;

        if (decisionTimer <= 0f)
        {
            ChooseRandomAction(distanceToPlayer);
            decisionTimer = decisionInterval;
        }

        UpdateAnimation();
    }

    private void ChooseRandomAction(float distanceToPlayer)
    {
        if (distanceToPlayer > detectionRange)
        {
            WanderRandomly();
            return;
        }

        EnemyAction action = (EnemyAction)Random.Range(0, 4);

        switch (action)
        {
            case EnemyAction.Idle:
                agent.isStopped = true;
                break;

            case EnemyAction.ChasePlayer:
                agent.isStopped = false;
                agent.SetDestination(player.position);
                break;

            case EnemyAction.WanderRandomly:
                WanderRandomly();
                break;

            case EnemyAction.DodgeSideways:
                DodgeSideways();
                break;
        }
    }

    private void WanderRandomly()
    {
        agent.isStopped = false;

        Vector3 randomDirection = Random.insideUnitSphere * randomMoveRadius;
        randomDirection += transform.position;
        randomDirection.y = transform.position.y;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, randomMoveRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    private void DodgeSideways()
    {
        agent.isStopped = false;

        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0f;
        directionToPlayer.Normalize();

        Vector3 sideDirection = Vector3.Cross(Vector3.up, directionToPlayer);

        if (Random.value > 0.5f)
        {
            sideDirection *= -1f;
        }

        Vector3 dodgeTarget = transform.position + sideDirection * dodgeDistance;

        if (NavMesh.SamplePosition(dodgeTarget, out NavMeshHit hit, dodgeDistance, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
    private void TryAttack()
    {
        if (attackTimer > 0f) return;

        attackTimer = attackCooldown;

        Vector3 attackPosition = attackPoint != null
            ? attackPoint.position
            : transform.position + transform.forward;

        Collider[] hits = Physics.OverlapSphere(
            attackPosition,
            attackRadius,
            playerLayer
        );

        foreach (Collider hit in hits)
        {
            Hurtbox hurtbox = hit.GetComponentInParent<Hurtbox>();

            if (hurtbox != null)
            {
                hurtbox.OnHit(damage);
                Debug.Log($"{gameObject.name} hit player for {damage} damage.");
                return;
            }

        }

        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        Debug.Log($"{gameObject.name} attacked but hit nothing.");
    }

    private void LookAtPlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.01f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * 10f
        );
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector3 attackPosition = attackPoint != null
            ? attackPoint.position
            : transform.position + transform.forward;

        Gizmos.DrawWireSphere(attackPosition, attackRadius);
    }

    private void UpdateAnimation()
    {
        if (animator == null || agent == null) return;

        animator.SetFloat("fwdSpeed", dt.fwdSpeed());
        animator.SetFloat("sideSpeed", dt.sideSpeed());

        bool isMoving = agent.velocity.magnitude > 0.1f && !agent.isStopped;
        animator.SetBool("isMoving", isMoving);
    }
}