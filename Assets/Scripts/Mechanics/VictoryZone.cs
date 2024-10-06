using Flatformer.GameData;
using Mechanics;
using Platformer.Observer;
using UnityEngine;


namespace Platformer.Mechanics
{
    public class VictoryZone : MonoBehaviour, ICanDied
    {
        [SerializeField] private Animator _myAnimator;
        [SerializeField] private AudioClip deathAudio;
        
        private int reward = 50;

        private const string IS_FAIL = "IsFail";
        
        private void OnCollisionEnter(Collision other)
        {
            var player = other.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                _myAnimator.SetBool(IS_FAIL, true);
                GameManager.Instance.PlayerWin(player, reward);
            }
            if (other.gameObject.CompareTag("Bomb"))
            {
                Die();
                return;
            }
            if (other.gameObject.CompareTag("Dumbbells"))
            {
                Die();
                return;
            }
            if (other.gameObject.CompareTag("Trap"))
            {
                Die();
                return;
            }

            if (other.gameObject.CompareTag("Zombie"))
            {
                Die();
                return;
            }
        }

        public void Die()
        {
            _myAnimator.SetTrigger("Death");
            SoundManager.Instance.PlayAudioSound(deathAudio);
            EventDispatcherExtension.PostEvent(EventID.Lose);
            Destroy(gameObject, 3f);
        }
    }
}

