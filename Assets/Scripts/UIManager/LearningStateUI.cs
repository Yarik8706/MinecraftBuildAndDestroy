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
        
        public const int LevelWhenLearningStop = 26;

        private bool _learningMessageState = true;
        
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
               _freeLearningCountText.transform.parent.gameObject.SetActive(false);
               _learningStateInfo.SetActive(false);
            }
            else if(GameDataManager.GetLevel() < LevelWhenLearningStop)
            {
                _learningStateInfo.SetActive(!YandexGame.savesData.isNeedLearning);
            }
            EventDispatcherExtension.RegisterListener(EventID.EditMode, (param) =>
            {
                if(!(bool)param) return;
                switch (GameDataManager.GetLevel())
                {
                    case 0:
                        _learningMessage.SetActive(false);
                        break;
                    case 1:
                        if(_learningMessageState) _learningMessage.SetActive(true);
                        break;
                    case 2:
                        _learningMessage.SetActive(false);
                        break;
                }
                if(GameDataManager.GetLevel() >= LevelWhenLearningStop)
                {
                    _learningStateInfo.SetActive(false);
                    _learningMessage.SetActive(false);
                    if (YandexGame.savesData.freeLearningCount > 0)
                    {
                        _freeLearningCountText.transform.parent.gameObject.SetActive(true);
                        _freeLearningCountText.text = YandexGame.savesData.freeLearningCount.ToString();
                    }

                    YandexGame.savesData.isNeedLearning = false;
                    YandexGame.SaveProgress();
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
                if(YandexGame.savesData.isNeedLearning) return;
                if (YandexGame.savesData.freeLearningCount != 0)
                {
                    YandexGame.savesData.freeLearningCount--;
                    if (YandexGame.savesData.freeLearningCount == 0)
                    {
                        _adsIcon.SetActive(true);
                        _freeLearningCountText.transform.parent.gameObject.SetActive(false);
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
            _learningMessageState = isNeedLearning;
            GameStartUI.OnReplayGameButton();
        }
    }
}