using UnityEngine;

public class LevelChange : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        LevelManager.Instance.LoadNextLevel();
    }
}