using System;
using Flatformer.GameData;
using UnityEngine;



namespace Platformer.Mechanics
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float maxSpeed = 2f;
        [SerializeField] private float maxDistance = 0.5f;
        [SerializeField] private LayerMask moveLayerMask;
        [SerializeField] private LayerMask playerDirLayerMask;

        public AudioClip coinAudio;
        public AudioClip deathAudio;
        
        public bool isControlEnable = true;

        private bool isPlayerDir;
        private bool canMove;

        private float dirZ = 1;

        public Animator _myAnimator;
        
        private const string IS_RUN = "IsRun";

        private void Update()
        {
            if (isControlEnable)
            {
                if (!canMove)
                {
                    HandleInterection();
                }
                else
                {
                    HandleMovement();
                }
            }
        }


        private void HandleInterection()
        {
            canMove = GameState.IsGameStart;
            // var DerictionLeft = new Ray(transform.position + new Vector3(0, 1f, 0), transform.TransformDirection(Vector3.forward));
            // var DerictionRight = new Ray(transform.position + new Vector3(0, 1f, 0), -transform.TransformDirection(Vector3.forward));
            // var DerictionBotton = new Ray(transform.position, transform.TransformDirection(Vector3.down));
            //
            //
            //
            // Physics.Raycast(DerictionRight, out RaycastHit hitRight, 3f, moveLayerMask);
            // Physics.Raycast(DerictionLeft, out RaycastHit hitLeft, 3f, moveLayerMask);
            // Physics.Raycast(DerictionBotton, out RaycastHit hitBotton, 0.5f, moveLayerMask);
            //
            // if (hitRight.collider != null)
            // {
            //     dirZ = 1;
            //     return;
            // }
            // if (hitLeft.collider != null)
            // {
            //     dirZ = 1;
            //     return;
            // }
            // if (hitBotton.collider != null)
            // {
            //     dirZ = 1;
            //     return;
            // }
            //
            // transform.localScale = new Vector3(1, 1, dirZ);
            // canMove = true;
        }

        private void OnGUI()
        {
            Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), transform.TransformDirection(Vector3.forward));
        }

        private void HandleMovement()
        {
            var translate = Vector3.forward * Time.deltaTime * maxSpeed;
            bool isRun = translate != Vector3.zero;
            if (isRun)
            {
                _myAnimator.SetBool(IS_RUN, isRun);
            }
            var forwardDirection = new Ray(transform.position + new Vector3(0,0.5f,0), transform.TransformDirection(Vector3.forward));
            isPlayerDir = Physics.Raycast(forwardDirection, maxDistance, playerDirLayerMask);
            if (isPlayerDir)
            {

                transform.Rotate(0, -180, 0);
            }
            transform.Translate(translate);
        }
    }
}

