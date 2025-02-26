using ISN.Entity;
using UnityEngine;

namespace ISN.Player
{
    public class PlayerController : MonoBehaviour, IGridEntity
    {
        public static PlayerController Instance { private set; get; }

        private void Awake()
        {
            Instance = this;
        }
    }
}