using UnityEngine;

namespace ISN.Manager
{
    public class ResourceManager : MonoBehaviour
    {
        public static ResourceManager Instance { private set; get; }

        private void Awake()
        {
            Instance = this;
        }
    }
}