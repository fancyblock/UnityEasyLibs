using TMPro;
using UnityEngine;

namespace EasyLibs
{
    [RequireComponent(typeof(TextMeshPro))]
    public class LocalizeMeshTextWidget : MonoBehaviour, ILocalizeWidget
    {
        [SerializeField] protected string m_key;
        [SerializeField] protected string m_type;

        private bool m_init = false;

        private TextMeshPro m_text;
        private float m_defaultFontSize;
        private float m_defaultFontSizeMin;
        private float m_defaultFontSizeMax;

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

            if (!string.IsNullOrEmpty(m_key))
            {
                string str = LocalizeMgr.Instance.GetText(m_key);
                m_text.text = str;
            }

            var cfg = LocalizeMgr.Instance.GetTextProperty(m_type);

            if (!string.IsNullOrEmpty(cfg.type))
            {
                m_text.font = cfg.font;

                m_text.fontSize = m_defaultFontSize * cfg.fontSizeRatio;
                m_text.fontSizeMin = m_defaultFontSizeMin * cfg.fontSizeMinRatio;
                m_text.fontSizeMax = m_defaultFontSizeMax * cfg.fontSizeMaxRatio;
            }

            SendMessage("onLanguageUpdated", SendMessageOptions.DontRequireReceiver); // for compatibility with old code, can be removed later
        }


        private void init()
        {
            if (m_init)
                return;

            m_text = GetComponent<TextMeshPro>();

            m_defaultFontSize = m_text.fontSize;
            m_defaultFontSizeMin = m_text.fontSizeMin;
            m_defaultFontSizeMax = m_text.fontSizeMax;

            m_init = true;
        }


#if UNITY_EDITOR

        public void SetType(string type)
        {
            m_type = type;
        }

#endif

    }
}