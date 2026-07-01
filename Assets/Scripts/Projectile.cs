using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float lifeTime = 5f;

    [Header("Hit FX")]
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private float hitEffectForwardOffset = 0.05f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsInLayerMask(other.gameObject.layer, targetLayer))
        {
            Vector3 hitPosition = other.ClosestPoint(transform.position);

            SpawnHitEffect(hitPosition);

            Hurtbox hurtbox = other.GetComponentInParent<Hurtbox>();

            if (hurtbox != null)
            {
                hurtbox.OnHit(damage);
            }

            Destroy(gameObject);
            return;
        }

        if (IsInLayerMask(other.gameObject.layer, obstacleLayer))
        {
            Destroy(gameObject);
            return;
        }
    }

    private void SpawnHitEffect(Vector3 hitPosition)
    {
        if (hitEffectPrefab == null) return;

        Quaternion rotation = Quaternion.LookRotation(-transform.forward);

        Vector3 spawnPosition = hitPosition + (-transform.forward * hitEffectForwardOffset);

        Instantiate(hitEffectPrefab, spawnPosition, rotation);
    }

    private bool IsInLayerMask(int layer, LayerMask mask)
    {
        return mask == (mask | (1 << layer));
    }
}