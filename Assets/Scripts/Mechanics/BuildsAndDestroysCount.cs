using System;
using UIManager;
using UnityEngine;

namespace Mechanics
{
    public class BuildsAndDestroysCount : MonoBehaviour
    {
        private int _buildCount;
        private int _destroyCount;
        
        public int BuildCount
        {
            get
            {
                return _buildCount;
            }
        }    
        
        public int DestroyCount
        {
            get
            {
                return _destroyCount;
            }
        }
        
        public static BuildsAndDestroysCount Instance { get; private set; }
        
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            ResetData();
        }

        public void AddBuildCount(int count)
        {
            BuildAndDestroyInfoUI.Instance.AddBuildCount(count);
            _buildCount += count;
        }
        
        public void AddDestroyCount(int count)
        {
            BuildAndDestroyInfoUI.Instance.AddDestroyCount(count);
            _destroyCount += count;
        }

        public void ResetData()
        {
            BuildAndDestroyInfoUI.Instance.SetBuildCount(6);
            BuildAndDestroyInfoUI.Instance.SetDestroyCount(6);
            _destroyCount = 6;
            _buildCount = 6;
        }
    }
}