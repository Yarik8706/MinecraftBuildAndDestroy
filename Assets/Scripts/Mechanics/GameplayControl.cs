using System;
using DG.Tweening;
using Flatformer.GameData;
using Flatformer.GameData.UIManager;
using Mechanics;
using Platformer.Observer;
using UnityEngine;

namespace Platformer.Mechanics
{
    public class GameplayControl : MonoBehaviour
    {
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private Transform _baseCameraTransform;
        [SerializeField] private Transform _editCameraTransform;
        [SerializeField] private SpawnBlock _spawnBlockControl;
        [SerializeField] private DeleteBlock _deleteBlockControl;
        [SerializeField] private GameObject _blockGridBackground;
        
        private bool _editMode;

        private const float TimeTransitionToEditMode = 0.5f;

        public static GameplayControl Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            _blockGridBackground.SetActive(false);
            EventDispatcherExtension.RegisterListener(EventID.Home, 
                (param) => ExitToMenu());
            EventDispatcherExtension.RegisterListener(EventID.StartGame, 
                (param) => ActivateEditModeAfterSomeTime());
            EventDispatcherExtension.RegisterListener(EventID.Victory, 
                (param) => Invoke(nameof(SetIdleLevelState), 2f));
            EventDispatcherExtension.RegisterListener(EventID.Lose, 
                (param) => Invoke(nameof(SetIdleLevelState), 2f));
        }

        private void Start()
        {
            SetIdleLevelState();
        }

        public void ActivateEditModeAfterSomeTime()
        {
            Invoke(nameof(ActivateLevel), TimeTransitionToEditMode);
        }

        public void ActivateLevel()
        {
            if(GameState.IsGameStart) return;
            if(_editMode) return;
            EventDispatcher.Instance.PostEvent(EventID.EditMode, true);
            _editMode = true;
            ResetState();
            GameplayMechanicsStateUI.Instance.SetReloadButtonActive(false);
            GameplayMechanicsStateUI.Instance.SetUIWhenLevelActive(true);
            _cameraTransform.DOMove(_editCameraTransform.position, 0.5f).OnComplete(() =>
            {
               _blockGridBackground.SetActive(true); 
               GameState.IsEditMode = true;
               _deleteBlockControl.StartDelete();
               _spawnBlockControl.StartSpawn();
            });
            _cameraTransform.DORotate(_editCameraTransform.rotation.eulerAngles, 0.5f);
        }

        public void ResetState()
        {
            _spawnBlockControl.ResetSpawn();
            BuildsAndDestroysCount.Instance.ResetData();
        }

        private void ExitToMenu()
        {
            SetIdleLevelState();
            GameManager.Instance.ReplayGame();
        }

        public void StartLevelPlayback()
        {
            _editMode = false;
            GameState.IsEditMode = false;
            EventDispatcher.Instance.PostEvent(EventID.EditMode, false);
            GameState.IsGameStart = true;
            GameplayMechanicsStateUI.Instance.SetUIWhenLevelActive(false);
            _cameraTransform.DOMove(_baseCameraTransform.position, 0.5f);
            _cameraTransform.DORotate(_baseCameraTransform.rotation.eulerAngles, 0.5f);
            _deleteBlockControl.StopDelete();
            _spawnBlockControl.StopSpawn();
            _blockGridBackground.SetActive(false);
            GameplayMechanicsStateUI.Instance.SetReloadButtonActive(false);
        }

        public void SetIdleLevelState()
        {
            _editMode = false;
            GameState.IsEditMode = false;
            EventDispatcher.Instance.PostEvent(EventID.EditMode, false);
            ResetState();
            GameplayMechanicsStateUI.Instance.SetReloadButtonActive(false);
            GameplayMechanicsStateUI.Instance.SetUIWhenLevelActive(false);
            _cameraTransform.position = _baseCameraTransform.position;
            var rotation = _cameraTransform.rotation;
            rotation.eulerAngles = _baseCameraTransform.rotation.eulerAngles;
            _cameraTransform.rotation = rotation;
            _blockGridBackground.SetActive(false);
            _deleteBlockControl.StopDelete();
            _spawnBlockControl.StopSpawn();
        }
    }
}