using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClip[] _buildAudios;
    [SerializeField] private AudioClip[] _destroyAudios;
    [SerializeField] private AudioClip winAudio;
    [SerializeField] private AudioClip failAudio;
    [SerializeField] private AudioSource audioSource;
    
    public AudioClip buttonAudio;
    
    private void Awake()
    {
        Instance = this;
    }

    public void PlayAudioSound(AudioClip clip)
    {
        var temp = Instantiate(audioSource, transform);
        temp.clip = clip;
        temp.Play();
        Destroy(temp.gameObject, clip.length);
    }

    public void PlayAudioBuild()
    {
        var temp = Instantiate(audioSource, transform);
        var clip = _buildAudios[Random.Range(0, _buildAudios.Length)];
        temp.clip = clip;
        temp.Play();
        Destroy(temp.gameObject, clip.length);
    }

    public void PlayAudioDestroy()
    {
        var temp = Instantiate(audioSource, transform);
        var clip = _destroyAudios[Random.Range(0, _destroyAudios.Length)];
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
