using Platformer.Observer;
using UnityEngine;



namespace Platformer.Mechanics
{
    public class DeathZone : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            var player = other.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                GameManager.Instance.PlayerDeath(player);
            }
        }
    }
}

