using System.Collections.Generic;
using UnityEngine;

namespace Mechanics
{
    public enum BlockType
    {
        DoNotSpendAction,
        DoReturnBuildAction
    }
    
    public class BuildingBlock : MonoBehaviour
    {
        public List<BlockType> blockTypes;
    }
}