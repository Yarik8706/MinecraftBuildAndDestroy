using UnityEngine;

namespace Flatformer.GameData.UIManager
{
    public class StartGameButton : MonoBehaviour
    {
        public void StartGame()
        {
            GameState.IsGameStart = true;
        }
    }
}