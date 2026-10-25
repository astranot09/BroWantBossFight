using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        GameStateManager.instance.SwitchState(GameState.GameplayScene);
    }

    public void ExitGame()
    {
        SceneController.instance.ExitGame();
    }
}
