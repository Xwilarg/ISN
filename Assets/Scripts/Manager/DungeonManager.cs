using UnityEngine;

namespace ISN.Manager
{
    /// <summary>
    /// Manage a dungeon spawn
    /// </summary>
    public class DungeonManager : MonoBehaviour
    {
        public static DungeonManager Instance { private set; get; }

        private void Awake()
        {
            Instance = this;
        }

        public void GoToDungeon()
        {
            foreach (var charac in PartyManager.Instance.Party)
            {
                
            }
        }
    }
}