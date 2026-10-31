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
    public AudioClip playerHurt;
    public AudioClip playerParry;
    public AudioClip rocketHit;
    public AudioClip playerDash;
    public AudioClip buttonHit;
    public AudioClip enemyTeleport;

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
