using System;
using System.Collections;
using Platformer.Observer;
using UnityEngine;

namespace Flatformer.GameData
{
    public class WaitForWin : MonoBehaviour
    {
        
        public static WaitForWin Instance { get; private set; }
        
        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            StartListening();
        }

        public void StartListening()
        {
            EventDispatcherExtension.RegisterListener(EventID.OnCarMove, (param) =>
            {
                if((bool)param) StartCoroutine(TimeDelayForWinCoroutine());
            });
        }
        
        public void StopListening()
        {
            EventDispatcher.Instance.RemoveListener(EventID.OnCarMove, (param) =>
            {
                if((bool)param) StartCoroutine(TimeDelayForWinCoroutine());
            });
        }

        private IEnumerator TimeDelayForWinCoroutine()
        {
            yield return new WaitForSeconds(3f);
            EventDispatcherExtension.PostEvent(EventID.Victory);
        }
    }
}