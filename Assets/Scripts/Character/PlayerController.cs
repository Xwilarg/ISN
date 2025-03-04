using ISN.Entity;
using ISN.Manager;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ISN.Character
{
    public class PlayerController : ACharacter
    {
        private Vector2Int _lookDirection = Vector2Int.up;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere((Vector2)transform.position + ((Vector2)_lookDirection).normalized * 1.28f, 1.28f / 2f);
        }

        private Vector2Int ToVector2Int(Vector2 v)
        {
            int x = 0;
            int y = 0;
            if (v.x > 0f) x = 1;
            else if (v.x < 0f) x = -1;
            if (v.y > 0f) y = 1;
            else if (v.y < 0f) y = -1;
            return new Vector2Int(x, y);
        }

        public void OnMove(InputAction.CallbackContext value)
        {
            if (value.phase == InputActionPhase.Started && !VNManager.Instance.IsPlayingStory)
            {
                var dir = ToVector2Int(value.ReadValue<Vector2>());
                GridManager.Instance.TryMove(CurrentPosition, dir, out var walkable);
                if (walkable != null)
                {
                    walkable.WalkOn(this);
                }
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