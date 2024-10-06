using UnityEngine;

namespace Mechanics
{
    public class FloatingTextSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _floatingTextPrefab;
        
        public static FloatingTextSpawner Instance { get; private set; }
        
        private Camera _mainCamera;
        
        private void Awake()
        {
            _mainCamera = Camera.main;
            Instance = this;
        }
        
        public void SpawnFloatingText(string text, Vector3 spawnPoint)
        {
            var floatingText = Instantiate(_floatingTextPrefab, spawnPoint, Quaternion.identity);
            FloatingText floatingTextComponent = floatingText.GetComponentInChildren<FloatingText>();
            floatingTextComponent.transform.forward = _mainCamera.transform.forward;
            floatingTextComponent.SetText(text);
            Destroy(floatingText, 1f);
        }
    }
}