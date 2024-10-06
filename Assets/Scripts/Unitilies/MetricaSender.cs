using System.Collections.Generic;
using Flatformer.GameData;
using UnityEngine;
using YG;

namespace Unitilies
{
    public class MetricaSender : MonoBehaviour
    {
        public static MetricaSender Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public void SendLevelCompleteData()
        {
            Send("LevelComplete", new Dictionary<string, string>
            {
                {"LevelComplete", GameDataManager.GetLevel().ToString()}
            });
        }

        public void SendLevelFailedData()
        {
            Send("LevelFailed", new Dictionary<string, string>
            {
                {"LevelFailed", GameDataManager.GetLevel().ToString()}
            });
        }

        public void SendLevelStartData()
        {
            Send("LevelStart", new Dictionary<string, string>
            {
                {"LevelStart", GameDataManager.GetLevel().ToString()}
            });
        }
        
        public void SendLevelReplayData()
        {
            Send("LevelReplay", new Dictionary<string, string>
            {
                {"LevelReplay", GameDataManager.GetLevel().ToString()}
            });
        }


        private void Send(string id, Dictionary<string, string> data)
        {
            YandexMetrica.Send(id, data);
        }
    }
}