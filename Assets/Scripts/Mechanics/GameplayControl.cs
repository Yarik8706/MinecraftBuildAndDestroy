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
        
        public static GameplayControl Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            _blockGridBackground.SetActive(false);
            EventDispatcherExtension.RegisterListener(EventID.IsPlayGame, 
                param => { if((bool)param) ActivateLevel(); });
            EventDispatcherExtension.RegisterListener(EventID.Home, 
                (param) => ExitToMenu());
            EventDispatcherExtension.RegisterListener(EventID.Victory, 
                (param) => SetIdleLevelState());
            EventDispatcherExtension.RegisterListener(EventID.Lose, 
                (param) => ActivateLevel());
        }

        private void Start()
        {
            SetIdleLevelState();
        }

        public void ActivateLevel()
        {
            if(_editMode) return;
            EventDispatcher.Instance.PostEvent(EventID.StartEditMode);
            _editMode = true;
            GameState.IsEditMode = true;
            ResetState();
            GameplayMechanicsStateUI.Instance.SetReloadButtonActive(false);
            GameplayMechanicsStateUI.Instance.SetUIWhenLevelActive(true);
            _cameraTransform.DOMove(_editCameraTransform.position, 0.5f).OnComplete(() =>
            {
               _blockGridBackground.SetActive(true); 
            });
            _cameraTransform.DORotate(_editCameraTransform.rotation.eulerAngles, 0.5f);
            _deleteBlockControl.StartDelete();
            _spawnBlockControl.StartSpawn();
        }

        public void ResetState()
        {
            _spawnBlockControl.ResetSpawn();
            BuildsAndDestroysCount.Instance.ResetData();
        }

        private void ExitToMenu()
        {
            GameManager.Instance.ReplayGame();
        }

        public void StartLevelPlayback()
        {
            _editMode = false;
            GameState.IsEditMode = false;
            EventDispatcher.Instance.PostEvent(EventID.EndEditMode);
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
            EventDispatcher.Instance.PostEvent(EventID.EndEditMode);
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