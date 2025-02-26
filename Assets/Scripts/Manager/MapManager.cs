using UnityEngine;

namespace ISN.Manager
{
    public class MapManager : MonoBehaviour
    {
        [SerializeField]
        private TextAsset _lobby;

        private void Start()
        {
            GridManager.Instance.LoadMap(_lobby);
        }
    }
}