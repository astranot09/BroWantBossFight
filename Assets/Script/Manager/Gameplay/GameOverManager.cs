using UnityEngine;

public class GameOverManager : MonoBehaviour
{

    public static GameOverManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject winPanel;
    public void PlayerLose()
    {
        UIManager.instance.OpenPanel(losePanel);
    }
    public void PlayerWin()
    {
        UIManager.instance.OpenPanel(winPanel);
    }

    public void RestartGame()
    {
        GameStateManager.instance.SwitchState(GameState.GameplayScene);
    }

    public void BackToMainMenu()
    {
        GameStateManager.instance.SwitchState(GameState.MainMenu);
    }
}
