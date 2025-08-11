using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace EasyLibs
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public class LocalizeDropdownWidget : MonoBehaviour, ILocalizeWidget
    {
        [SerializeField] private List<string> m_valuesKey;

        private TMP_Dropdown m_dropdown;


        void Awake()
        {
            init();
        }

        void OnEnable()
        {
            LocalizeMgr.Instance.AddWidget(this);

            this.OnUpdateLanguage();
        }

        void OnDisable()
        {
            LocalizeMgr.Instance.RemoveWidget(this);
        }

        public void OnUpdateLanguage()
        {
            init();

            int curValue = m_dropdown.value;

            m_dropdown.ClearOptions();

            List<string> options = new List<string>();
            for (int i = 0; i < m_valuesKey.Count; i++)
                options.Add(LocalizeMgr.Instance.GetText(m_valuesKey[i]));

            m_dropdown.AddOptions(options);
            m_dropdown.SetValueWithoutNotify(curValue);
            m_dropdown.RefreshShownValue();
        }

        private void init()
        {
            if (m_dropdown == null)
                m_dropdown = GetComponent<TMP_Dropdown>();
        }
    }
}