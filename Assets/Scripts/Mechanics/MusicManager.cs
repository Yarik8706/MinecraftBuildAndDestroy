using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using YG;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;
    #region Singleton Class: MusicManager
    public static MusicManager instance;

    private void Awake()
    {
        if(instance == null)
            instance = this;
    }
    #endregion

    [SerializeField]
    private AudioSource _audioSource;

 
    public void InitData()
    {
        _audioSource.mute = YandexGame.savesData.muted;
    }

    public void OnChangeMusic(bool muted)
    {
        _audioMixer.SetFloat("Master", muted ? -80 : 0); 
        _audioSource.mute = muted; 
    }
   
}
