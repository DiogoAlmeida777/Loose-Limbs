using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private AudioSource hitSound; 

    public void setDamage(float damage)
    {
        this.damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (layerMask == (layerMask | (1 << other.transform.gameObject.layer)))
        {
            Hurtbox hurtbox = other.GetComponentInParent<Hurtbox>();

            if (hurtbox)
            {
                hurtbox.OnHit(damage);
                hitSound.Play();
            }
        }
    }
}