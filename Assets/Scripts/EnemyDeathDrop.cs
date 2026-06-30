using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDeathDrop : MonoBehaviour
{
    [Header("Drops")]
    [SerializeField] private GameObject[] limbDropPrefabs;
    [SerializeField] private Transform dropPoint;

    [Header("Death FX")]
    [SerializeField] private GameObject deathEffectPrefab;

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
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }

        RandomMeleeNavMeshEnemy enemyAI = GetComponent<RandomMeleeNavMeshEnemy>();
        if (enemyAI != null)
        {
            enemyAI.enabled = false;
        }

        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }

        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        Vector3 spawnPosition = dropPoint != null ? dropPoint.position : transform.position;

        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, spawnPosition, Quaternion.identity);
        }

        yield return new WaitForSeconds(destroyDelay);

        if (limbDropPrefabs != null && limbDropPrefabs.Length > 0)
        {
            int randomIndex = Random.Range(0, limbDropPrefabs.Length);
            GameObject selectedDrop = limbDropPrefabs[randomIndex];

            if (selectedDrop != null)
            {
                Instantiate(selectedDrop, spawnPosition, Quaternion.identity);
            }
        }

        Destroy(gameObject);
    }
}