using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using Mechanics;
using UnityEngine;

public class BombController : MonoBehaviour, ICanDied
{
    public GameObject exeplosionEffect;
    [SerializeField] private GameObject fireBombEffect;
    [SerializeField] private GameObject smokeEffect;
    [SerializeField] private Transform effectPositionTransform;

    public AudioClip exeplosionClip;
    
    private bool wasInAir;
    private Rigidbody body;
    private GameObject objFireEffect;

    private void Start()
    {
        body= GetComponent<Rigidbody>();
        objFireEffect = Instantiate(fireBombEffect, effectPositionTransform);
        GameManager.Instance.listEffect.Add(objFireEffect);
    }
    
    private void Update()
    {
        if(body.velocity.y < -2)
        {
            wasInAir = true;
        }
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Dumbbells"))
        {
            Explosion();
            return;
        }
        if (other.gameObject.CompareTag("Trap"))
        {
            Explosion();
            return;
        }
        if (other.gameObject.CompareTag("Ground") && wasInAir && other.contacts[0].normal.y > 0.8f){
            
            Explosion();
            return;
        }
        // collision in Enemy
        if(other.gameObject.CompareTag("Gangster"))
        {
            Explosion();
            return;
        }
        if(other.gameObject.CompareTag("Zombie"))
        {
            Explosion();
            return;
        }
        
        if (other.gameObject.CompareTag("Player") )
        {
            Explosion();
            return;
        }
        if (other.gameObject.CompareTag("Pin")&& wasInAir)
        {
            Explosion();
        }
    }

    private void Explosion()
    {
        Instantiate(exeplosionEffect, transform.position, Quaternion.identity);
        SoundManager.Instance.PlayAudioSound(exeplosionClip);
        Destroy(gameObject);
        Destroy(objFireEffect.gameObject);
    }

    public void Die()
    {
        Explosion();
    }
}
