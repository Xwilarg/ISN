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
        private bool _isRecruited = false;

        public void Interact(PlayerController pc)
        {
            if (_isRecruited) return;

            VNManager.Instance.ShowStory(Info.LobbyStory, this, new Dictionary<string, object>{
                { "firstSpeak", _isFirstConversation }
            });
            _isFirstConversation = false;
        }

        public override void DoDialogueAction(string action)
        {
            switch (action)
            {
                case "recruit":
                    _isRecruited = true;
                    transform.parent = null;
                    break;

                default:
                    throw new System.NotImplementedException();
            }
        }
    }
}