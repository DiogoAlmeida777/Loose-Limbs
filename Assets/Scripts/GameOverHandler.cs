using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class GameOverHandler : MonoBehaviour { 
    [Header("UI")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject hudObject;

    private void Awake()
    {
        gameOverPanel.SetActive(false);
    }

    public void OnPlayerDeath() { 
        Debug.Log("GAME OVER CALLED"); 
        if (hudObject != null) hudObject.SetActive(false); 
        if (gameOverPanel != null) gameOverPanel.SetActive(true); 
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true; 
    } 

    private void clearPersistentData()
    {
        foreach (var obj in DontDestroy.persistentObjects)
        {
            if (obj != null)
                Destroy(obj);
        }

        Array.Clear(DontDestroy.persistentObjects, 0, DontDestroy.persistentObjects.Length);
    }

    public void Retry() { 
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false; 
        Time.timeScale = 1f;
        clearPersistentData();
        gameOverPanel.SetActive(false);
        SceneManager.LoadScene("Level1"); 
    } 
    public void ReturnToMainMenu() {
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true; 
        Time.timeScale = 1f;
        clearPersistentData();
        SceneManager.LoadScene("MainMenu"); 
    } 
}