using Platformer.Mechanics;
using TMPro;
using UnityEngine;

namespace Flatformer.GameData.UIManager
{
    public class GameplayMechanicsStateUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _actionCountText;
        [SerializeField] private GameObject _editModeUIContainer;
        [SerializeField] private GameObject _levelStartButtonUIContainer;
        [SerializeField] private GameObject _deleteBlockModeImage;
        [SerializeField] private GameObject _spawnBlockModeImage;
        
        public static GameplayMechanicsStateUI Instance { get; private set; }
        
        private void Awake()
        {
            Instance = this;
        }

        public void StartLevelPlaybackUI()
        {
            GameplayMechanicsState.Instance.StartLevelPlayback();
        }
        
        public void SetDeleteBlockMode()
        {
            _deleteBlockModeImage.SetActive(true);
            GameplayMechanicsState.Instance.SetDeleteBlockMode();
        }
        
        public void SetSpawnBlockMode()
        {
            _spawnBlockModeImage.SetActive(true);
            GameplayMechanicsState.Instance.SetSpawnBlockMode();
        }

        public void SetUIWhenGameStart(bool state)
        {
            _editModeUIContainer.SetActive(state);
            _levelStartButtonUIContainer.SetActive(state);
            _actionCountText.gameObject.SetActive(state);
        }
    }
}