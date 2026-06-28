using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyDamageTester : MonoBehaviour
{
    [SerializeField] private float damage = 999f;

    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.kKey.wasPressedThisFrame)
        {
            DamageEnemy();
        }
    }

    [ContextMenu("Damage Enemy")]
    public void DamageEnemy()
    {
        if (health == null)
        {
            Debug.LogError("No Health component found on enemy.");
            return;
        }

        health.takeDamage(damage);
        Debug.Log("Enemy took test damage");
    }
}