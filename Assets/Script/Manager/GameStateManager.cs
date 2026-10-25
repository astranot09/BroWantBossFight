using UnityEngine;

[System.Serializable]
public enum GameState
{
    MainMenu,
    GameplayScene,
    GameStart,
    PlayerLose,
    PlayerWin
}


public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    [SerializeField] private GameState currentState;
    public void SwitchState(GameState state)
    {
        if (currentState == state) return;
        currentState = state;
        CheckState();
    }

    private void CheckState()
    {
        switch (currentState)
        {
            case GameState.MainMenu:
                SceneController.instance.LoadScene("MainMenu");
                break;
            case GameState.GameplayScene:
                SceneController.instance.LoadScene("Gameplay");
                break;
            case GameState.GameStart:
                break;
            case GameState.PlayerLose:
                break;
            case GameState.PlayerWin:
                break;
        }
    }
}
