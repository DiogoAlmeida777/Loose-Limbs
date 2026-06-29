using UnityEngine;
using UnityEngine.Events;

public class LimbHealth : Health
{
    [SerializeField] private LimbStats limbStats;
    public UnityEvent<LimbStats> OnLimbLoss;

    public override float MaxHealth => limbStats.health;

    public void setCurrentHealth(float health)
    {
        currentHealth = health;
    }

    protected override void healthDepleted()
    {
        OnLimbLoss?.Invoke(limbStats);
        gameObject.SetActive(false);
    }

}
