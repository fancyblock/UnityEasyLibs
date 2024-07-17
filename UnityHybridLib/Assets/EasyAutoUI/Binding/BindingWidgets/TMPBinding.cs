using TMPro;
using UnityEngine;

namespace EasyLibs
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TMPBinding : BaseBindingWidget
    {
        private TextMeshProUGUI m_text;


        void Awake()
        {
            init();
        }

        protected override void onDataChange(object val)
        {
            init();
            m_text.text = val.ToString();
        }


        private void init()
        {
            if (m_text == null)
                m_text = GetComponent<TextMeshProUGUI>();
        }
    }
}