using System;
using UnityEngine;

namespace Mechanics
{
    public class Chest : MonoBehaviour
    {
        private bool _wasInAir;
        private Rigidbody _rigidbody3d;

        private void Start()
        {
            _rigidbody3d = GetComponent<Rigidbody>();
        }
        
        private void Update()
        {
            if(_rigidbody3d.velocity.y < -2)
            {
                _wasInAir = true;
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (_wasInAir && other.gameObject.TryGetComponent(out ICanDied canDied))
            {
                canDied.Die();
            }

            if (other.gameObject.CompareTag("Ground"))
            {
                _wasInAir = false;
            }
        }
    }
}