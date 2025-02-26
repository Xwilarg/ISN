using ISN.Entity;
using ISN.Manager;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ISN.Character
{
    public class PlayerController : ACharacter
    {
        public static PlayerController Instance { private set; get; }
        private Vector2Int _lookDirection = Vector2Int.up;

        private void Awake()
        {
            Instance = this;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere((Vector2)transform.position + ((Vector2)_lookDirection).normalized * 1.28f, 1.28f / 2f);
        }

        private Vector2Int ToVector2Int(Vector2 v)
        {
            return new Vector2Int(Mathf.CeilToInt(v.x), Mathf.CeilToInt(v.y));
        }

        public void OnMove(InputAction.CallbackContext value)
        {
            if (value.phase == InputActionPhase.Started && !VNManager.Instance.IsPlayingStory)
            {
                var dir = ToVector2Int(value.ReadValue<Vector2>());
                GridManager.Instance.TryMove(CurrentPosition, dir);
                _lookDirection = dir;
            }
        }

        public void OnInteract(InputAction.CallbackContext value)
        {
            if (value.phase == InputActionPhase.Started)
            {
                if (VNManager.Instance.IsPlayingStory)
                {
                    VNManager.Instance.OnNextDialogue();
                }
                else
                {
                    var target = GridManager.Instance.Get(CurrentPosition + _lookDirection);
                    if (target != null && target.GameObject.TryGetComponent<IInteractable>(out var interactible))
                    {
                        interactible.Interact(this);
                    }
                }
            }
        }

        public void OnDialogueSkip(InputAction.CallbackContext value)
        {
            if (VNManager.Instance.IsPlayingStory)
            {
                if (value.phase == InputActionPhase.Started) VNManager.Instance.OnSkip(true);
                else if (value.phase == InputActionPhase.Canceled) VNManager.Instance.OnSkip(true);
            }
        }
    }
}