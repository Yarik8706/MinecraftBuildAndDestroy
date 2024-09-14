using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public GameObject exeplosionEffect;
    [SerializeField] private GameObject fireBombEffect;
    [SerializeField] private GameObject smokeEffect;

    public AudioClip exeplosionClip;

   
    private bool isGrounded;
    private Rigidbody body;
    Vector3 currentPos = new Vector3(0.2f, 1.2f, -0.1f);
    private GameObject objFireEffect;

    private void Start()
    {
        body= GetComponent<Rigidbody>();
        objFireEffect = Instantiate(fireBombEffect);
        GameManager.Instance.listEffect.Add(objFireEffect);
    }
    private void Update()
    {
        if(body.velocity.y < -2)
        {
            isGrounded = true;
        }
        objFireEffect.transform.position = transform.position + currentPos;
        
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Dumbbells"))
        {
            Exeplosion();
            return;
        }
        if (other.gameObject.CompareTag("Trap"))
        {
            Exeplosion();
            return;
        }
        if (other.gameObject.CompareTag("Ground") && isGrounded){
            Exeplosion();
            return;
        }
        if (other.gameObject.CompareTag("Block")){
            Exeplosion();
            return;
        }
        // collision in Enemy
        if(other.gameObject.CompareTag("Gangster"))
        {
            Exeplosion();
            return;
        }
        if(other.gameObject.CompareTag("Zombie"))
        {
            Exeplosion();
            return;
        }
        // collison in Player
        if (other.gameObject.CompareTag("Player") )
        {
            Exeplosion();
            return;
        }
        if (other.gameObject.CompareTag("Pin")&& isGrounded)
        {
            Exeplosion();
        }
     
    }

    private void Exeplosion()
    {
        Instantiate(exeplosionEffect, transform.position, Quaternion.identity);
        
        
        SoundManager.instance.PlayAudioSound(exeplosionClip);
        Destroy(gameObject);
        Destroy(objFireEffect.gameObject);
    }
   
}
