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
        Debug.Log($"{gameObject.name} health reset to {currentHealth}");
    }

    protected override void healthDepleted()
    {
        Debug.Log($"{gameObject.name} health depleted. Calling OnDeath.");
        OnDeath?.Invoke();
    }
}