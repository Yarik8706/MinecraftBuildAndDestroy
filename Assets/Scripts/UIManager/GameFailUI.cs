using Flatformer.GameData;
using Platformer.Mechanics;
using Platformer.Observer;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using YG;

public enum VideoAdsId
{
    Reward,
    TurnWheel,
    SkipLevel,
    Reward1,
    ShopReward,
    Reward2
}

public class GameFailUI : MonoBehaviour
{
    [SerializeField] private GameObject gameVictory;
    [Header("Event UI")]
    [SerializeField] 
    private Button _replayButton;
    [SerializeField] 
    private Button _rewardButton;
    
    private void OnEnable()
    {
        YandexGame.RewardVideoEvent += OnCompleteAds;
        this.RegisterListener(EventID.Loss, (param) => Invoke(nameof(Show), 1f));
    }

    private void OnDisable()
    {
        YandexGame.RewardVideoEvent -= OnCompleteAds;
        if (EventDispatcher.HasInstance())
        {
            EventDispatcher.Instance.RemoveListener(EventID.Loss, (param) => Invoke("Show", 1f));
        }
    }
    private void Start()
    {
        Hide();
        AddEvents();
    }

    private void AddEvents()
    {
        _replayButton.onClick.AddListener(() =>
        {
            if (GameManager.Instance.currentLevel >= 3)
            {
                YandexGame.FullscreenShow();
            }
            SoundManager.instance.PlayAudioSound(SoundManager.instance.buttonAudio);
            OnReplayGame();
        });

        _rewardButton.onClick.AddListener(() =>
        {
            SoundManager.instance.PlayAudioSound(SoundManager.instance.buttonAudio);
            YandexGame.RewVideoShow((int)VideoAdsId.Reward);
        });
    }

    private void OnReplayGame()
    {
        this.PostEvent(EventID.GameStartUI);
        GameManager.Instance.ReplayGame();
        this.PostEvent(EventID.IsPlayGame, true);
        Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
        => gameObject.SetActive(false);

    private void OnCompleteAds(int id)
    {
        if(id != (int) VideoAdsId.Reward) return;
        StartCoroutine(DelayAdsReward());
    }

    private IEnumerator DelayAdsReward()
    {
        GameDataManager.AddLevel(1);
        GameManager.Instance.NextLevel();
        var t = 5;
        while (t > 0)
        {
            yield return new WaitForEndOfFrame();
            --t;
        }
        this.PostEvent(EventID.GameStartUI);
        this.PostEvent(EventID.IsPlayGame, true);
        Hide();
    }
}
