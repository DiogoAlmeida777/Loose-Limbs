using UnityEngine;
using UnityEngine.AI;

public class BossRuleBasedEnemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform meleePoint;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Animator animator;

    [Header("Movement")]
    [SerializeField] private float detectionRange = 25f;
    [SerializeField] private float meleeRange = 3f;
    [SerializeField] private float rangedRange = 18f;
    [SerializeField] private float preferredDistance = 9f;

    [Header("Melee Attack")]
    [SerializeField] private float meleeCooldown = 2f;
    [SerializeField] private float meleeRadius = 2.2f;
    [SerializeField] private float meleeDamage = 20f;
    [SerializeField] private LayerMask playerLayer;

    [Header("Ranged Attack")]
    [SerializeField] private float shootCooldown = 2.5f;
    [SerializeField] private float aimHeight = 1.2f;
    [SerializeField] private LayerMask obstacleLayer;

    [Header("Audio")]
    [SerializeField] private AudioSource rangedAttackAudio;
    [SerializeField] private AudioSource meleeSwingAudio;
    [SerializeField] private AudioSource meleeHitAudio;

    [Header("AI Mode")]
    [SerializeField] private bool useAutonomousAI = false;

    private float nextPathUpdateTime;
    [SerializeField] private float pathUpdateInterval = 0.25f;

    private NavMeshAgent agent;
    private float meleeTimer;
    private float shootTimer;

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
        if (!useAutonomousAI) return;

        if (player == null || agent == null) return;

        meleeTimer = Mathf.Max(0f, meleeTimer - Time.deltaTime);
        shootTimer = Mathf.Max(0f, shootTimer - Time.deltaTime);

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > detectionRange)
        {
            agent.isStopped = true;
            UpdateAnimation();
            return;
        }

        LookAtPlayer();

        if (distanceToPlayer <= meleeRange)
        {
            agent.isStopped = true;
            TryMeleeAttack();
        }
        else if (distanceToPlayer <= rangedRange)
        {
            if (HasLineOfSight())
            {
                KeepPreferredDistance(distanceToPlayer);
                TryRangedAttack();
            }
            else
            {
                ChasePlayer();
            }
        }
        else
        {
            ChasePlayer();
        }

        UpdateAnimation();
    }

    private void UpdateTimers()
    {
        meleeTimer = Mathf.Max(0f, meleeTimer - Time.deltaTime);
        shootTimer = Mathf.Max(0f, shootTimer - Time.deltaTime);
    }

    public void ChasePlayer()
    {
        UpdateTimers();

        if (player == null)
        {
            Debug.LogWarning("CHASE FAIL: player is null");
            return;
        }

        if (agent == null)
        {
            Debug.LogWarning("CHASE FAIL: agent is null");
            return;
        }

        if (!agent.enabled)
        {
            Debug.LogWarning("CHASE FAIL: agent disabled");
            return;
        }

        if (!agent.isOnNavMesh)
        {
            Debug.LogWarning("CHASE FAIL: boss is not on NavMesh");
            return;
        }

        agent.updatePosition = true;
        agent.updateRotation = true;
        agent.isStopped = false;

        if (agent.speed <= 0f)
        {
            agent.speed = 3f;
        }

        if (agent.acceleration <= 0f)
        {
            agent.acceleration = 8f;
        }

        if (Time.time < nextPathUpdateTime)
        {
            return;
        }

        nextPathUpdateTime = Time.time + pathUpdateInterval;

        Vector3 targetPosition;

        if (NavMesh.SamplePosition(player.position, out NavMeshHit hit, 5f, NavMesh.AllAreas))
        {
            targetPosition = hit.position;
        }
        else
        {
            Debug.LogWarning("CHASE FAIL: could not find NavMesh near player");
            return;
        }

        bool success = agent.SetDestination(targetPosition);

        Debug.Log(
            "CHASE called"
            + " | success: " + success
            + " | destination: " + targetPosition
            + " | isStopped: " + agent.isStopped
            + " | speed: " + agent.speed
            + " | velocity: " + agent.velocity
            + " | hasPath: " + agent.hasPath
            + " | pathPending: " + agent.pathPending
            + " | pathStatus: " + agent.pathStatus
            + " | remainingDistance: " + agent.remainingDistance
        );

        UpdateAnimation();
    }

    public void RetreatFromPlayer()
    {
        UpdateTimers();

        if (player == null || agent == null) return;

        agent.isStopped = false;

        Vector3 directionAway = transform.position - player.position;
        directionAway.y = 0f;
        directionAway.Normalize();

        Vector3 target = transform.position + directionAway * 4f;

        if (NavMesh.SamplePosition(target, out NavMeshHit hit, 4f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }

        UpdateAnimation();
    }

    private void KeepPreferredDistance(float distanceToPlayer)
    {
        if (distanceToPlayer > preferredDistance + 1f)
        {
            ChasePlayer();
            return;
        }

        if (distanceToPlayer < preferredDistance - 1f)
        {
            RetreatFromPlayer();
            return;
        }

        agent.isStopped = true;
    }

    public void TryMeleeAttack()
    {
        UpdateTimers();

        if (meleeTimer > 0f) return;

        meleeTimer = meleeCooldown;

        if (animator != null)
        {
            animator.SetTrigger("meleeAttack");
        }

        PlayAudio(meleeSwingAudio);

        Vector3 attackPosition = meleePoint != null
            ? meleePoint.position
            : transform.position + transform.forward;

        Collider[] hits = Physics.OverlapSphere(
            attackPosition,
            meleeRadius,
            playerLayer
        );

        foreach (Collider hit in hits)
        {
            Hurtbox hurtbox = hit.GetComponentInParent<Hurtbox>();

            if (hurtbox != null)
            {
                hurtbox.OnHit(meleeDamage);

                PlayAudio(meleeHitAudio);

                Debug.Log("Boss melee hit player.");
                return;
            }
        }

        Debug.Log("Boss melee missed.");
    }

    public void TryRangedAttack()
    {
        UpdateTimers();

        if (shootTimer > 0f)
        {
            Debug.Log("Ranged failed: cooldown " + shootTimer);
            return;
        }

        if (player == null)
        {
            Debug.LogWarning("Ranged failed: player null");
            return;
        }

        if (projectilePrefab == null)
        {
            Debug.LogWarning("Ranged failed: projectile null");
            return;
        }

        if (shootPoint == null)
        {
            Debug.LogWarning("Ranged failed: shootPoint null");
            return;
        }

        if (!HasLineOfSight())
        {
            Debug.LogWarning("Ranged failed: no line of sight");
            return;
        }

        Vector3 targetPosition = player.position + Vector3.up * aimHeight;
        Vector3 direction = targetPosition - shootPoint.position;

        if (direction.sqrMagnitude <= 0.01f)
        {
            Debug.LogWarning("Ranged failed: direction too small");
            return;
        }

        Quaternion rotation = Quaternion.LookRotation(direction.normalized);

        Vector3 spawnPosition = shootPoint.position + direction.normalized * 1.5f;
        Instantiate(projectilePrefab, spawnPosition, rotation);

        PlayAudio(rangedAttackAudio);

        if (animator != null)
        {
            animator.SetTrigger("rangedAttack");
        }

        shootTimer = shootCooldown;
        Debug.Log("Boss shot projectile");
    }

    public void StopMoving()
    {
        UpdateTimers();

        if (agent != null)
        {
            agent.isStopped = true;
        }

        UpdateAnimation();
    }

    public bool CanSeePlayer()
    {
        return HasLineOfSight();
    }

    private bool HasLineOfSight()
    {
        if (shootPoint == null || player == null) return false;

        Vector3 start = shootPoint.position;
        Vector3 target = player.position + Vector3.up * aimHeight;
        Vector3 direction = target - start;

        if (Physics.Raycast(start, direction.normalized, out RaycastHit hit, direction.magnitude, obstacleLayer))
        {
            return false;
        }

        return true;
    }

    private void LookAtPlayer()
    {
        if (player == null) return;

        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.01f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * 6f
        );
    }

    private void UpdateAnimation()
    {
        if (animator == null || agent == null) return;

        bool isMoving = agent.velocity.magnitude > 0.1f && !agent.isStopped;

        animator.SetBool("isMoving", isMoving);
    }

    private void PlayAudio(AudioSource audioSource)
    {
        if (audioSource == null) return;
        if (audioSource.clip == null) return;

        audioSource.PlayOneShot(audioSource.clip);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector3 attackPosition = meleePoint != null
            ? meleePoint.position
            : transform.position + transform.forward;

        Gizmos.DrawWireSphere(attackPosition, meleeRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, rangedRange);
    }
}