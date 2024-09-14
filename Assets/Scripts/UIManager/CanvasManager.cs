using Platformer.Observer;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class CanvasManager : MonoBehaviour
{

    [Header("Referencs")]
    [SerializeField] GameObject openSpin;

    [Header("UI elements")]
    [SerializeField]
    private GameObject _mainMenuUI;
    [SerializeField]
    private Image _soundOfficon;
    [SerializeField] private MusicManager _musicManager;
    
    [Header("Event CanvasManager")]

    [Space(20f)]
    [SerializeField] private Button _playGameButton;
    [SerializeField] private Button _soundButton;
    [SerializeField] private Button _openShopButton;
    [SerializeField] private Button _removeAdsButton;
    [SerializeField] private Button openSpinButton;


    private bool muted;



    private void OnDisable()
    {
        if (EventDispatcher.HasInstance())
        {
            EventDispatcher.Instance.RemoveListener(EventID.Home, (param) => Show());
        }
    }
    private void Start()
    {
        
        _soundOfficon.enabled = muted;
        Hide();
        _musicManager.gameObject.SetActive(false);
        

        AddEvents();
    }
    
    private void AddEvents()
    {
        // Handle Event Button Play Game
        _playGameButton.onClick.AddListener(() =>
        {
            SoundManager.instance.PlayAudioSound(SoundManager.instance.buttonAudio);
            OnPlayGame();
        });

        // Handle Event Button sound
        _soundButton.onClick.AddListener(() =>
        {
            SoundManager.instance.PlayAudioSound(SoundManager.instance.buttonAudio);
            HandleStateMusic();
        });

        //
        _openShopButton.onClick.AddListener(() =>
        {
            SoundManager.instance.PlayAudioSound(SoundManager.instance.buttonAudio);
            Hide();
            this.PostEvent(EventID.OpenShop);
        });

        //
        _removeAdsButton.onClick.AddListener(() =>
        {
            SoundManager.instance.PlayAudioSound(SoundManager.instance.buttonAudio);
            Debug.Log("remove Ads successfully");
        });

        //
        openSpinButton.onClick.AddListener(() =>
        {
            SoundManager.instance.PlayAudioSound(SoundManager.instance.buttonAudio);
            Hide();
            openSpin.SetActive(true);
        });
    }

    private void OnPlayGame()
    {
        this.PostEvent(EventID.GameStartUI);
        this.PostEvent(EventID.IsPlayGame, true);
        Hide() ;
    }
    private void Show()
    {
        _mainMenuUI.SetActive(true);
        _musicManager.gameObject.SetActive(true);
    }
       
    
    private void Hide()
        => _mainMenuUI.SetActive(false);
    

    private void HandleStateMusic()
    {
        if (muted)
        {
            muted = false;
            Save();
        }
        else
        {
            muted = true;
            Save();
        }
        UpdateStateMusic();
    }
  
 
    private void UpdateStateMusic()
    {
        if(muted)
        {
            _soundOfficon.enabled = false;
            MusicManager.instance.OnChangeMusic(false);
        }
        else
        {
            _soundOfficon.enabled = true;
            MusicManager.instance.OnChangeMusic(true);
        }
    }

    private void Save()
    {
        YandexGame.savesData.muted = muted;
        YandexGame.SaveProgress();
    }
    private void OnEnable()
    {
        muted = YandexGame.savesData.muted;
        this.RegisterListener(EventID.Home, (param) => Show());

    }

}
