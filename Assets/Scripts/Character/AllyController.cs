using ISN.Entity;
using ISN.Manager;
using ISN.SO;
using UnityEngine;

namespace ISN.Character
{
    public class AllyController : ACharacter, IInteractable
    {
        public AllyInfo Info { set; get; }

        public void Interact(PlayerController pc)
        {
            VNManager.Instance.ShowStory(Info.LobbyStory);
        }
    }
}