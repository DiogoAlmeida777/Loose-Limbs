using UnityEngine;
using UnityEngine.AI;

public class RangedRuleBasedEnemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Animator animator;

    [Header("Movement")]
    [SerializeField] private float detectionRange = 18f;
    [SerializeField] private float preferredDistance = 8f;
    [SerializeField] private float tooCloseDistance = 5f;
    [SerializeField] private float retreatDistance = 4f;
    [SerializeField] private float strafeDistance = 3f;

    [Header("Shooting")]
    [SerializeField] private float shootCooldown = 1.5f;
    [SerializeField] private float aimHeight = 1.2f;

    [SerializeField] private LayerMask obstacleLayer;

    private NavMeshAgent agent;
    private float shootTimer;
    private float strafeTimer;

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

        shootTimer = Mathf.Max(0f, shootTimer - Time.deltaTime);
        strafeTimer = Mathf.Max(0f, strafeTimer - Time.deltaTime);

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > detectionRange)
        {
            agent.isStopped = true;
            UpdateAnimation();
            return;
        }

        LookAtPlayer();

        if (distanceToPlayer < tooCloseDistance)
        {
            RetreatFromPlayer();
        }
        else if (distanceToPlayer > preferredDistance)
        {
            ChasePlayer();
        }
        else
        {
            KeepDistanceAndShoot();
        }

        TryShoot(distanceToPlayer);
        UpdateAnimation();
    }

    private void ChasePlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);
    }

    private void RetreatFromPlayer()
    {
        agent.isStopped = false;

        Vector3 directionAway = transform.position - player.position;
        directionAway.y = 0f;
        directionAway.Normalize();

        Vector3 retreatTarget = transform.position + directionAway * retreatDistance;

        if (NavMesh.SamplePosition(retreatTarget, out NavMeshHit hit, retreatDistance, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    private void KeepDistanceAndShoot()
    {
        if (strafeTimer > 0f)
        {
            return;
        }

        strafeTimer = 1.5f;

        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0f;
        directionToPlayer.Normalize();

        Vector3 sideDirection = Vector3.Cross(Vector3.up, directionToPlayer);

        if (Random.value > 0.5f)
        {
            sideDirection *= -1f;
        }

        Vector3 strafeTarget = transform.position + sideDirection * strafeDistance;

        if (NavMesh.SamplePosition(strafeTarget, out NavMeshHit hit, strafeDistance, NavMesh.AllAreas))
        {
            agent.isStopped = false;
            agent.SetDestination(hit.position);
        }
    }

    private void TryShoot(float distanceToPlayer)
    {
        if (shootTimer > 0f)
        {
            return;
        }

        if (projectilePrefab == null)
        {
            Debug.LogError("Cannot shoot: projectilePrefab is null");
            return;
        }

        if (shootPoint == null)
        {
            Debug.LogError("Cannot shoot: shootPoint is null");
            return;
        }

        if (distanceToPlayer > detectionRange)
        {
            Debug.Log("Cannot shoot: player too far");
            return;
        }

        if (!HasLineOfSight())
        {
            return;
        }

        Vector3 targetPosition = player.position + Vector3.up * aimHeight;
        Vector3 direction = targetPosition - shootPoint.position;
        Quaternion rotation = Quaternion.LookRotation(direction.normalized);

        Debug.Log("Spawning projectile from " + shootPoint.position);

        Vector3 spawnPosition = shootPoint.position + direction.normalized * 0.5f;
        Instantiate(projectilePrefab, spawnPosition, rotation);

        shootTimer = shootCooldown;

        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }
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
            Time.deltaTime * 8f
        );
    }

    private void UpdateAnimation()
    {
        if (animator == null || agent == null) return;

        bool isMoving = agent.velocity.magnitude > 0.1f && !agent.isStopped;

        // Only use this if your Animator has a parameter called IsMoving.
        animator.SetBool("IsMoving", isMoving);
    }

    private bool HasLineOfSight()
    {
        if (shootPoint == null || player == null) return false;

        Vector3 start = shootPoint.position;
        Vector3 target = player.position + Vector3.up * aimHeight;
        Vector3 direction = target - start;

        if (Physics.Raycast(start, direction.normalized, out RaycastHit hit, direction.magnitude, obstacleLayer))
        {
            Debug.Log("Blocked by: " + hit.collider.name);
            return false;
        }

        return true;
    }
}