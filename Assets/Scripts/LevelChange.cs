using UnityEngine;

public class LevelChange : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player"; // confirm your player's tag matches this

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered by: " + other.name);
        Debug.Log("LevelManager.Instance is null? " + (LevelManager.Instance == null));
        if (!other.CompareTag(playerTag)) return;
        LevelManager.Instance.LoadNextLevel();
    }
}