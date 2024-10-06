using System;
using Flatformer.GameData;
using Platformer;
using Platformer.Model;
using Platformer.Observer;
using System.Collections.Generic;
using Unitilies;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using YG;
using Random = UnityEngine.Random;


namespace Platformer.Mechanics
{
    public class GameManager : MonoBehaviour
    {

        #region Singleton Class: GameManager
        public static GameManager Instance { get; private set; }

        private void OnEnable()
        {
            Instance = this;
            EventDispatcherExtension.RegisterListener(EventID.Replay, (param) => ReplayGame());
        }
        #endregion
        
        [SerializeField] private PlatformerModel model;
        [SerializeField] private Canvas Canvas;
        [SerializeField] private Material gridSpriteRenderersMaterial;

        public GameObject learningBlockForStartGame;
        public GameObject learningBlockPrefab;
        public GameObject startGameButton;
        public int currentLevel;
        public List<GameObject> listEffect = new();

        private GameObject _objLevel;
        private Camera _camera;

        private const string IS_WIN = "IsWin";
        private const string IS_DEATH = "IsDeath";
        
        public int Coin { get; set; }

        private void Awake()
        {
#if UNITY_EDITOR
            YandexGame.ResetSaveProgress();
#endif
            YandexGame.InitEnvirData();
            _camera = Camera.main;
            Canvas.GetComponent<CanvasScaler>().matchWidthOrHeight =
                YandexGame.EnvironmentData.deviceType == "desktop" ? 1 : 0;
        }

        private void OnDisable()
        {
            if (EventDispatcher.HasInstance())
            {
                EventDispatcher.Instance.RemoveListener(EventID.Replay, (param) => ReplayGame());
            }
        }

        public void StartGame()
        {
            LoadNewLevel();
        }

        public void ReplayGame()
        {
            if (_objLevel != null)
            {
                Destroy(_objLevel.gameObject);
            }
            foreach (GameObject effect in listEffect)
            {
                Destroy(effect.gameObject);
            }
            _objLevel = Instantiate(model.levels[currentLevel],transform) as GameObject;
        }

        private void LoadNewLevel()
        {
            if (_objLevel != null)
                Destroy(_objLevel.gameObject);

            currentLevel = GameDataManager.GetLevel();
            var randomColor = model.backGrounds[Random.Range(0, model.backGrounds.Count)];
            _camera.backgroundColor = randomColor;
            gridSpriteRenderersMaterial.color = randomColor;
            if (GameDataManager.GetLevel() >= model.levels.Count)
            {
                currentLevel = Random.Range(10, model.levels.Count);
            }
            MetricaSender.Instance.SendLevelStartData();
            _objLevel = Instantiate(model.levels[currentLevel], transform);
        }

        public void NextLevel()
        {
            if (currentLevel > model.levels.Count)
                EventDispatcherExtension.PostEvent(EventID.EndGame);
           
            foreach(GameObject effect in listEffect)
            {
                Destroy(effect.gameObject);
            }
            LoadNewLevel();
        }

        public void PlayerDeath(PlayerController playerRef)
        {
            if(!GameState.IsGameStart && !GameState.IsEditMode) return;
            GameState.IsGameStart = false;
            EventDispatcherExtension.PostEvent(EventID.Lose);
            MetricaSender.Instance.SendLevelFailedData();
            if (playerRef != null)
            {
                playerRef.isControlEnable = false;
                playerRef.transform.rotation = Quaternion.Euler(0, 180f, 0);
                SoundManager.Instance.PlayAudioSound(playerRef.deathAudio);
                playerRef._myAnimator.SetBool(IS_DEATH, true);
            }
        }

        public void PlayerWin(PlayerController playerRef, int reward)
        {
            if(!GameState.IsGameStart && !GameState.IsEditMode) return;
            GameState.IsGameStart = false;
            Instance.Coin += reward;
            MetricaSender.Instance.SendLevelCompleteData();
            GameDataManager.AddLevel(1);
            EventDispatcherExtension.PostEvent(EventID.OnCarMove,true);
            if (playerRef != null)
            {
                playerRef.isControlEnable = false;
                playerRef.transform.rotation = Quaternion.Euler(0, 180f, 0);
                playerRef._myAnimator.SetBool(IS_WIN, true);
            }
        }
    }
}


