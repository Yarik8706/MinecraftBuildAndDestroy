using System;
using Platformer.Observer;
using System.Collections;
using System.Collections.Generic;
using Flatformer.GameData;
using Mechanics;
using UnityEngine;


namespace Platformer.Mechanics
{
    public class EnemyZombieController : Enemy
    {
        [SerializeField] private float maxSpeed;
        [SerializeField] private LayerMask groundLayerMask;
        [SerializeField] private Animator _myAnimator;
        [SerializeField] private GameObject _floatingText;
        [SerializeField] private AudioClip zombieAudio;

        private const string IS_DEATH = "IsDeath";

        private void Update()
        {
            HandleMovement();
            HandleInteractions();
        }
        
        private void HandleInteractions()
        {
            Ray forwardDirection = new Ray(transform.position + new Vector3(0, 0.5f, 0), transform.TransformDirection(Vector3.forward));
            if (Physics.Raycast(forwardDirection, 0.65f, groundLayerMask))
            {
                transform.Rotate(0, -180, 0);
            }
        }

        private void HandleMovement()
        {
            var translate = Vector3.forward * (Time.deltaTime * maxSpeed);
            transform.Translate(translate);
        }

        protected override void IsAttack()
        {
            SoundManager.Instance.PlayAudioSound(zombieAudio);
            maxSpeed = 0f;
        }

        public override void Die()
        {
            base.Die();
            maxSpeed = 0;
            _myAnimator.SetBool(IS_DEATH, true);
        }
    }
}
