using ISN.Entity;
using UnityEngine;

namespace ISN.Character
{
    public abstract class ACharacter : MonoBehaviour, IDamageableEntity
    {
        private int _health = 100;

        public void TakeDamage(int amount)
        {
            _health -= amount;
        }
    }
}