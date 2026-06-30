using UnityEngine;
using UnityEngine.AI;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.InputSystem;

public class BossAgent : Agent
{
    [Header("References")]
    [SerializeField] private BossRuleBasedEnemy bossBrain;
    [SerializeField] private Transform player;
    [SerializeField] private Health bossHealth;
    [SerializeField] private Health playerHealth;

    [Header("Training")]
    [SerializeField] private float maxDistance = 25f;
    [SerializeField] private TrainingPlayerBot trainingPlayerBot;

    [Header("Episode")]
    [SerializeField] private float episodeDuration = 30f;

    private float episodeTimer;

    private Vector3 startPosition;
    private Quaternion startRotation;

    private Vector3 playerStartPosition;
    private Quaternion playerStartRotation;

    private float previousBossHealth;
    private float previousPlayerHealth;



    private NavMeshAgent navMeshAgent;

    private void Awake()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;

        navMeshAgent = GetComponent<NavMeshAgent>();

        if (bossBrain == null)
        {
            bossBrain = GetComponent<BossRuleBasedEnemy>();
        }

        if (bossHealth == null)
        {
            bossHealth = GetComponent<Health>();
        }

        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }

        if (player != null)
        {
            playerStartPosition = player.position;
            playerStartRotation = player.rotation;

            if (playerHealth == null)
            {
                playerHealth = player.GetComponent<Health>();
            }

            if (trainingPlayerBot == null)
            {
                trainingPlayerBot = player.GetComponent<TrainingPlayerBot>();
            }
        }
    }

    public override void OnEpisodeBegin()
    {
        episodeTimer = 0f;

        ResetBoss();

        if (trainingPlayerBot != null)
        {
            trainingPlayerBot.ResetTrainingPlayer();
        }
        else
        {
            ResetPlayer();
        }

        if (bossHealth != null)
        {
            bossHealth.resetHealth();
            previousBossHealth = bossHealth.currentHealth;
        }

        if (playerHealth != null)
        {
            playerHealth.resetHealth();
            previousPlayerHealth = playerHealth.currentHealth;
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        if (player == null)
        {
            sensor.AddObservation(Vector3.zero);
            sensor.AddObservation(1f);
            sensor.AddObservation(0f);
            sensor.AddObservation(1f);
            sensor.AddObservation(1f);
            return;
        }

        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        sensor.AddObservation(directionToPlayer.normalized);
        sensor.AddObservation(Mathf.Clamp01(distanceToPlayer / maxDistance));

        bool canSeePlayer = bossBrain != null && bossBrain.CanSeePlayer();
        sensor.AddObservation(canSeePlayer ? 1f : 0f);

        float bossHealthNormalized = 1f;

        if (bossHealth != null && bossHealth.MaxHealth > 0f)
        {
            bossHealthNormalized = bossHealth.currentHealth / bossHealth.MaxHealth;
        }

        float playerHealthNormalized = 1f;

        if (playerHealth != null && playerHealth.MaxHealth > 0f)
        {
            playerHealthNormalized = playerHealth.currentHealth / playerHealth.MaxHealth;
        }

        sensor.AddObservation(bossHealthNormalized);
        sensor.AddObservation(playerHealthNormalized);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        if (bossBrain == null)
        {
            AddReward(-1f);
            EndEpisode();
            return;
        }

        int action = actions.DiscreteActions[0];

        AddReward(-0.001f);

        switch (action)
        {
            case 0:
                // Idle. Do nothing.
                break;

            case 1:
                bossBrain.ChasePlayer();
                break;

            case 2:
                bossBrain.RetreatFromPlayer();
                break;

            case 3:
                bossBrain.TryMeleeAttack();
                break;

            case 4:
                bossBrain.TryRangedAttack();
                break;
        }

        RewardHealthChanges();
        CheckEpisodeEnd();
    }

    private void RewardHealthChanges()
    {
        if (bossHealth != null)
        {
            float currentBossHp = bossHealth.currentHealth;

            if (currentBossHp < previousBossHealth)
            {
                AddReward(-0.5f);
            }

            previousBossHealth = currentBossHp;
        }

        if (playerHealth != null)
        {
            float currentPlayerHp = playerHealth.currentHealth;

            if (currentPlayerHp < previousPlayerHealth)
            {
                AddReward(+1.0f);
            }

            previousPlayerHealth = currentPlayerHp;
        }
    }

    private void CheckEpisodeEnd()
    {
        if (playerHealth != null && playerHealth.currentHealth <= 0f)
        {
            AddReward(+5f);
            EndEpisode();
            return;
        }

        if (bossHealth != null && bossHealth.currentHealth <= 0f)
        {
            AddReward(-5f);
            EndEpisode();
            return;
        }

        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer > maxDistance)
            {
                AddReward(-0.01f);
            }
        }

        episodeTimer += Time.deltaTime;

        if (episodeTimer >= episodeDuration)
        {
            AddReward(-1f);
            EndEpisode();
        }
    }

    private void ResetBoss()
    {
        if (navMeshAgent != null && navMeshAgent.enabled)
        {
            if (NavMesh.SamplePosition(startPosition, out NavMeshHit hit, 5f, NavMesh.AllAreas))
            {
                navMeshAgent.Warp(hit.position);
            }
            else
            {
                Debug.LogWarning("BOSS RESET FAIL: could not find NavMesh near start position.");
            }

            navMeshAgent.isStopped = false;
        }
        else
        {
            transform.position = startPosition;
        }

        transform.rotation = startRotation;
    }

    private void ResetPlayer()
    {
        if (player == null) return;

        NavMeshAgent playerAgent = player.GetComponent<NavMeshAgent>();

        if (playerAgent != null && playerAgent.enabled)
        {
            if (NavMesh.SamplePosition(playerStartPosition, out NavMeshHit hit, 5f, NavMesh.AllAreas))
            {
                playerAgent.Warp(hit.position);
            }
            else
            {
                Debug.LogWarning("PLAYER RESET FAIL: could not find NavMesh near player start position.");
            }

            playerAgent.isStopped = false;
        }
        else
        {
            player.position = playerStartPosition;
        }

        player.rotation = playerStartRotation;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;

        discreteActions[0] = 0;

        if (Keyboard.current == null)
        {
            return;
        }

        if (Keyboard.current.digit1Key.isPressed || Keyboard.current.numpad1Key.isPressed)
        {
            discreteActions[0] = 1;
        }
        else if (Keyboard.current.digit2Key.isPressed || Keyboard.current.numpad2Key.isPressed)
        {
            discreteActions[0] = 2;
        }
        else if (Keyboard.current.digit3Key.isPressed || Keyboard.current.numpad3Key.isPressed)
        {
            discreteActions[0] = 3;
        }
        else if (Keyboard.current.digit4Key.isPressed || Keyboard.current.numpad4Key.isPressed)
        {
            discreteActions[0] = 4;
        }
    }
}