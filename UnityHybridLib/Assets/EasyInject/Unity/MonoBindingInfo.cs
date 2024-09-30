using System;
using UnityEngine;

namespace EasyLibs
{
    [Serializable]
    public class GameObjectBindingInfo
    {
        public GameObject m_gameObject;
        public string m_id;
    }

    [Serializable]
    public class MonoBindingInfo
    {
        public MonoBehaviour m_monobehaviour;
        public string m_id;
    }

    [Serializable]
    public class ScriptableObjBindingInfo
    {
        public ScriptableObject m_scriptableObj;
        public string m_id;
    }
}