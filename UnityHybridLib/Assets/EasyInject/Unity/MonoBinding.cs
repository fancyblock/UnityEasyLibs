using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace EasyLibs
{
    public class MonoBinding : MonoBehaviour
    {
        [SerializeField] private GameObjectBindingInfo m_goBinding;
        [SerializeField] private List<MonoBindingInfo> m_bindings;


        // binding
        void Awake()
        {
            if(m_goBinding != null && m_goBinding.m_gameObject != null)
            {
                Assert.IsFalse(string.IsNullOrEmpty(m_goBinding.m_id), "GameObject binding must have id.");

                InjectContext.Instance.Bind(typeof(GameObject), gameObject).WithId(m_goBinding.m_id);
            }

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
            if(m_goBinding != null && m_goBinding.m_gameObject != null)
            {
                //TODO 
            }

            foreach(var i in m_bindings)
            {
                InjectContext.Instance.UnbindingData(i.m_monobehaviour.GetType(), i.m_monobehaviour);
            }
        }
    }
}
