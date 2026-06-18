using UnityEngine;
using UnityEngine.Events;

public abstract class Health : MonoBehaviour
{

    public float currentHealth {  get; protected set; }
    public abstract float MaxHealth { get; }

    public virtual void getHeal(float hp)
    {
        currentHealth += hp;
    }

    public virtual void takeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
            healthDepleted();
    }

    protected abstract void healthDepleted();
    
}
