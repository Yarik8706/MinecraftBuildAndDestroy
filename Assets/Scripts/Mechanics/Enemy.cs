using Platformer.Mechanics;
using Platformer.Observer;
using UnityEngine;

namespace Mechanics
{
    public interface ICanDied
    {
        public void Die();
    }
    
    public abstract class Enemy : MonoBehaviour, ICanDied
    {
        [SerializeField] private AudioClip deathAudio;
        protected bool IsDied;

        protected abstract void IsAttack();

        public virtual void Die()
        {
            if(IsDied) return;
            IsDied = true;
            FloatingTextSpawner.Instance.SpawnFloatingText("+50", 
                transform.position+Vector3.up);
            SoundManager.Instance.PlayAudioSound(PlayerController.Instance.coinAudio);
            GameManager.Instance.Coin += 50;
            Destroy(GetComponent<DeathZone>());
            Destroy(gameObject, 1f);
            if(deathAudio!=null)SoundManager.Instance.PlayAudioSound(deathAudio);
        }
        
        protected virtual void OnCollisionEnter(Collision other)
        {
            if(IsDied) return;
            var p = other.gameObject.GetComponent<PlayerController>();
            if (p != null)
            {
                IsAttack();
                return;
            }

            if (other.gameObject.CompareTag("Bomb"))
            {
                Die();
                return;
            }
            if (other.gameObject.CompareTag("Trap"))
            {
                Die();
                return;
            }
        }
    }
}