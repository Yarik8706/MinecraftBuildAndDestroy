using DG.Tweening;
using Platformer.Mechanics;
using Platformer.Observer;
using System.Collections;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [SerializeField] private Transform _target;

    [SerializeField] private float _timeMovement = 2f;


    [SerializeField] GameObject effect;
    [SerializeField] AudioClip policeAudio;
    private bool canMove;

    private void SetCanMove(bool canMove)
    {
        this.canMove = canMove;
    }
    private void OnEnable()
    {
        this.RegisterListener(EventID.OnCarMove, (param) => SetCanMove((bool)param));
    }
    private void OnDisable()
    {
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
            CarMoveToTarget();
            SoundManager.instance.PlayAudioSound(policeAudio);
            canMove = false;
        }
    }


    private void CarMoveToTarget()
    {
        transform.DOComplete();
        transform.DOMove(_target.GetChild(0).transform.position,2f);
       
        effect.SetActive(false);
    }
    private void PlayAudio()
    {
        SoundManager.instance.PlayAudioWin();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(DelayCarMove());
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Victory Zone"))
        {
            Destroy(other.gameObject);
        }
    }
    private IEnumerator DelayCarMove()
    {
        yield return new WaitForSeconds(1f);
        transform.DOMove(_target.GetChild(1).transform.position, 2f);
        effect.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        this.PostEvent(EventID.Victory);
    }
}
