using Platformer.Observer;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Unitilies
{
    public class AutoDestroy : MonoBehaviour
    {
        [Header("Config")]
        [Range(0,10f)] [SerializeField] float lifeTime;



        [Header("Events")]
        [SerializeField] UnityEvent OnTimeUpAndDestroyHandler;

        #region Private
        float _count;
        void Start()
        {
            _count = lifeTime;
        }


        private void Update()
        {
            _count -= Time.deltaTime;
            if (_count <= 0)
            {
                Destroy(gameObject);
                OnTimeUpAndDestroyHandler.Invoke();
            }
        }
        #endregion // Private

    }
}
