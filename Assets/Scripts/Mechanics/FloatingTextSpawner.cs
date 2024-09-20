using UnityEngine;

namespace Mechanics
{
    public class FloatingTextSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _floatingTextPrefab;
        
        public static FloatingTextSpawner Instance { get; private set; }
        
        private void Awake()
        {
            Instance = this;
        }
        
        public void SpawnFloatingText(string text, Transform spawnPoint)
        {
            var floatingText = Instantiate(_floatingTextPrefab, spawnPoint.position, Quaternion.identity);
            FloatingText floatingTextComponent = floatingText.GetComponentInChildren<FloatingText>();
            floatingTextComponent.SetText(text);
            Destroy(floatingText, 1f);
        }
    }
}