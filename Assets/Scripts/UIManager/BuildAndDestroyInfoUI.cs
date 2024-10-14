using Mechanics;
using TMPro;
using UnityEngine;

namespace UIManager
{
    public class BuildAndDestroyInfoUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _buildCountText;
        [SerializeField] private TMP_Text _destroyCountText;
        
        public static BuildAndDestroyInfoUI Instance { get; private set; }
        
        private void Awake()
        {
            Instance = this;
        }
        
        public void SetBuildCount(int count)
        {
            _buildCountText.text = (count.ToString());
        }
        
        public void SetDestroyCount(int count)
        {
            _destroyCountText.text = (count.ToString());
        }

        public void AddBuildCount(int count)
        {
            SetBuildCount(BuildsAndDestroysCount.Instance.BuildCount + count);
        }
        
        public void AddDestroyCount(int count)
        {
            SetDestroyCount(BuildsAndDestroysCount.Instance.DestroyCount + count);
        }
    }
}