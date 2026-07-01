using UnityEngine;

public class MedpackPickup : MonoBehaviour
{
    [SerializeField] private float healAmount = 50f;
    [SerializeField] private string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        BodyHealth bodyHealth = other.GetComponent<BodyHealth>();
        if (bodyHealth == null) return;

        if (bodyHealth.currentHealth >= bodyHealth.MaxHealth) return;

        bodyHealth.getHeal(healAmount);
        Destroy(gameObject);
    }
}