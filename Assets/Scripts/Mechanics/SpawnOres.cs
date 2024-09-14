using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Platformer.Mechanics
{
    public class SpawnOres : MonoBehaviour
    {
        [SerializeField] private GameObject redstone;
        [SerializeField] private GameObject coal;
        [SerializeField] private GameObject iron;
        [SerializeField] private GameObject gold;
        [SerializeField] private GameObject diamond;
        [SerializeField] private GameObject emerald;
        [SerializeField] private GameObject lapis;

        [SerializeField] private List<GameObject> replaceableObjects;

        private void Start()
        {
            SpawnOre(3, diamond);
            SpawnOre(5, redstone);
            SpawnOre(7, coal);
            SpawnOre(5, iron);
            SpawnOre(3, gold);
            SpawnOre(1, emerald);
            SpawnOre(2, lapis);
        }

        public void SpawnOre(int count, GameObject ore)
        {
            for (int i = 0; i < count; i++)
            {
                var index = Random.Range(0, replaceableObjects.Count);
                Instantiate(ore, 
                    replaceableObjects[index].transform.position+new Vector3(0.5f, -0.5f, -0.5f), 
                    Quaternion.identity);
                Destroy(replaceableObjects[index]);
            }
        }
    }
}