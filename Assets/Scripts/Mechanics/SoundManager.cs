using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager instance;

    [SerializeField] private AudioClip winAudio;
    [SerializeField] private AudioClip failAudio;

    [SerializeField] private AudioSource audioSource;


    public AudioClip buttonAudio;




    private void Awake()
    {
        instance = this;
    }

    public void PlayAudioSound(AudioClip clip)
    {
        var temp = Instantiate(audioSource, transform);
        temp.clip = clip;
        temp.Play();
        Destroy(temp.gameObject, clip.length);
    }

    public void PlayAudioWin()
    {
        var temp = Instantiate(audioSource, transform);
        temp.clip = winAudio;
        temp.Play();
        Destroy(temp.gameObject, winAudio.length);
    }

    public void PlayAudioFail()
    {
        var temp = Instantiate(audioSource, transform);
        temp.clip = failAudio;
        temp.Play();
        Destroy(temp.gameObject, winAudio.length);
    }
}
