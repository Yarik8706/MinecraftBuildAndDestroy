using Platformer.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Unitilies
{
    public class SpawnObjectRequester : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] GameObject prefab;


        [Header("Validation")]
        [SerializeField] bool isFaildedConfig;

        private void OnValidate()
        {
            isFaildedConfig = prefab == null;
        }


        public void In_SpawnObject()
        {
            if (isFaildedConfig)
            {
                Common.Warning(false, this, "Missing prefab");
                return;
            }
            var instance = Instantiate(prefab).transform;
            instance.position = transform.position;
        }
    }
}
