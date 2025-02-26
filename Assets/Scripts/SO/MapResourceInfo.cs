using UnityEngine;

namespace SIN.SO
{
    public abstract class MapResourceInfo : ScriptableObject
    {
        public string Key;
        public GameObject Prefab;

        public abstract void InitSelf(GameObject target);
    }
}