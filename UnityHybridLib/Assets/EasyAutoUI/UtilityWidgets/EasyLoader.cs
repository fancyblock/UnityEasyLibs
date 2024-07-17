using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyLibs
{
    public class EasyLoader : MonoBehaviour
    {
        [SerializeField] protected GameObject m_itemPrefab;
        [SerializeField] protected RectTransform m_itemContainer;

        private List<GameObject> m_itemGos = new List<GameObject>();


        public void Load(int count, Action<int, GameObject> setFunc)
        {
            foreach (GameObject go in m_itemGos)
                Destroy(go);

            m_itemGos.Clear();

            for (int i = 0; i < count; i++)
            {
                GameObject go = Instantiate(m_itemPrefab, m_itemContainer);
                go.transform.localScale = Vector3.one;

                setFunc?.Invoke(i, go);
                m_itemGos.Add(go);
            }
        }
    }
}