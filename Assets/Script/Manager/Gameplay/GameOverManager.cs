using UnityEngine;
using DG.Tweening;
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

    [SerializeField] private float animDuration = 1f;
    public void PlayerLose()
    {
        UIManager.instance.OpenPanel(losePanel);
        EntryAnimation(losePanel);
    }
    public void PlayerWin()
    {
        UIManager.instance.OpenPanel(winPanel);
        EntryAnimation(winPanel);
    }

    public void RestartGame()
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.buttonHit);
        GameStateManager.instance.SwitchState(GameState.GameplayScene);
    }

    public void BackToMainMenu()
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.buttonHit);
        GameStateManager.instance.SwitchState(GameState.MainMenu);
    }

    private void EntryAnimation(GameObject panel)
    {
        CanvasGroup canvas = panel.GetComponent<CanvasGroup>();
        Transform x = panel.transform;
        Sequence s = DOTween.Sequence().SetUpdate(true);

        s.Append(canvas.DOFade(1, animDuration))
            .Join(x.DORotate(new Vector3(0, 0, -360f), animDuration/2, RotateMode.FastBeyond360).SetEase(Ease.Linear)).OnComplete(() =>
            {
                canvas.interactable = true;
                canvas.blocksRaycasts = true;
            })
            ;
    }

}
