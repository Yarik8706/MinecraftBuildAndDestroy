using System;
using Flatformer.GameData;
using Platformer.Mechanics;
using UnityEngine;

namespace Mechanics
{
    public class EnemyGangsterController : Enemy
    {
        [SerializeField] private float maxSpeed = 2f;
        [SerializeField] private LayerMask layerMaskMovement;
        [SerializeField] private LayerMask groundLayerMask;
        [SerializeField] private Animator animator;
    
        private float _raycastDistance = 3f;
        private bool _canMove;

        private const string IS_RUN = "IsRun";
        private const string IS_ATTACK = "IsAttack";
        private const string IS_DEATH = "IsDeath";

        private void Start()
        {
            transform.rotation = Quaternion.Euler(0, 180f, 0);
        }

        private void Update()
        {
            if(!GameState.IsGameStart) return;
            if (!_canMove)
            {
                PerformInteractions();
            }
            else
            {
                HandleMovement();
            }
        }

        // xu ly tuong tac
        private void PerformInteractions()
        {
            RaycastHit hitLeft;
            var leftDirection = new Ray(transform.position + new Vector3(0, 0.5f, 0), transform.TransformDirection(Vector3.forward));
            var rightDirection = new Ray(transform.position + new Vector3(0, 0.5f, 0), -transform.TransformDirection(Vector3.forward));


            if (!Physics.Raycast(leftDirection, out hitLeft, _raycastDistance, layerMaskMovement))
            {
                RaycastHit hitRight;
                if (!Physics.Raycast(rightDirection, out hitRight, _raycastDistance, layerMaskMovement))
                {
                    if (Physics.Raycast(leftDirection, _raycastDistance, LayerMask.GetMask("Player")))
                    {
                        transform.rotation = Quaternion.Euler(0, 90f, 0);
                        _canMove = true;
                    }
                    else if (Physics.Raycast(rightDirection, _raycastDistance, LayerMask.GetMask("Player")))
                    {
                        transform.rotation = Quaternion.Euler(0, -90f, 0);
                        _canMove = true;
                    }
                }
            }
        }
        
        private void HandleMovement()
        {
            var forwardDirection = new Ray(transform.position + new Vector3(0, 0.5f, 0), 
                transform.TransformDirection(transform.forward));
            var translate = transform.forward * (Time.deltaTime * maxSpeed);
            
            var isRun = translate != Vector3.zero;
            if (isRun)
            {
                animator.SetBool(IS_RUN, isRun);
            }
            if (Physics.Raycast(forwardDirection, 0.5f, groundLayerMask))
            {
                transform.Rotate(0, -180, 0);
            }
            
            transform.Translate(translate);
        }

        protected override void OnCollisionEnter(Collision other)
        {
            base.OnCollisionEnter(other);

            if (other.gameObject.CompareTag("Zombie"))
            {
                Die();
            }
        }

        public override void Die()
        {
            maxSpeed = 0;
            base.Die();
            animator.SetBool(IS_DEATH, true);
        }

        protected override void IsAttack()
        {
            maxSpeed = 0;
            animator.SetBool(IS_ATTACK, true);
        }
    }
}
