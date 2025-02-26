using ISN.Manager;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ISN.Character
{
    public class PlayerController : ACharacter
    {
        public static PlayerController Instance { private set; get; }

        private void Awake()
        {
            Instance = this;
        }

        private Vector2Int ToVector2Int(Vector2 v)
        {
            return new Vector2Int(Mathf.CeilToInt(v.x), Mathf.CeilToInt(v.y));
        }

        public void OnMove(InputAction.CallbackContext value)
        {
            if (value.phase == InputActionPhase.Started)
            {
                var dir = ToVector2Int(value.ReadValue<Vector2>());
                GridManager.Instance.TryMove(CurrentPosition, dir);
            }
        }
    }
}