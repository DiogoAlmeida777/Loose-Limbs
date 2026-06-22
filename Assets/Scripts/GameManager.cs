using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameState currentState = GameState.StartScreen;
    public GameState CurrentState => currentState;

    public UnityEvent<GameState> OnStateChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        SetState(currentState);
    }

    public void SetState(GameState newState)
    {
        currentState = newState;
        OnStateChanged?.Invoke(currentState);
    }

    public void StartGame() => SetState(GameState.Playing);
    public void GameOver() => SetState(GameState.GameOver);
    public void ReturnToStartScreen() => SetState(GameState.StartScreen);
}