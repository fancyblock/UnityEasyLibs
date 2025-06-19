using TMPro;
using UnityEngine;

namespace EasyLibs
{
    [RequireComponent(typeof(TMP_InputField))]
    public class LocalizeInputWidget : MonoBehaviour, ILocalizeWidget
    {
        [SerializeField] protected string m_type;

        private TMP_InputField m_inputField;


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

            var cfg = LocalizeMgr.Instance.GetTextProperty(m_type);

            if(!string.IsNullOrEmpty(cfg.type))
            {
                m_inputField.fontAsset = cfg.font;
            }
        }

        private void init()
        {
            if(m_inputField == null)
                m_inputField = GetComponent<TMP_InputField>();
        }
    }
}