using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EasyLibs
{
    public class EasyTabView : MonoBehaviour
    {
        [Serializable]
        public class RelatedObject
        {
            public List<GameObject> m_objects;
        }

        [SerializeField] List<Button> m_tabButtons;
        [SerializeField] List<GameObject> m_tabList;
        [SerializeField] List<GameObject> m_buttonCovers;
        [SerializeField] List<RelatedObject> m_relatedObjects;
        [SerializeField] int m_defaultTabIndex = 0;

        private Action<int, string> m_switchCallback;
        private int m_curPanelIndex = -1;


        void Awake()
        {
            if (m_tabButtons != null)
            {
                for (int i = 0; i < m_tabButtons.Count; i++)
                {
                    int idx = i;
                    m_tabButtons[i].onClick.AddListener(() =>
                    {
                        ShowTab(idx);
                    });
                }
            }
        }

        void Start()
        {
            if (m_defaultTabIndex >= 0)
                ShowTab(m_defaultTabIndex);
        }


        public void ShowTab(int index)
        {
            m_curPanelIndex = index;

            GameObject showedPanel = null;

            for (int i = 0; i < m_tabList.Count; i++)
            {
                m_tabButtons[i].interactable = index != i;

                if (index == i)
                    showedPanel = m_tabList[i];
            }

            bool open = false;

            foreach (GameObject panel in m_tabList)
            {
                if (panel == showedPanel)
                {
                    if (!open)
                    {
                        panel.SetActive(true);
                        panel.SendMessage("OnOpenView", m_tabButtons[index].gameObject.name, SendMessageOptions.DontRequireReceiver);

                        open = true;

                        m_switchCallback?.Invoke(index, panel.gameObject.name);
                    }
                }
                else
                {
                    panel.SetActive(false);
                }
            }

            if(m_buttonCovers != null && m_buttonCovers.Count > 0)
            {
                for (int i = 0; i < m_buttonCovers.Count; i++)
                    m_buttonCovers[i].SetActive(i == index);
            }

            if(m_relatedObjects != null && m_relatedObjects.Count > 0)
            {
                for (int i = 0; i < m_relatedObjects.Count; i++)
                {
                    if (i == index)
                    {
                        foreach (GameObject obj in m_relatedObjects[i].m_objects)
                            obj.SetActive(true);
                    }
                    else
                    {
                        foreach (GameObject obj in m_relatedObjects[i].m_objects)
                            obj.SetActive(false);
                    }
                }
            }
        }

        public void ShowTab(string name)
        {
            int index = -1;

            for (int i = 0; i < m_tabList.Count; i++)
            {
                bool cur = m_tabList[i].gameObject.name == name;

                if(cur)
                {
                    index = i;
                    m_tabList[i].SetActive(true);
                    m_tabList[i].SendMessage("OnOpenView", m_tabButtons[i].gameObject.name, SendMessageOptions.DontRequireReceiver);

                    m_switchCallback?.Invoke(index, name);
                }
                else
                {
                    m_tabList[i].SetActive(false);
                }

                m_tabButtons[i].interactable = !cur;
            }

            if(index >= 0)
            {
                m_curPanelIndex = index;

                if(m_buttonCovers != null && m_buttonCovers.Count > 0)
                {
                    for (int i = 0; i < m_buttonCovers.Count; i++)
                        m_buttonCovers[i].SetActive(i == index);
                }
            }

            if(m_relatedObjects != null && m_relatedObjects.Count > 0)
            {
                for (int i = 0; i < m_relatedObjects.Count; i++)
                {
                    if (i == index)
                    {
                        foreach (GameObject obj in m_relatedObjects[i].m_objects)
                            obj.SetActive(true);
                    }
                    else
                    {
                        foreach (GameObject obj in m_relatedObjects[i].m_objects)
                            obj.SetActive(false);
                    }
                }
            }
        }

        public void ShowTabWithButtonName(string tabBtnName)
        {
            List<string> btnNames = new List<string>();

            for (int i = 0; i < m_tabButtons.Count; i++)
                btnNames.Add(m_tabButtons[i].name);

            int index = btnNames.IndexOf(tabBtnName);

            ShowTab(index);
        }

        public void SetSwitchCallback(Action<int, string> callback)
        {
            m_switchCallback = callback;
        }

        public int CUR_PANEL_INDEX
        {
            get { return m_curPanelIndex; }
        }
    }
}
