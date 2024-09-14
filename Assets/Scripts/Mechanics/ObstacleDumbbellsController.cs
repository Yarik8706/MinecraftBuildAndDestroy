
using UnityEngine;

public class ObstacleDumbbellsController : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Bomb"))
        {
            Destroy(gameObject);
        }
    }
}
