
using Mechanics;
using UnityEngine;

public class TrapController : MonoBehaviour, ICanDied
{
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private AudioClip trapClip;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Dumbbells"))
        {
            Die();
            return;
        }

        if (other.gameObject.CompareTag("Bomb"))
        {
            Die();
            return;
        }
        if (other.gameObject.CompareTag("Gangster"))
        {
            Die();
            return;
        }
        if (other.gameObject.CompareTag("Zombie"))
        {
            Die();
            return;
        } 
        if (other.gameObject.CompareTag("Player"))
        {
            Die();
        }
    }

    public void Die()
    {
        SoundManager.Instance.PlayAudioSound(trapClip);
        Instantiate(explosionPrefab, transform.GetChild(0).position, Quaternion.identity);
        Destroy(gameObject);
    }
}
