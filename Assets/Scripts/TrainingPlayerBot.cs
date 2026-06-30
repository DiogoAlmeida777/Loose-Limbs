using UnityEngine;
using UnityEngine.AI;

public class TrainingPlayerBot : MonoBehaviour
{
    private enum TrainingLevel
    {
        Level1_StandStill,
        Level2_RandomMovement,
        Level3_FleeAndShoot
    }

    [Header("Training Mode")]
    [SerializeField] private TrainingLevel trainingLevel = TrainingLevel.Level1_StandStill;

    [Header("References")]
    [SerializeField] private Transform boss;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Animator animator;

    [Header("Movement")]
    [SerializeField] private float detectionRange = 20f;
    [SerializeField] private float randomMoveRadius = 6f;
    [SerializeField] private float randomMoveInterval = 2f;
    [SerializeField] private float fleeDistance = 5f;
    [SerializeField] private float tooCloseDistance = 6f;

    [Header("Shooting")]
    [SerializeField] private float shootCooldown = 1.2f;
    [SerializeField] private float aimHeight = 1.2f;
    [SerializeField] private LayerMask obstacleLayer;

    private NavMeshAgent agent;
    private float randomMoveTimer;
    private float shootTimer;
    private Vector3 startPosition;
    private Quaternion startRotation;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        startPosition = transform.position;
        startRotation = transform.rotation;

        if (boss == null)
        {
            GameObject bossObject = GameObject.FindGameObjectWithTag("Enemy");

            if (bossObject != null)
            {
                boss = bossObject.transform;
            }
        }
    }

    private void Update()
    {
        shootTimer = Mathf.Max(0f, shootTimer - Time.deltaTime);
        randomMoveTimer = Mathf.Max(0f, randomMoveTimer - Time.deltaTime);

        if (agent == null) return;

        switch (trainingLevel)
        {
            case TrainingLevel.Level1_StandStill:
                StandStill();
                UpdateAnimation();
                return;

            case TrainingLevel.Level2_RandomMovement:
                if (boss == null) return;
                RandomMovement();
                break;

            case TrainingLevel.Level3_FleeAndShoot:
                if (boss == null) return;
                FleeAndShoot();
                break;
        }

        UpdateAnimation();
    }

    private void StandStill()
    {
        if (agent != null && agent.enabled && agent.isOnNavMesh)
        {
            agent.isStopped = true;
            agent.ResetPath();
            agent.velocity = Vector3.zero;

            // Force position every frame so the boss/physics cannot push it.
            agent.Warp(startPosition);
        }
        else
        {
            transform.position = startPosition;
        }

        // Keep fixed rotation too. Remove this line if you want him to rotate visually.
        transform.rotation = startRotation;
    }

    private void RandomMovement()
    {
        agent.isStopped = false;

        if (randomMoveTimer > 0f) return;

        randomMoveTimer = randomMoveInterval;

        Vector3 randomDirection = Random.insideUnitSphere * randomMoveRadius;
        randomDirection += transform.position;
        randomDirection.y = transform.position.y;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, randomMoveRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    private void FleeAndShoot()
    {
        float distanceToBoss = Vector3.Distance(transform.position, boss.position);

        LookAtBoss();

        if (distanceToBoss <= tooCloseDistance)
        {
            if (randomMoveTimer <= 0f)
            {
                FleeFromBoss();
                randomMoveTimer = randomMoveInterval;
            }
        }
        else
        {
            RandomMovement();
        }

        if (distanceToBoss <= detectionRange)
        {
            TryShoot();
        }
    }

    private void FleeFromBoss()
    {
        agent.isStopped = false;

        Vector3 directionAway = transform.position - boss.position;
        directionAway.y = 0f;

        if (directionAway.sqrMagnitude <= 0.01f)
        {
            directionAway = -transform.forward;
        }

        directionAway.Normalize();

        Vector3 target = transform.position + directionAway * fleeDistance;

        if (NavMesh.SamplePosition(target, out NavMeshHit hit, fleeDistance, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    private void TryShoot()
    {
        if (shootTimer > 0f) return;
        if (projectilePrefab == null) return;
        if (shootPoint == null) return;
        if (!HasLineOfSight()) return;

        Vector3 targetPosition = boss.position + Vector3.up * aimHeight;
        Vector3 direction = targetPosition - shootPoint.position;

        if (direction.sqrMagnitude <= 0.01f) return;

        Quaternion rotation = Quaternion.LookRotation(direction.normalized);
        Vector3 spawnPosition = shootPoint.position + direction.normalized * 1f;

        Instantiate(projectilePrefab, spawnPosition, rotation);

        shootTimer = shootCooldown;
    }

    private bool HasLineOfSight()
    {
        if (boss == null || shootPoint == null) return false;

        Vector3 start = shootPoint.position;
        Vector3 target = boss.position + Vector3.up * aimHeight;
        Vector3 direction = target - start;

        if (Physics.Raycast(start, direction.normalized, out RaycastHit hit, direction.magnitude, obstacleLayer))
        {
            return false;
        }

        return true;
    }

    private void LookAtBoss()
    {
        if (boss == null) return;

        Vector3 direction = boss.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.01f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * 10f
        );
    }

    private void UpdateAnimation()
    {
        if (animator == null || agent == null) return;

        bool isMoving = agent.velocity.magnitude > 0.1f && !agent.isStopped;
        animator.SetBool("IsMoving", isMoving);
    }
    public void ResetTrainingPlayer()
    {
        if (agent != null && agent.enabled && agent.isOnNavMesh)
        {
            agent.Warp(startPosition);
            agent.ResetPath();
            agent.velocity = Vector3.zero;

            if (trainingLevel == TrainingLevel.Level1_StandStill)
            {
                agent.isStopped = true;
            }
            else
            {
                agent.isStopped = false;
            }
        }
        else
        {
            transform.position = startPosition;
        }

        transform.rotation = startRotation;
    }
}