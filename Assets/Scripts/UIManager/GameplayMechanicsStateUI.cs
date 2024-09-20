using Platformer.Mechanics;
using TMPro;
using UnityEngine;

namespace Flatformer.GameData.UIManager
{
    public class GameplayMechanicsStateUI : MonoBehaviour
    {
        [SerializeField] private GameObject _editModeUIContainer;
        [SerializeField] private GameObject _levelStartButtonUIContainer;
        [SerializeField] private GameObject _reloadButton;
        
        public static GameplayMechanicsStateUI Instance { get; private set; }
        
        private void Awake()
        {
            Instance = this;
        }

        public void SetUIWhenLevelActive(bool state)
        {
            _editModeUIContainer.SetActive(state);
            _levelStartButtonUIContainer.SetActive(state);
        }
        
        public void SetReloadButtonActive(bool state)
        {
            _reloadButton.SetActive(state);
        }
    }
}