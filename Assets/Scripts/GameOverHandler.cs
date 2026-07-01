using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;
using UnityEngine.Events;
public class GameOverHandler : MonoBehaviour { [Header("UI")][SerializeField] private GameObject gameOverPanel; [SerializeField] private GameObject hudObject; public void OnPlayerDeath() { Debug.Log("GAME OVER CALLED"); if (hudObject != null) hudObject.SetActive(false); if (gameOverPanel != null) gameOverPanel.SetActive(true); Cursor.lockState = CursorLockMode.None; Cursor.visible = true; } public void Retry() { Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false; Time.timeScale = 1f; SceneManager.LoadScene("Level1"); } public void ReturnToMainMenu() { Cursor.lockState = CursorLockMode.None; Cursor.visible = true; Time.timeScale = 1f; SceneManager.LoadScene("MainMenu"); } }