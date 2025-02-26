using ISN.Entity;
using UnityEngine;

namespace ISN.Character
{
    public class AllyController : ACharacter, IInteractable
    {
        public void Interact(PlayerController pc)
        {
            Debug.Log("Interaction");
        }
    }
}