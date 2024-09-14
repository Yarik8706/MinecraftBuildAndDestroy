using System;
using Platformer.Observer;
using System.Collections;
using System.Collections.Generic;
using Flatformer.GameData;
using UnityEngine;


namespace Platformer.Mechanics
{
    public class EnemyZombieController : MonoBehaviour
    {
        [SerializeField] private float maxSpeed;
        [SerializeField] private LayerMask groundLayerMask;
        [SerializeField] private Animator _myAnimator;

        public AudioClip zombieAudio;
        public AudioClip deathAudio;

        private bool _isDied;
        private const string IS_DEATH = "IsDeath";

        private void Update()
        {
            if(GameState.IsGameStart) return;
            HandleMovement();
            HandleInteractions();
        }
        private void HandleInteractions()
        {
            Ray forwardDirection = new Ray(transform.position + new Vector3(0, 0.7f, 0), transform.TransformDirection(Vector3.forward));
            if (Physics.Raycast(forwardDirection, 0.6f, groundLayerMask))
            {
                transform.Rotate(0, -180, 0);
            }
        }

        private void OnGUI()
        {
            Debug.DrawRay(transform.position + new Vector3(0, 0.7f, 0), transform.TransformDirection(Vector3.forward));
        }

        private void HandleMovement()
        {
            var translate = Vector3.forward * Time.deltaTime * maxSpeed;
            transform.Translate(translate);
        }
        
        private void OnCollisionEnter(Collision other)
        {
            if(_isDied) return;
            if (other.gameObject.CompareTag("Player"))
            {
                SoundManager.instance.PlayAudioSound(zombieAudio);
                maxSpeed = 0f;
                return;
            }
            if (other.gameObject.CompareTag("Gangster"))
            {
                SoundManager.instance.PlayAudioSound(zombieAudio);
                return;
            }
            if (other.gameObject.CompareTag("Bomb"))
            {
                EnemyDeath();
                Destroy(gameObject, 1f);
                return;
            }
            if (other.gameObject.CompareTag("Trap"))
            {
                EnemyDeath();
                Destroy(gameObject, 1f);
                return;
            }
            if (other.gameObject.CompareTag("Dumbbells"))
            {
                EnemyDeath();
                Destroy(gameObject, 1f);
            }
            if (other.gameObject.CompareTag("Victory Zone"))
            {
                SoundManager.instance.PlayAudioSound(zombieAudio);
                this.PostEvent(EventID.Loss);
                maxSpeed = 0f;
            }
        }

        private void EnemyDeath()
        {
            maxSpeed = 0;
            _isDied = true;
            Destroy(GetComponent<DeathZone>());
            _myAnimator.SetBool(IS_DEATH, true);
        }
    }
}
