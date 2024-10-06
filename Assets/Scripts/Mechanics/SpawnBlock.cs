using System;
using System.Collections.Generic;
using Mechanics;
using UnityEngine;

namespace Platformer.Mechanics
{
    public class SpawnBlock : MonoBehaviour
    {
        private bool _isStartSpawn;

        [SerializeField] private Vector3 _spawnOffset;
        [SerializeField] private GameObject _spawnObject;
        [SerializeField] private LayerMask _spawnLayer;
        [SerializeField] private LayerMask _notSpawnLayer;

        public static bool IsSpawningNow;
        
        private Camera _camera;
        
        private readonly List<GameObject> _spawnedObjects = new List<GameObject>();
        
        private void Awake()
        {
            _camera = Camera.main;
        }
        
        public void StartSpawn()
        {
            _isStartSpawn = true;
        }
        
        public void StopSpawn()
        {
            _isStartSpawn = false;
        }

        public void ResetSpawn()
        {
            foreach (var spawnedObject in _spawnedObjects)
            {
                if(spawnedObject==null) continue;
                Destroy(spawnedObject);
            }
            _spawnedObjects.Clear();
        }

        private void Update()
        {
            IsSpawningNow = false;
            if(!_isStartSpawn
               || !TimeDelayForSpawnOrDelete.Instance.CanIBuildOrDestroy()
               || !Input.GetMouseButtonDown(0) 
               || DeleteBlock.IsDeletingNow
               || BuildsAndDestroysCount.Instance.BuildCount <= 0) return;
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out _, 50, _notSpawnLayer, QueryTriggerInteraction.Collide) 
                || !Physics.Raycast(ray, out var raycastHit, 5000, _spawnLayer)) return;
            IsSpawningNow = true;
            var spawnPosition = raycastHit.point;
            spawnPosition.z = raycastHit.collider.transform.position.z;
            spawnPosition.y = Convert.ToSingle(Mathf.Round(spawnPosition.y));
            spawnPosition.x = Convert.ToSingle(Mathf.Round(spawnPosition.x));
            spawnPosition += _spawnOffset;

            BuildsAndDestroysCount.Instance.AddBuildCount(-1);
            
            var block = Instantiate(_spawnObject, spawnPosition, Quaternion.identity);
            SoundManager.Instance.PlayAudioBuild();
            TimeDelayForSpawnOrDelete.Instance.ResetTimer();
            _spawnedObjects.Add(block);
        }
    }
}