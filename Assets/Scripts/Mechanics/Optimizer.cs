using System;
using UnityEngine;
using YG;

namespace Platformer.Mechanics
{
    public class Optimizer : MonoBehaviour
    {
        [SerializeField] private GameObject[] notMobileObjects;
        
        public static Optimizer Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            
        }

        public void Init()
        {
            if (!YandexGame.EnvironmentData.isDesktop)
            {
                Application.targetFrameRate = 30;
                QualitySettings.vSyncCount = 2;
                foreach (var notMobileObject in notMobileObjects)
                {
                    notMobileObject.SetActive(false);
                } 
            }
        }
    }
}