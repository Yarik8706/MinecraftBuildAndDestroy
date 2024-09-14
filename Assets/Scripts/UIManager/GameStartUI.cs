using System;
using Flatformer.GameData;
using Platformer.Mechanics;
using Platformer.Observer;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class MultiText
{
    public string ruText;
    public string enText;

    public MultiText(string ruText, string enText)
    {
        this.ruText = ruText;
        this.enText = enText;
    }

    public string GetText()
    {
        return MultiTextUI.lang == "ru" ? ruText : enText;
    }
}

public class GameStartUI : MonoBehaviour
{
    [Header("Event UI")]
    [SerializeField] private Button _backHomeButton;
    [SerializeField] private Button _replayGameButton;
    [SerializeField] private Button _rewardSkipLevelButton;
    [SerializeField] private TextMeshProUGUI _currenLevelText;

    public static GameStartUI Instance { get; private set; }

    private MultiText levelText = new MultiText("Уровень ", "Level ");
    private float _timeShowInter;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        YandexGame.RewardVideoEvent += OnCompleteAds;
        this.RegisterListener(EventID.GameStartUI, (param) => Show());
        this.RegisterListener(EventID.OnCarMove, (param) => Hide());
        this.RegisterListener(EventID.Victory,(param) => Hide());   
        this.RegisterListener(EventID.Loss, (param) => Hide());
    }

    private void OnDisable()
    {
        YandexGame.RewardVideoEvent -= OnCompleteAds;
        if (EventDispatcher.HasInstance())
        {
            EventDispatcher.Instance.RemoveListener(EventID.GameStartUI, (param) => Show());
            EventDispatcher.Instance.RemoveListener(EventID.OnCarMove, (param) => Hide());
            EventDispatcher.Instance.RegisterListener(EventID.Victory, (param) => Hide());
            EventDispatcher.Instance.RemoveListener(EventID.Loss, (param) => Hide());
        }
    }

    private void Update()
    {
        if (_rewardSkipLevelButton.gameObject.activeSelf == GameState.IsGameStart)
        {
            _rewardSkipLevelButton.gameObject.SetActive(!GameState.IsGameStart);
        }
    }

    public void Init()
    {
        if (_timeShowInter < 30)
        {
            _timeShowInter = 30;
        }
        Hide();
        SetCurrentText();
        AddEvents();
    }

    private void AddEvents()
    {
        _backHomeButton.onClick.RemoveAllListeners();
        _backHomeButton.onClick.AddListener(() =>
        {
            SoundManager.instance.PlayAudioSound(SoundManager.instance.buttonAudio);
            if(_timeShowInter <= 0)
            {
                YandexGame.FullscreenShow();
            }
            OnBackHomeButton();
        });
        
        _replayGameButton.onClick.RemoveAllListeners();
        _replayGameButton.onClick.AddListener(() =>
        {
            SoundManager.instance.PlayAudioSound(SoundManager.instance.buttonAudio);
            OnReplayGameButton();
        });
        
        _rewardSkipLevelButton.onClick.RemoveAllListeners();
        _rewardSkipLevelButton.onClick.AddListener(() =>
        {
            SoundManager.instance.PlayAudioSound(SoundManager.instance.buttonAudio);
            this.PostEvent(EventID.BtnSkipLevel, true);
            YandexGame.RewVideoShow((int) VideoAdsId.SkipLevel);
        });
    }

    private IEnumerator CountDownTimeShow()
    {
        while (_timeShowInter > 0)
        {
            yield return new WaitForSeconds(1f);
            _timeShowInter--;
            
        }
    } 
    private void OnReplayGameButton()
    {
        GameManager.Instance.ReplayGame();
        this.PostEvent(EventID.IsPlayGame,true);
    }

    private void OnBackHomeButton()
    {
        GameManager.Instance.ReplayGame();
        this.PostEvent(EventID.Home);
        Hide() ;
    }
    private void SetCurrentText()
    {
        _currenLevelText.text = levelText.GetText() + (GameDataManager.GetLevel() +1);
    }
    private void Show()
    {
        gameObject.SetActive(true);
        SetCurrentText();
        if(_timeShowInter < 30)
        {
            _timeShowInter = 30;
        }
        StartCoroutine(CountDownTimeShow());
    }
    private void Hide() => gameObject.SetActive(false);

    
    private void OnCompleteAds(int id)
    {
        if(id != (int)VideoAdsId.SkipLevel) return;
        StartCoroutine(DelayCompleteAds());
    }

    private IEnumerator DelayCompleteAds()
    {
        var t = 5;
        while( t > 0)
        {
            yield return new WaitForEndOfFrame();
            t--;
            if(t == 0)
            {
                SoundManager.instance.PlayAudioWin();
                GameManager.Instance.Coin += 50;
                GameDataManager.AddLevel(1);
                this.PostEvent(EventID.Victory);
                SetCurrentText();
            }
        }
    }
    private void FailedAds()
    {
        Debug.Log("Ads");
    }
}
