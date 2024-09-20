using Mechanics;
using UnityEngine;

namespace Platformer.Mechanics
{
    public class DeleteBlock : MonoBehaviour
    {
        [SerializeField] private GameObject _deleteObjectEffect;
        
        public bool isStartDelete;

        public static bool IsDeletingNow;
        
        public void StartDelete()
        {
            isStartDelete = true;
        }
        
        public void StopDelete()
        {
            isStartDelete = false;
        }
        
        private void Update()
        {
            IsDeletingNow = false;
            if(!isStartDelete 
               || !Input.GetMouseButtonDown(0) 
               || SpawnBlock.IsSpawningNow 
               || BuildsAndDestroysCount.Instance.DestroyCount <= 0) return;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var raycastHit, 5000, LayerMask.GetMask("Block"))) return;
            if (raycastHit.collider.TryGetComponent(out BuildingBlock buildingBlock))
            {
                if (!buildingBlock.blockTypes.Contains(BlockType.DoNotSpendAction))
                {
                    BuildsAndDestroysCount.Instance.AddDestroyCount(-1);
                }

                if (buildingBlock.blockTypes.Contains(BlockType.DoReturnBuildAction))
                {
                    BuildsAndDestroysCount.Instance.AddBuildCount(1);
                }
            }
            else
            {
                BuildsAndDestroysCount.Instance.AddDestroyCount(-1);
            }
            IsDeletingNow = true;
            Instantiate(_deleteObjectEffect, raycastHit.collider.transform.position, Quaternion.identity);
            Destroy(raycastHit.collider.gameObject);
        }
    }
}