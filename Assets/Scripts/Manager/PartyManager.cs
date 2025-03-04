using ISN.Character;
using System.Collections.Generic;
using UnityEngine;

namespace ISN.Manager
{
    public class PartyManager : MonoBehaviour
    {
        public static PartyManager Instance { private set; get; }

        private readonly List<ACharacter> _party = new();
        public IEnumerable<ACharacter> Party => _party;

        private void Awake()
        {
            Instance = this;
        }

        public void AddToParty(ACharacter character)
        {
            _party.Add(character);
        }
    }
}