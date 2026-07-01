using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDeathDrop : MonoBehaviour
{
    [Header("Limb Drops")]
    [SerializeField] private GameObject[] limbDropPrefabs;
    [SerializeField] private Transform dropPoint;
    [SerializeField] private float dropSpreadRadius = 1.2f;

    [Header("Death FX")]
    [SerializeField] private GameObject deathEffectPrefab;
    [SerializeField] private Transform deathEffectPoint;

    [Header("Audio")]
    [SerializeField] private AudioSource deathAudio;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private float destroyDelay = 1.5f;

    private bool alreadyDead;

    public void Die()
    {
        if (alreadyDead) return;
        alreadyDead = true;

        StartCoroutine(DieRoutine());
    }

    private IEnumerator DieRoutine()
    {
        StopEnemyImmediately();

        Vector3 effectPosition = deathEffectPoint != null
            ? deathEffectPoint.position
            : transform.position + Vector3.up * 1.1f;

        Vector3 dropPosition = dropPoint != null
            ? dropPoint.position
            : transform.position;

        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, effectPosition, Quaternion.identity);
        }

        PlayAudio(deathAudio);

        if (animator != null)
        {
            animator.applyRootMotion = false;
            animator.SetTrigger("Die");
        }

        yield return new WaitForSeconds(destroyDelay);

        DropLimbs(dropPosition);

        Destroy(gameObject);
    }

    private void StopEnemyImmediately()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();

        if (agent != null && agent.enabled)
        {
            if (agent.isOnNavMesh)
            {
                agent.isStopped = true;
                agent.ResetPath();
            }

            agent.velocity = Vector3.zero;
            agent.enabled = false;
        }

        RangedRuleBasedEnemy ranged = GetComponent<RangedRuleBasedEnemy>();

        if (ranged != null)
        {
            ranged.enabled = false;
        }

        RandomMeleeNavMeshEnemy melee = GetComponent<RandomMeleeNavMeshEnemy>();

        if (melee != null)
        {
            melee.enabled = false;
        }

        BossRuleBasedEnemy boss = GetComponent<BossRuleBasedEnemy>();

        if (boss != null)
        {
            boss.enabled = false;
        }

        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }
    }

    private void DropLimbs(Vector3 centerPosition)
    {
        if (limbDropPrefabs == null) return;

        foreach (GameObject limbPrefab in limbDropPrefabs)
        {
            if (limbPrefab == null) continue;

            Vector2 randomOffset = Random.insideUnitCircle * dropSpreadRadius;

            Vector3 spawnPosition = centerPosition + new Vector3(
                randomOffset.x,
                0.3f,
                randomOffset.y
            );

            Quaternion randomRotation = Quaternion.Euler(
                Random.Range(0f, 30f),
                Random.Range(0f, 360f),
                Random.Range(0f, 30f)
            );

            Instantiate(limbPrefab, spawnPosition, randomRotation);
        }
    }

    private void PlayAudio(AudioSource audioSource)
    {
        if (audioSource == null) return;
        if (audioSource.clip == null) return;

        audioSource.PlayOneShot(audioSource.clip);
    }
}