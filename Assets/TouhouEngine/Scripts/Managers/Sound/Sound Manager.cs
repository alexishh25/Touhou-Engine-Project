using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    private float loop_start_time, loop_end_time;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        musicSource = GetComponent<AudioSource>();
    }



    public void PlayMusic(AudioClip clip, float start, float end)
    {
        loop_start_time = start;
        loop_end_time = end;

        musicSource.clip = clip;
        musicSource.loop = false;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;

        sfxSource.PlayOneShot(clip);
    }

    private void Update()
    {
        CheckLoop();
    }

    private void CheckLoop()
    {
        if (musicSource.isPlaying && musicSource.time >= loop_end_time)
            musicSource.time = loop_start_time;
    }
}
