using System;
using System.Collections;
using Platformer.Observer;
using UnityEngine;

namespace Flatformer.GameData
{
    public class WaitForWin : MonoBehaviour
    {
        
        public static WaitForWin Instance { get; private set; }
        
        internal bool isNeedMakeWin = true;
        
        private void Awake()
        {
            Instance = this;
            StartListening();
        }

        public void StartListening()
        {
            EventDispatcherExtension.RegisterListener(EventID.OnCarMove, (param) =>
            {
                if((bool)param) StartCoroutine(TimeDelayForWinCoroutine());
            });
        }

        private IEnumerator TimeDelayForWinCoroutine()
        {
            if(!isNeedMakeWin) yield break;
            yield return new WaitForSeconds(3f);
            EventDispatcherExtension.PostEvent(EventID.Victory);
        }
    }
}