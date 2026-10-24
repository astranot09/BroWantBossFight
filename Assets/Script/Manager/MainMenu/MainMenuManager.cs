using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneController.instance.LoadScene("Gameplay");
    }

    public void ExitGame()
    {
        SceneController.instance.ExitGame();
    }
}
