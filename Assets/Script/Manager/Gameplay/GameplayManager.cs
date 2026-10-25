using System;
using UnityEngine;
using UnityEngine.Playables;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }


    [SerializeField] private PlayableDirector playableDirector;


    public event Action OnGameStart;
    public event Action OnBattleStart;
    public event Action OnGameFinish;

    private void Start()
    {
        playableDirector.Play();
    }

    public void GameStart()
    {
        OnGameStart?.Invoke();
    }
}
