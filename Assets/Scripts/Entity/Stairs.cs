using ISN.Character;
using UnityEngine;

namespace ISN.Entity
{
    public class Stairs : MonoBehaviour, IGridEntity, IWalkable
    {
        public Vector2Int CurrentPosition { set; private get; }

        public GameObject GameObject => gameObject;

        public void WalkOn(PlayerController pc)
        {
            throw new System.NotImplementedException();
        }
    }
}