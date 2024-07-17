using System.Collections.Generic;
using UnityEngine;

namespace EasyLibs
{
    public class MonoBinding : MonoBehaviour
    {
        [SerializeField] private List<MonoBindingInfo> m_bindings;


        // binding
        void Awake()
        {
            foreach(var i in m_bindings)
            {
                BindingData bd = InjectContext.Instance.Bind(i.m_monobehaviour.GetType(), i.m_monobehaviour);

                if (!string.IsNullOrEmpty(i.m_id))
                    bd.WithId(i.m_id);
            }
        }

        // unbinding
        void OnDestroy()
        {
            foreach(var i in m_bindings)
            {
                InjectContext.Instance.UnbindingData(i.m_monobehaviour.GetType(), i.m_monobehaviour);
            }
        }
    }
}
