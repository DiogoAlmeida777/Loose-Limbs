using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float lifeTime = 5f;

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
        Debug.Log($"{gameObject.name} touched {other.gameObject.name} on layer {LayerMask.LayerToName(other.gameObject.layer)}");

        if (IsInLayerMask(other.gameObject.layer, targetLayer))
        {
            Hurtbox hurtbox = other.GetComponentInParent<Hurtbox>();

            if (hurtbox != null)
            {
                hurtbox.OnHit(damage);
                Debug.Log($"{gameObject.name} damaged {hurtbox.gameObject.name} for {damage}");
            }

            Destroy(gameObject);
            return;
        }

        if (IsInLayerMask(other.gameObject.layer, obstacleLayer))
        {
            Debug.Log($"{gameObject.name} hit obstacle {other.gameObject.name}");
            Destroy(gameObject);
            return;
        }

        // Ignore everything else, including the enemy who shot it.
    }

    private bool IsInLayerMask(int layer, LayerMask mask)
    {
        return mask == (mask | (1 << layer));
    }
}