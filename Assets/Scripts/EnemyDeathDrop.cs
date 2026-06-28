using UnityEngine;

public class EnemyDeathDrop : MonoBehaviour
{
    [Header("Drops")]
    [SerializeField] private GameObject[] limbDropPrefabs;
    [SerializeField] private Transform dropPoint;

    [Header("Death FX")]
    [SerializeField] private GameObject deathEffectPrefab;

    private bool alreadyDead;

    public void Die()
    {
        if (alreadyDead) return;
        alreadyDead = true;

        Vector3 spawnPosition = dropPoint != null ? dropPoint.position : transform.position;

        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, spawnPosition, Quaternion.identity);
        }

        if (limbDropPrefabs != null && limbDropPrefabs.Length > 0)
        {
            int randomIndex = Random.Range(0, limbDropPrefabs.Length);
            Instantiate(limbDropPrefabs[randomIndex], spawnPosition, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}