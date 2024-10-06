using System;
using DG.Tweening;
using Platformer.Mechanics;
using Platformer.Observer;
using System.Collections;
using Flatformer.GameData;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _timeMovement = 2f;
    [SerializeField] private GameObject effect;
    [SerializeField] private AudioClip policeAudio;
    [SerializeField] private AudioClip explosionAudio;
    
    private bool canMove;
    private bool isActive;
    private int _currentStep;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Bomb")
            && !other.gameObject.CompareTag("Trap")
            && !other.gameObject.CompareTag("Zombie")
            && !other.gameObject.CompareTag("Gangster")
            && other.gameObject.layer != LayerMask.NameToLayer("Block")) return;
        Destroy(other.gameObject);
        SoundManager.Instance.PlayAudioSound(explosionAudio);
    }

    private void SetCanMove(bool canMove)
    {
        this.canMove = canMove;
    }
    
    private void OnEnable()
    {
        WaitForWin.Instance.isNeedMakeWin = false;
        EventDispatcherExtension.RegisterListener(EventID.OnCarMove, (param) => SetCanMove((bool)param));
        
    }
    
    private void OnDisable()
    {
        WaitForWin.Instance.isNeedMakeWin = true;
        EventDispatcher.Instance.RemoveListener(EventID.OnCarMove, (param) => SetCanMove((bool)param));
    }

    private void Awake()
    {
        transform.position = _target.GetChild(1).transform.position;
    }

    private void Update()
    {
        if(this.canMove)
        {
            isActive = true;
            CarMoveToTarget();
            SoundManager.Instance.PlayAudioSound(policeAudio);
            canMove = false;
        }
    }


    private void CarMoveToTarget()
    {
        StartCoroutine(DelayCarMove());
        transform.DOComplete();
        transform.DOMove(_target.GetChild(0).transform.position,2f)
            .OnComplete(() => effect.SetActive(false));
       
        effect.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if(!isActive) return;
        if (other.gameObject.CompareTag("Player"))
        {
            _currentStep++;
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Victory Zone"))
        {
            Destroy(other.gameObject);
            _currentStep++;
        }
    }
    private IEnumerator DelayCarMove()
    {
        yield return new WaitUntil(() => _currentStep == 2);
        yield return new WaitForSeconds(1f);
        transform.DOMove(_target.GetChild(1).transform.position, 2f);
        effect.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        EventDispatcherExtension.PostEvent(EventID.Victory);
    }
}
