using System;
using System.Collections;
using Flatformer.GameData;
using Platformer.Mechanics;
using ShopMechanics;
using UIManager;
using UnityEngine;
using YG;

public class YandexControl : MonoBehaviour
{
    private void Awake()
    {
        MultiTextUI.lang = YandexGame.lang;
        StartCoroutine(YandexSDKEnabledCoroutine());
    }

    public IEnumerator YandexSDKEnabledCoroutine()
    {
        yield return new WaitUntil(() => YandexGame.SDKEnabled);
        YandexGame.InitEnvirData();
        Optimizer.Instance.Init();
        GameDataManager.InitData();
        YandexGame.NewLeaderboardScores("Score", YandexGame.savesData.allMoney);
        YandexGame.GetLeaderboard("Score",
            Int32.MaxValue, Int32.MaxValue, 
            Int32.MaxValue, "nonePhoto");
        ShopManager.Instance.Init();
        GameSharedUI.instance.Init();
        EveryDayRewardUI.Instance.Init();
        LearningStateUI.Instance.Init();
        GameStartUI.Instance.Init();
    }
}