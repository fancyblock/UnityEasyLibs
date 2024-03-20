using UnityEngine;


namespace EasyInject
{
    public class MonoInject : MonoBehaviour
    {
        [SerializeField] private bool m_injectChildren = true;


        void Start()
        {
            MonoBehaviour[] mbs = gameObject.GetComponents<MonoBehaviour>();

            foreach (var mb in mbs)
                InjectContext.Instance.Inject(mb);

            if(m_injectChildren)
            {
                var cmbs = gameObject.GetComponentsInChildren<MonoBehaviour>();

                foreach (var cmb in cmbs)
                    InjectContext.Instance.Inject(cmb);
            }
        }
    }
}
