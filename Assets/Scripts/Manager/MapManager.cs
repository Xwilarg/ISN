using ISN.Character;
using Unity.Cinemachine;
using UnityEngine;

namespace ISN.Manager
{
    /// <summary>
    /// Manage maps
    /// </summary>
    public class MapManager : MonoBehaviour
    {
        [SerializeField]
        private TextAsset _lobby;

        [SerializeField]
        private GameObject _playerPrefab;

        [SerializeField]
        private CinemachineCamera _cc;

        private void Start()
        {
            var p = Instantiate(_playerPrefab);
            PartyManager.Instance.AddToParty(p.GetComponent<ACharacter>());
            _cc.Target.TrackingTarget = p.transform;

            GridManager.Instance.LoadMap(_lobby);
        }
    }
}