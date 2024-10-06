using System;
using Flatformer.GameData;
using UnityEngine;

namespace Mechanics
{
    public class TimeDelayForSpawnOrDelete : MonoBehaviour
    {
        [SerializeField] private float TimeDelayAfterDestroyOrBuildBlock = 0.3f;
        
        private float _activeTimeDelayAfterDestroyOrBuildBlock = 0.3f;

        public static TimeDelayForSpawnOrDelete Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public bool CanIBuildOrDestroy()
        {
            return _activeTimeDelayAfterDestroyOrBuildBlock <= 0;
        }

        public void ResetTimer()
        {
            _activeTimeDelayAfterDestroyOrBuildBlock = TimeDelayAfterDestroyOrBuildBlock;
        }

        private void Update()
        {
            if (GameState.IsEditMode && _activeTimeDelayAfterDestroyOrBuildBlock > 0)
            {
                _activeTimeDelayAfterDestroyOrBuildBlock -= Time.deltaTime;
            }
        }
    }
}