using UnityEngine;

namespace Platformer.Mechanics
{
    public class DeleteBlock : MonoBehaviour
    {
        [SerializeField] private GameObject _deleteObjectEffect;
        
        public bool isStartDelete;
        
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
            if(!Input.GetMouseButtonDown(0) || !isStartDelete) return;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var raycastHit, 5000, LayerMask.GetMask("Block"))) return;
            Instantiate(_deleteObjectEffect, raycastHit.collider.transform.position, Quaternion.identity);
            Destroy(raycastHit.collider.gameObject);
        }
    }
}