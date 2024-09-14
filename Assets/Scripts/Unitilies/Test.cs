using Platformer.Observer;
using UnityEngine;

public class Test : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] EventID evenID;

    bool isDamageable;

    private void SetIsDamageable(bool isDamageable)
    {
        this.isDamageable = isDamageable;
    }
    private void OnEnable()
    {
        this.RegisterListener(evenID, (param) => SetIsDamageable((bool)param));
    }
    private void OnDisable()
    {
        if (EventDispatcher.HasInstance())
        {
            EventDispatcher.Instance.RemoveListener(evenID, (param) => SetIsDamageable((bool)param));
        }
    }
    private void Update()
    {
        if(isDamageable)
        {
            Destroy(gameObject);
        }
    }
    
}
