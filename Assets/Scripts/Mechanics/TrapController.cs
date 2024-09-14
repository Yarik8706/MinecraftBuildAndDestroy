
using UnityEngine;

public class TrapController : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private AudioClip trapClip;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Dumbbells"))
        {
            Instantiate(explosionPrefab, transform.GetChild(0).position, Quaternion.identity);
            Destroy(gameObject);
            return;
        }

        if (other.gameObject.CompareTag("Bomb"))
        {
            Destroy(gameObject);
            return;
        }
        if (other.gameObject.CompareTag("Gangster"))
        {
            Instantiate(explosionPrefab, transform.GetChild(0).position, Quaternion.identity);
            SoundManager.instance.PlayAudioSound(trapClip);
            Destroy(gameObject);
            return;
        }
        if (other.gameObject.CompareTag("Zombie"))
        {
            Instantiate(explosionPrefab, transform.GetChild(0).position, Quaternion.identity);
            SoundManager.instance.PlayAudioSound(trapClip);
            Destroy(gameObject);
            return;
        } 
        if (other.gameObject.CompareTag("Player"))
        {
            Instantiate(explosionPrefab, transform.GetChild(0).position, Quaternion.identity);
            SoundManager.instance.PlayAudioSound(trapClip);
            Destroy(gameObject);
        }
    }

}
