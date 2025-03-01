using ISN.Character;
using UnityEngine;

namespace ISN.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/AllyInfo", fileName = "AllyInfo")]
    public class AllyInfo : CharacterInfo
    {
        [Header("Dialogues")]
        public TextAsset LobbyStory;

        public override void InitSelf(GameObject target)
        {
            target.GetComponent<AllyController>().Info = this;
        }
    }
}