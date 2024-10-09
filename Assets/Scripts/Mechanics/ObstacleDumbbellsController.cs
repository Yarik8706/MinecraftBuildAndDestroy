
using System;
using Mechanics;
using UnityEngine;

public class ObstacleDumbbellsController : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayers;
    private bool _wasInAir;
    private Rigidbody _rigidbody3d;
    
    private float _jumpTimeReload = 0.5f;
    private float _activeJumpTimeReload;

    private void Start()
    {
        _rigidbody3d = GetComponent<Rigidbody>();
    }
        
    private void Update()
    {
        if (_activeJumpTimeReload > 0)
        {
            _activeJumpTimeReload -= Time.deltaTime;
        }
        if(_rigidbody3d.velocity.y < -2)
        {
            _wasInAir = true;
        }
    }

    private void OnMouseDown()
    {
        if (_activeJumpTimeReload > 0) return;
        Vector3 resultJumpDirection;

        if (!Physics.Raycast(transform.position, Vector3.right, 
                out var rightHit, 10, groundLayers))
        {
            resultJumpDirection = Vector3.right;
        } 
        else if (!Physics.Raycast(transform.position, Vector3.left,
                     out var leftHit, 10, groundLayers))
        {
            resultJumpDirection = Vector3.left; 
        }
        else
        {
            resultJumpDirection = rightHit.distance <= leftHit.distance ? Vector3.left : Vector3.right;
        }
        
        _rigidbody3d.AddForce((Vector3.up + resultJumpDirection) * 7, ForceMode.Impulse);
        _activeJumpTimeReload = _jumpTimeReload;
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
