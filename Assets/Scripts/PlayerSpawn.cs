using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawn : MonoBehaviour
{
    [Serializable]
    public class ScenePosition
    {
        public string sceneName;
        public Vector3 position;
        public Vector3 eulerRotation;
    }

    [Tooltip("Add one entry per scene, with the coordinates the player should spawn at in that scene.")]
    [SerializeField] private List<ScenePosition> scenePositions;

    [Tooltip("Optional: assign if your player uses a CharacterController (it blocks direct position changes otherwise).")]
    [SerializeField] private CharacterController controller;

    [Tooltip("Optional: assign if your player uses a Rigidbody, so velocity gets reset on teleport.")]
    [SerializeField] private Rigidbody rb;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ScenePosition match = scenePositions.Find(sp => sp.sceneName == scene.name);

        if (match == null)
        {
            Debug.LogWarning($"No spawn position set for scene '{scene.name}'. Player position unchanged.");
            return;
        }

        MoveTo(match.position, Quaternion.Euler(match.eulerRotation));
    }

    private void MoveTo(Vector3 pos, Quaternion rot)
    {
        if (controller != null)
        {
            controller.enabled = false;
            transform.SetPositionAndRotation(pos, rot);
            controller.enabled = true;
        }
        else
        {
            transform.SetPositionAndRotation(pos, rot);
        }

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}