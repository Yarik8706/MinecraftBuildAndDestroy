using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGangsterController : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 2f;
    [SerializeField] private LayerMask layerMaskMovement;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private Animator animator;

    public AudioClip deathAudio;
    public float raycastDistance = 3f;

    private bool canMove;
    private Rigidbody body;

    private const string IS_RUN = "IsRun";
    private const string IS_ATTACK = "IsAttack";
    private const string IS_DEATH = "IsDeath";

    public void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!canMove)
        {
            PerformInteractions();

        }
        else
        {
            HandleMovement();
        }
    }

    // xu ly tuong tac
    private void PerformInteractions()
    {
        RaycastHit hitLeft;
        var leftDirection = new Ray(transform.position + new Vector3(0, 0.5f, 0), transform.TransformDirection(Vector3.forward));
        var rightDirection = new Ray(transform.position + new Vector3(0, 0.5f, 0), -transform.TransformDirection(Vector3.forward));


        if (!Physics.Raycast(leftDirection, out hitLeft, raycastDistance, layerMaskMovement))
        {
            RaycastHit hitRight;
            if (!Physics.Raycast(rightDirection, out hitRight, raycastDistance, layerMaskMovement))
            {
                if (Physics.Raycast(leftDirection, raycastDistance, LayerMask.GetMask("Player")))
                {
                    // transform.rotation = Quaternion.Euler(0, 90f, 0);
                    canMove = true;
                }
                else if (Physics.Raycast(rightDirection, raycastDistance, LayerMask.GetMask("Player")))
                {
                    // transform.rotation = Quaternion.Euler(0, -90f, 0);
                    canMove = true;
                }
            }
        }
    }
    // xu ly di chuyen
    private void HandleMovement()
    {
        Ray fowardDirection = new Ray(transform.position + new Vector3(0, 0.5f, 0), transform.TransformDirection(Vector3.forward));

        bool isDir = Physics.Raycast(fowardDirection, 0.5f, groundLayerMask);
        var translate = Vector3.forward * Time.deltaTime * maxSpeed;
        bool isRun = translate != Vector3.zero;
        if (isRun)
        {
            animator.SetBool(IS_RUN, isRun);
        }
        transform.Translate(translate);
        if (isDir)
        {
            transform.Rotate(0, -180, 0);
        }

    }


    private void OnCollisionEnter(Collision other)
    {
        var p = other.gameObject.GetComponent<PlayerController>();
        if (p != null)
        {
            IsAttack();
            return;
        }

        if (other.gameObject.CompareTag("Bomb"))
        {
            IsDeath();
            Destroy(gameObject, 1f);
            return;
        }
        if (other.gameObject.CompareTag("Dumbbells"))
        {
            IsDeath();
            Destroy(gameObject, 1f);
            return;
        }
        if (other.gameObject.CompareTag("Trap"))
        {
            IsDeath();
            Destroy(gameObject, 1f);
            return;
        }
        if (other.gameObject.CompareTag("Zombie"))
        {
            IsDeath();
            Destroy(gameObject, 1f);
        }
    }

    private void IsDeath()
    {
        maxSpeed = 0;
        SoundManager.instance.PlayAudioSound(deathAudio);
        animator.SetBool(IS_DEATH, true);
    }
    private void IsAttack()
    {
        maxSpeed = 0;
        animator.SetBool(IS_ATTACK, true);
    }
}
