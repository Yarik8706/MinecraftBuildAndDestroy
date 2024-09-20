using UnityEngine;

namespace UIManager
{
    public class BuildAndDestroyInfoUI : MonoBehaviour
    {
        [SerializeField] private Transform _buildCountContainer;
        [SerializeField] private Transform _destroyCountContainer;
        [SerializeField] private GameObject _buildCountPrefab;
        [SerializeField] private GameObject _destroyCountPrefab;
        
        public static BuildAndDestroyInfoUI Instance { get; private set; }
        
        private void Awake()
        {
            Instance = this;
        }
        
        public void SetBuildCount(int count)
        {
            for (var i = 0; i < _buildCountContainer.childCount; i++)
            {
                Destroy(_buildCountContainer.GetChild(i).gameObject);
            }
            for (var i = 0; i < count; i++)
            {
                Instantiate(_buildCountPrefab, _buildCountContainer);
            }
        }
        
        public void SetDestroyCount(int count)
        {
            for (var i = 0; i < _destroyCountContainer.childCount; i++)
            {
                Destroy(_destroyCountContainer.GetChild(i).gameObject);
            }
            for (var i = 0; i < count; i++)
            {
                Instantiate(_destroyCountPrefab, _destroyCountContainer);
            }
        }

        public void AddBuildCount(int count)
        {
            SetBuildCount(_buildCountContainer.childCount + count);
        }
        
        public void AddDestroyCount(int count)
        {
            SetDestroyCount(_destroyCountContainer.childCount + count);
        }
    }
}