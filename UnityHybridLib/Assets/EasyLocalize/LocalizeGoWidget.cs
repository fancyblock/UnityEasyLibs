using System;
using UnityEngine;

namespace EasyLibs
{
    public class LocalizeGoWidget : MonoBehaviour, ILocalizeWidget
    {
        [SerializeField] private CLocalizeGo[] m_localizeGos;


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
            foreach(var i in m_localizeGos)
            {
                i.go.SetActive(i.language == LocalizeMgr.Instance.CURRENT_LANGUAGE_INDEX);
            }
        }


        [Serializable]
        public struct CLocalizeGo
        {
            public int language;
            public GameObject go;
        }
    }
}
