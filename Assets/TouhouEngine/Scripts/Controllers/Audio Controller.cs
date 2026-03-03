using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] public AudioClip musicClip;
    public float loopStartTime = 0f;
    public float loopEndTime = 30f;

    private void Start()
    {
        SoundManager.Instance.PlayMusic(musicClip, loopStartTime, loopEndTime);
    }
        
}
