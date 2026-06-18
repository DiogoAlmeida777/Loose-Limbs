using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    [SerializeField] private Health health;

    public void OnHit(float damage)
    {
        if(health)
            health.takeDamage(damage);
    }

    public void setHealth(Health health)
    {
        this.health = health;
    }

}
