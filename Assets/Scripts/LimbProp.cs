using System;
using UnityEngine;

public class LimbProp : MonoBehaviour, IInteractable
{

    [SerializeField] private LimbStats limbStats;

    public float remainingHealth;

    private void Awake()
    {
        remainingHealth = limbStats.health;
    }

    public void Initialize(float hp)
    {
        remainingHealth = hp;
    }

    public void Interact(GameObject interactor)
    {
        LimbsManager limbsManager = interactor.GetComponent<LimbsManager>();

        if (limbsManager)
        {
            limbsManager.getLimb(limbStats, remainingHealth);
        }


        Destroy(gameObject);
    }

}
