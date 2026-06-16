using UnityEngine;
using UnityEngine.Events;

public class BodyHealth : Health
{
    [SerializeField] private float maxHealth;
    public UnityEvent OnDeath;

    public override float MaxHealth => maxHealth;

    private void OnEnable()
    {
        currentHealth = maxHealth;
    }

    protected override void healthDepleted()
    {
        OnDeath?.Invoke();
    }

    private void Update()
    {
        takeDamage(0.1f);
    }
}
