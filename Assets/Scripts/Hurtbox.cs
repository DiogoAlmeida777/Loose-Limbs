using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Hurtbox : MonoBehaviour
{
    [SerializeField] private Health health;
    public UnityEvent OnGettingHurt;

    public void OnHit(float damage)
    {
        if (health)
        {
            health.takeDamage(damage);
            OnGettingHurt?.Invoke();
        }
    }

    public void setHealth(Health health)
    {
        this.health = health;
    }

}
