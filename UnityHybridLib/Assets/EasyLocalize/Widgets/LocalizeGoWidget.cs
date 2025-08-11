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
            string lang = LocalizeMgr.Instance.CURRENT_LANGUAGE;

            foreach(var i in m_localizeGos)
                i.go.SetActive(i.language == lang);

            SendMessage("onLanguageUpdated", SendMessageOptions.DontRequireReceiver);
        }


        [Serializable]
        public struct CLocalizeGo
        {
            public string language;
            public GameObject go;
        }
    }
}
