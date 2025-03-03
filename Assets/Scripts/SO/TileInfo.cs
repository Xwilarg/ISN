using UnityEngine;

namespace ISN.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/TileInfo", fileName = "TileInfo")]
    public class TileInfo : ScriptableObject
    {
        public char Character;
        public Sprite Sprite;

        public bool IsWalkable;
    }
}