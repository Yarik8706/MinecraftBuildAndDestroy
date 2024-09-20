using System;
using Platformer.Mechanics;
using UnityEngine;
using UnityEngine.UI;

namespace UIManager
{
    public class GameplayUI : MonoBehaviour
    {
        [SerializeField] private Button _repeatButton;

        private void Start()
        {
            _repeatButton.onClick.AddListener(GameStartUI.OnReplayGameButton);
        }
    }
}