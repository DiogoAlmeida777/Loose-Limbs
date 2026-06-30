using UnityEngine;
using Unity.Cinemachine;

public class DeathCamera : MonoBehaviour
{
    [SerializeField] private float orbitSpeed = 20f;
    private CinemachineOrbitalFollow orbitalFollow;

    private void Awake()
    {
        orbitalFollow = GetComponent<CinemachineOrbitalFollow>();
    }

    private void Update()
    {
        if (orbitalFollow == null) return;

        // Use unscaled delta time so orbit keeps moving even when timeScale = 0
        orbitalFollow.HorizontalAxis.Value += orbitSpeed * Time.unscaledDeltaTime;
    }
}