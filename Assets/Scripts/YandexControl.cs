using System;
using System.Collections;
using Platformer.Mechanics;
using ShopMechanics;
using UnityEngine;
using YG;

namespace Flatformer.GameData
{
    public class YandexControl : MonoBehaviour
    {
        private void Awake()
        {
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
            GameStartUI.Instance.Init();
        }
    }
}