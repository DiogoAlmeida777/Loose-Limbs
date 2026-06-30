using UnityEngine;
using UnityEngine.Events;

public abstract class Health : MonoBehaviour
{
    public UnityEvent<float, float> OnHealthChanged = new UnityEvent<float, float>();

    public float currentHealth { get; protected set; }
    public abstract float MaxHealth { get; }

    public virtual void resetHealth()
    {
        currentHealth = MaxHealth;
    }

    public virtual void getHeal(float hp)
    {
        currentHealth = Mathf.Min(currentHealth + hp, MaxHealth);
        OnHealthChanged?.Invoke(currentHealth, MaxHealth);
    }

    public virtual void takeDamage(float damage)
    {
        currentHealth -= damage;
        OnHealthChanged?.Invoke(currentHealth, MaxHealth);
        if (currentHealth <= 0)
        {
            healthDepleted();
        }
    }

    protected abstract void healthDepleted();
}