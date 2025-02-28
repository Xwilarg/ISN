using ISN.SO;
using NUnit.Framework;
using System.Linq;
using UnityEngine;

namespace ISN.Manager
{
    public class ResourceManager : MonoBehaviour
    {
        public static ResourceManager Instance { private set; get; }

        [SerializeField]
        private MapResourceInfo[] _resources;

        private void Awake()
        {
            Instance = this;
        }

        public T GetMapResource<T>(string key)
            where T : MapResourceInfo
        {
            Assert.True(_resources.Any(x => x.Key == key), $"{key} couldn't be found");
            return (T)_resources.First(x => x.Key == key);
        }
    }
}