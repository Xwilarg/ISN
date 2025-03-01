using UnityEngine;

namespace ISN.SO
{
    public abstract class MapResourceInfo : ScriptableObject
    {
        [Header("Generic data")]
        public string Key;
        public GameObject Prefab;

        public abstract void InitSelf(GameObject target);
    }
}