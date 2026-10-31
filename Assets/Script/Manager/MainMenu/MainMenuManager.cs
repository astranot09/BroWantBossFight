using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.buttonHit);
        GameStateManager.instance.SwitchState(GameState.GameplayScene);
    }

    public void ExitGame()
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.buttonHit);
        SceneController.instance.ExitGame();
    }
}
