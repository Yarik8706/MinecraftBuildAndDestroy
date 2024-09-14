using System;
using UnityEngine;

namespace Platformer.Mechanics
{
    public class SpawnBlock : MonoBehaviour
    {
        private bool _isStartSpawn;

        [SerializeField] private Vector3 _spawnOffset;
        [SerializeField] private GameObject _spawnObject;
        [SerializeField] private LayerMask _spawnLayer;
        
        public void StartSpawn()
        {
            _isStartSpawn = true;
        }
        
        public void StopSpawn()
        {
            _isStartSpawn = false;
        }

        private void Update()
        {
            if(!_isStartSpawn || !Input.GetMouseButtonDown(0)) return;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var checkRay = Physics.Raycast(ray, out var checkHit, 5000);
            if (checkRay && checkHit.collider.gameObject.layer.Equals(LayerMask.NameToLayer("NotPlaceBlock"))) return;
            if (!Physics.Raycast(ray, out var raycastHit, 5000, _spawnLayer)) return;

            var spawnPosition = raycastHit.point;
            spawnPosition.z = raycastHit.collider.transform.position.z;
            spawnPosition.y = Convert.ToSingle(Mathf.Round(spawnPosition.y));
            spawnPosition.x = Convert.ToSingle(Mathf.Round(spawnPosition.x));
            spawnPosition += _spawnOffset;
            
            Instantiate(_spawnObject, spawnPosition, Quaternion.identity);
        }
    }
}