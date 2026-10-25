using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    [SerializeField] private AudioSource BGM;
    [SerializeField] private AudioSource SFX;

    [SerializeField] private AudioClip bgmMusic;
    public AudioClip swordSwing;
    public AudioClip swordHit;
    public AudioClip enemyDeath;

    private void Start()
    {
        PlayBGM(bgmMusic);
    }

    public void PlayBGM(AudioClip clip)
    {
        if (clip != null)
        {
            BGM.clip = clip;
            BGM.Play();
        }
    }
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            SFX.PlayOneShot(clip);
        }
    }
}
