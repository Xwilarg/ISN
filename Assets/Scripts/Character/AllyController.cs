using ISN.Entity;
using ISN.Manager;
using ISN.SO;
using System.Collections.Generic;

namespace ISN.Character
{
    public class AllyController : ACharacter, IInteractable
    {
        public AllyInfo Info { set; get; }

        private bool _isFirstConversation = true;

        public void Interact(PlayerController pc)
        {
            VNManager.Instance.ShowStory(Info.LobbyStory, new Dictionary<string, object>{
                { "firstSpeak", _isFirstConversation }
            });
            _isFirstConversation = false;
        }
    }
}