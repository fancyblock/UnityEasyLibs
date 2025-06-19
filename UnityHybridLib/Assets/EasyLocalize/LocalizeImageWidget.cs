using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EasyLibs
{
    [RequireComponent(typeof(Image))]
    public class LocalizeImageWidget : MonoBehaviour, ILocalizeWidget
    {
        [SerializeField] protected string m_key;

        private Image m_image = null;


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

            int langIndex = LocalizeMgr.Instance.CURRENT_LANGUAGE_INDEX;
            SLocalizeImage cfg = LocalizeMgr.Instance.GetLocalizeImage(m_key);

            m_image.sprite = cfg.sprites[langIndex];
            m_image.SetNativeSize();
        }


        private void init()
        {
            if (m_image == null)
                m_image = GetComponent<Image>();
        }
    }
}
