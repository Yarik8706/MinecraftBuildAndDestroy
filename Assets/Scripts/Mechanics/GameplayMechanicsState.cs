using System;
using Flatformer.GameData;
using Flatformer.GameData.UIManager;
using Platformer.Observer;
using UnityEngine;

namespace Platformer.Mechanics
{
    public class GameplayMechanicsState : MonoBehaviour
    {
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private Transform _baseCameraTransform;
        [SerializeField] private Transform _editCameraTransform;
        [SerializeField] private SpawnBlock _spawnBlockControl;
        [SerializeField] private DeleteBlock _deleteBlockControl;
        
        public static GameplayMechanicsState Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            EventDispatcher.Instance.RegisterListener(EventID.EndGame, OnGameStartUI);
        }

        public void StartLevelPlayback()
        {
            GameState.IsGameStart = true;
            GameplayMechanicsStateUI.Instance.SetUIWhenGameStart(false);
        }
        
        public void SetSpawnBlockMode()
        {
            _spawnBlockControl.StartSpawn();
            _deleteBlockControl.StopDelete();
        }
        
        public void SetDeleteBlockMode()
        {
            _deleteBlockControl.StartDelete();
            _spawnBlockControl.StopSpawn();
        }
    }
}