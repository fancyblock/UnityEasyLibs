using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EasyTabView : MonoBehaviour
{
    [SerializeField] List<Button> m_tabButtons;
    [SerializeField] List<GameObject> m_tabList;
    [SerializeField] int m_defaultTabIndex = 0;


    void Awake()
    {
        if(m_tabButtons != null)
        {
            for(int i = 0; i < m_tabButtons.Count; i++)
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
        if(m_defaultTabIndex >= 0)
            ShowTab(m_defaultTabIndex);
    }

    
    public void ShowTab(int index)
    {
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
                }
            }
            else
            {
                panel.SetActive(false);
            }
        }
    }

    public void ShowTab(string name)
    {
        for (int i = 0; i < m_tabList.Count; i++)
        {
            m_tabList[i].SetActive(m_tabList[i].gameObject.name == name);
            m_tabButtons[i].interactable = m_tabList[i].gameObject.name != name;

            if (m_tabList[i].gameObject.name == name)
                m_tabList[i].SendMessage("OnOpenView", m_tabButtons[i].gameObject.name, SendMessageOptions.DontRequireReceiver);
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

}
