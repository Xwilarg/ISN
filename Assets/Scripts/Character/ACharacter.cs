using ISN.Entity;
using UnityEngine;

namespace ISN.Character
{
    public abstract class ACharacter : MonoBehaviour, IDamageableEntity, IGridEntity
    {
        private int _health = 100;

        public void TakeDamage(int amount)
        {
            _health -= amount;
        }
        public Vector2Int CurrentPosition { set; protected get; }

        public GameObject GameObject => gameObject;
    }
}