using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Call this when the player dies - wire to BodyHealth.OnDeath in Inspector
    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    // Call this when level is complete (e.g. all enemies dead)
    public void LoadNextLevel()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        // If there's a next level, load it - otherwise go to GameOver (or a Win screen later)
        if (nextIndex <= SceneManager.sceneCountInBuildSettings - 2) // -2 to exclude GameOver scene
            SceneManager.LoadScene(nextIndex);
        else
            SceneManager.LoadScene("GameOver");
    }
}