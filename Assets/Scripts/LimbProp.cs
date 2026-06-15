using System;
using UnityEngine;

public class LimbProp : MonoBehaviour, IInteractable
{

    [SerializeField] private LimbStats limbStats;

    public void Interact(GameObject interactor)
    {
        LimbsManager limbsManager = interactor.GetComponent<LimbsManager>();

        if (limbsManager)
        {
            limbsManager.getLimb(limbStats);
        }


        Destroy(gameObject);
    }

}
