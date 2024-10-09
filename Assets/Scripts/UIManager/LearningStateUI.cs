using System;
using Flatformer.GameData;
using Platformer.Mechanics;
using Platformer.Observer;
using TMPro;
using UnityEngine;
using YG;

namespace UIManager
{
    public class LearningStateUI : MonoBehaviour
    {
        [SerializeField] private GameObject _learningStateInfo;
        [SerializeField] private GameObject _learningMessage;
        [SerializeField] private TMP_Text _freeLearningCountText;
        [SerializeField] private GameObject _adsIcon;
        
        public const int LevelWhenLearningStop = 23;
        
        public static LearningStateUI Instance { get; private set; }
        
        private void Awake()
        {
            Instance = this;
        }

        public void Init()
        {
            if (!YandexGame.savesData.isFirstSession)
            {
                _learningMessage.SetActive(false);
            }
            else if (GameDataManager.GetLevel() >= LevelWhenLearningStop && YandexGame.savesData.freeLearningCount == 0)
            {
               _adsIcon.SetActive(true);
               _freeLearningCountText.gameObject.SetActive(false);
               _learningStateInfo.SetActive(false);
            }
            else if(GameDataManager.GetLevel() < LevelWhenLearningStop)
            {
                _learningStateInfo.SetActive(!YandexGame.savesData.isNeedLearning);
            }
            EventDispatcherExtension.RegisterListener(EventID.EditMode, (param) =>
            {
                switch (GameDataManager.GetLevel())
                {
                    case 0:
                        _learningMessage.SetActive(false);
                        break;
                    case 1:
                        _learningMessage.SetActive(true);
                        break;
                    case 2:
                        _learningMessage.SetActive(false);
                        break;
                }
                if(GameDataManager.GetLevel() >= LevelWhenLearningStop)
                {
                    _learningStateInfo.SetActive(false);
                    _learningMessage.SetActive(false);
                }
            });
        }

        public void StartLearning(int videoId)
        {
            if(videoId != (int)VideoAdsId.Learning) return;
            ChangeLearningState(true);
        }

        public void ShowAdsForLearning()
        {
            YandexGame.RewVideoShow((int)VideoAdsId.Learning);
        }

        private void OnDisable()
        {
            YandexGame.RewardVideoEvent -= StartLearning;
        }

        private void OnEnable()
        {
            YandexGame.RewardVideoEvent += StartLearning;
        }

        public void ChangeLearningState()
        {
            if (GameDataManager.GetLevel() >= LevelWhenLearningStop)
            {
                if (YandexGame.savesData.freeLearningCount != 0)
                {
                    YandexGame.savesData.freeLearningCount--;
                    if (YandexGame.savesData.freeLearningCount == 0)
                    {
                        _adsIcon.SetActive(true);
                        _freeLearningCountText.gameObject.SetActive(false);
                    }
                    YandexGame.SaveProgress();
                    _freeLearningCountText.text = YandexGame.savesData.freeLearningCount.ToString();
                    ChangeLearningState(true);
                }
                else
                {
                    ShowAdsForLearning();
                }
                return;
            }
            YandexGame.savesData.isNeedLearning = !YandexGame.savesData.isNeedLearning;
            _learningMessage.SetActive(false);
            _learningStateInfo.SetActive(!YandexGame.savesData.isNeedLearning);
            GameStartUI.OnReplayGameButton();
        }

        public void ChangeLearningState(bool isNeedLearning)
        {
            YandexGame.savesData.isNeedLearning = isNeedLearning;
            YandexGame.SaveProgress();
            _learningStateInfo.SetActive(!isNeedLearning);
            _learningMessage.SetActive(false);
            GameStartUI.OnReplayGameButton();
        }
    }
}