using System.Collections.Generic;
using UnityEngine;

namespace EasyLibs
{
    public class GameObjectPool : MonoBehaviour
    {
        [SerializeField] private GameObject m_prefab;
        [SerializeField] private List<GameObject> m_preCacheObjects = new List<GameObject>();

        private readonly Stack<GameObject> m_availableObjects = new Stack<GameObject>();
        private readonly HashSet<GameObject> m_inUseObjects = new HashSet<GameObject>();


        public GameObject GetObject()
        {
            GameObject go = null;

            if(m_availableObjects.Count > 0)
            {
                go = m_availableObjects.Pop();
            }
            else
            {
                if(m_preCacheObjects.Count > 0)
                {
                    go = m_preCacheObjects[0];
                    m_preCacheObjects.RemoveAt(0);
                }
                else
                {
                    go = Instantiate(m_prefab);
                }
            }

            m_inUseObjects.Add(go);
            go.SetActive(true);

            return go;
        }

        public void Release(GameObject go)
        {
            if(m_inUseObjects.Remove(go))
            {
                go.SetActive(false);
                m_availableObjects.Push(go);

                go.transform.SetParent(transform);
            }
        }
    }
}
