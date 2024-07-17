using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EasyLibs
{
    public class MoreEasyScrollView : MonoBehaviour
    {
        [SerializeField] protected GameObject m_itemPrefab;

        [SerializeField] protected ScrollRect m_scrollRect;
        [SerializeField] protected RectTransform m_itemContainer;

        private Action<int, GameObject> m_setFunc;
        private List<GameObject> m_itemGOs = new List<GameObject>();


        public void Load(int count, Action<int, GameObject> setFunc)
        {
            unload();

            m_setFunc = setFunc;

            for (int i = 0; i < count; i++)
            {
                GameObject go = Instantiate(m_itemPrefab, m_itemContainer);
                go.transform.localScale = Vector3.one;
                go.SetActive(true);

                m_setFunc?.Invoke(i, go);

                m_itemGOs.Add(go);
            }
        }

        public void SnapTo(int index)
        {
            //TODO 
        }

        public List<GameObject> ITEMS
        {
            get { return m_itemGOs; }
        }


        private void unload()
        {
            foreach (GameObject go in m_itemGOs)
                Destroy(go);

            m_itemGOs.Clear();
        }
    }
}