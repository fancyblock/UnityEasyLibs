using System.Collections.Generic;
using UnityEngine;

namespace EasyLibs
{
    public class MonoInject : MonoBehaviour
    {
        [SerializeField] private bool m_injectChildren = true;


        void Start()
        {
            MonoBehaviour[] mbs = gameObject.GetComponents<MonoBehaviour>();

            foreach (var mb in mbs)
            {
                if(mb != this)
                    InjectContext.Instance.Inject(mb);
            }

            if(m_injectChildren)
            {
                Queue<Transform> queue = new Queue<Transform>();
                queue.Enqueue(transform);

                while (queue.Count > 0)
                {
                    Transform t = queue.Dequeue();

                    foreach (Transform child in t)
                    {
                        MonoBehaviour[] cmbs = child.GetComponents<MonoBehaviour>();

                        foreach (var cmb in cmbs)
                            InjectContext.Instance.Inject(cmb);

                        queue.Enqueue(child);
                    }
                }
            }
        }
    }
}