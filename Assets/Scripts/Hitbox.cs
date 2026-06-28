using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private float damage = 20f;

    [SerializeField] private LayerMask layerMask;

    private void OnTriggerEnter(Collider other)
    {
        if (layerMask == (layerMask | (1 << other.transform.gameObject.layer)))
        {
            Hurtbox hurtbox = other.GetComponentInParent<Hurtbox>();

            if (hurtbox)
            {
                hurtbox.OnHit(damage);
                Destroy(gameObject);
            }
        }
    }
}