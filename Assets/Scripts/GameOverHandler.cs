using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;

public class GameOverHandler : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject hudObject;
    [SerializeField] private GameObject crosshairObject;

    [Header("Cameras")]
    [SerializeField] private CinemachineCamera deathVcam;
    [SerializeField] private CinemachineCamera thirdPersonCamera;
    public void OnPlayerDeath()
{
    // Camera swap first, before freezing time
    thirdPersonCamera.gameObject.SetActive(false);
    deathVcam.gameObject.SetActive(true);
    deathVcam.enabled = false;
    deathVcam.enabled = true;

    // Freeze after camera switch
    Time.timeScale = 0f;

    if (hudObject != null)
        hudObject.SetActive(false);
    if (gameOverPanel != null)
        gameOverPanel.SetActive(true);
    if (crosshairObject != null)
        crosshairObject.SetActive(false);

    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;
}

    public void Retry()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level1");
    }

    public void ReturnToMainMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}