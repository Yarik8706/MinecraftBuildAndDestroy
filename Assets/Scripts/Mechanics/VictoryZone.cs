using Flatformer.GameData;
using Platformer.Observer;
using UnityEngine;


namespace Platformer.Mechanics
{
    public class VictoryZone : MonoBehaviour
    {
        [SerializeField] private Animator _myAnimator;
        
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
        }
    }
}

