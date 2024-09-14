
using Platformer.Observer;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Platformer.Mechanics;
using Flatformer.GameData;
using YG;

public class GameVictoryUI : MonoBehaviour
{
    [SerializeField] private GameObject gameFail;
    [Header("User Interface")]
    [SerializeField] private GameObject _victoryUI;
    [SerializeField] private TextMeshProUGUI _coinText;
    [SerializeField] private TextMeshProUGUI _coinRewardText;
    [SerializeField] private TextMeshProUGUI _currentCoinText;
    [Header("Events")]
    [SerializeField] private Button _tapContinueButton;
    [SerializeField] private Button _rewardButton;
    private bool isInteractBtnSkipLevel;

    private void SetIsInteractBtnSkipLevel(bool isInterac)
    {
        this.isInteractBtnSkipLevel = isInterac;
    }

    private void OnEnable()
    {
        YandexGame.RewardVideoEvent += OnCompleteAds;
        this.RegisterListener(EventID.Victory, (param) => OpenVictoryPanel());
        this.RegisterListener(EventID.BtnSkipLevel, (param) => SetIsInteractBtnSkipLevel((bool)param));
    }

    private void OnDisable()
    {
        YandexGame.RewardVideoEvent -= OnCompleteAds;
        if (EventDispatcher.HasInstance())
        {
            EventDispatcher.Instance.RemoveListener(EventID.Victory, (param) => OpenVictoryPanel());
            EventDispatcher.Instance.RemoveListener(EventID.BtnSkipLevel, (param) => SetIsInteractBtnSkipLevel((bool)param));
        }
    }

    private void Start()
    {
        CloseVictoryPanel();
        AddEvents();
    }

    private void SetAllCoinText()
    {
        Debug.Log(GameManager.Instance.Coin);
        _coinText.text = GameManager.Instance.Coin.ToString();
        _coinRewardText.text = (GameManager.Instance.Coin * 3).ToString();
        _currentCoinText.text = GameDataManager.GetCoin().ToString();
    }

    private void OpenVictoryPanel()
    {
        _victoryUI.gameObject.SetActive(true);
        SoundManager.instance.PlayAudioWin();
        SetAllCoinText();
        _tapContinueButton.gameObject.SetActive(false);
        StartCoroutine(DelayShowTapcontinueButton(3));
    }

    private void CloseVictoryPanel()
    {
        _victoryUI.SetActive(false);
    }

    private void AddEvents()
    {
        // Envet Tap to continue
        _tapContinueButton.onClick.RemoveAllListeners();
        _tapContinueButton.onClick.AddListener(() =>
        {
            SoundManager.instance.PlayAudioSound(SoundManager.instance.buttonAudio);
            HandleEventTapContinue();
            CloseVictoryPanel();
            GameSharedUI.instance.UpdateCoinsTextUI();
            this.PostEvent(EventID.IsPlayGame, true);
        });


        // Event Ads Reward
        _rewardButton.onClick.RemoveAllListeners();
        _rewardButton.onClick.AddListener(() =>
        {
            SoundManager.instance.PlayAudioSound(SoundManager.instance.buttonAudio);
            YandexGame.RewVideoShow((int) VideoAdsId.Reward1);
        });
    }
    private void HandleEventTapContinue()
    {
       
        // xu ly next level cho use
        TapContinueButton();

        // Save Data
        ChangeDataGame();

        // hien thi lai reward button
        ShowButtonReward();
        this.PostEvent(EventID.GameStartUI);

    }
    private void TapContinueButton()
    {
        if (GameManager.Instance.currentLevel >= 3)
        {
            if (!isInteractBtnSkipLevel)
            {
                YandexGame.FullscreenShow();
            }
        }
        SoundManager.instance.PlayAudioSound(SoundManager.instance.buttonAudio);
        GameManager.Instance.NextLevel();
    }

    private IEnumerator DelayShowTapcontinueButton(float time)
    {
        var t = time;
        while (t > 0)
        {
            yield return new WaitForSeconds(1f);
            t--;
        }
        _tapContinueButton.gameObject.SetActive(true);
    }

    private void ChangeDataGame()
    {
        GameDataManager.AddCoin(GameManager.Instance.Coin);
        GameManager.Instance.Coin = 0;
    }




    // Handle Button Reward: Show, Hide
    private void ShowButtonReward()
        => _rewardButton.gameObject.SetActive(true);

    private void HideButtonReward()
        => _rewardButton.gameObject.SetActive(false);
    



    // Handle Ads Reward
    private void OnCompleteAds(int i)
    {
        if(i != (int) VideoAdsId.Reward1) return;
        StartCoroutine(DelayCompleteAds());
    }

    private IEnumerator DelayCompleteAds()
    {
        var t = 5;
        while (t > 0)
        {
            yield return new WaitForEndOfFrame();
            t--;
            if (t == 0)
            {
                GameManager.Instance.Coin *= 3;
                GameDataManager.AddCoin(GameManager.Instance.Coin);
            }
        }
        GameSharedUI.instance.UpdateCoinsTextUI();
        GameManager.Instance.Coin = 0;
        HandlerAdsReward();
    }

    private void HandlerAdsReward()
    {
        GameManager.Instance.NextLevel();
        CloseVictoryPanel();
        this.PostEvent(EventID.GameStartUI);
        this.PostEvent(EventID.IsPlayGame, true);
    }

    private void FailedAds()
    {
        Debug.Log("Ads not");
    }
}
