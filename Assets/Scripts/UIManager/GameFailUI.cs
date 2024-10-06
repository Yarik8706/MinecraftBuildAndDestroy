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
    [SerializeField] private Button _replayButton;
    [SerializeField] private Button _rewardButton;
    
    private void OnEnable()
    {
        YandexGame.RewardVideoEvent += OnCompleteAds;
        EventDispatcherExtension.RegisterListener(EventID.Lose, (param) => Invoke(nameof(OpenLosePanel), 1f));
    }

    private void OnDisable()
    {
        YandexGame.RewardVideoEvent -= OnCompleteAds;
        if (EventDispatcher.HasInstance())
        {
            EventDispatcher.Instance.RemoveListener(EventID.Lose, (param) => Invoke(nameof(OpenLosePanel), 1f));
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
            SoundManager.Instance.PlayAudioSound(SoundManager.Instance.buttonAudio);
            OnReplayGame();
        });

        _rewardButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayAudioSound(SoundManager.Instance.buttonAudio);
            YandexGame.RewVideoShow((int)VideoAdsId.Reward);
        });
    }

    private void OnReplayGame()
    {
        EventDispatcherExtension.PostEvent(EventID.StartGame);
        EventDispatcherExtension.PostEvent(EventID.GameStartUI);
        GameManager.Instance.ReplayGame();
        EventDispatcherExtension.PostEvent(EventID.IsPlayGame, true);
        Hide();
    }

    private void OpenLosePanel()
    {
        SoundManager.Instance.PlayAudioFail();
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
        EventDispatcherExtension.PostEvent(EventID.StartGame);
        EventDispatcherExtension.PostEvent(EventID.GameStartUI);
        EventDispatcherExtension.PostEvent(EventID.IsPlayGame, true);
        Hide();
    }
}
