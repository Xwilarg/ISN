using UnityEngine;

namespace ISN.Manager
{
    public class ResourceManager : MonoBehaviour
    {
        public static ResourceManager Instance { private set; get; }

        [SerializeField]
        private GameObject _allyPrefab;
        public GameObject AllyPrefab => _allyPrefab;

        private void Awake()
        {
            Instance = this;
        }
    }
}