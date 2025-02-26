using UnityEngine;

namespace ISN.Entity
{
    /// <summary>
    /// Represent something being on the grid
    /// </summary>
    public interface IGridEntity
    {
        public Vector2Int CurrentPosition { set; }
        public GameObject GameObject { get; }
    }
}