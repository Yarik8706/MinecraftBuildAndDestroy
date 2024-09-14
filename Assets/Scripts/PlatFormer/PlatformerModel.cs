using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Platformer.Model
{
    [System.Serializable]
    public class PlatformerModel
    {
        public PlayerController playerPrefab;

        public List<GameObject> levels;
        public List<Color> backGrounds;
    }
}

