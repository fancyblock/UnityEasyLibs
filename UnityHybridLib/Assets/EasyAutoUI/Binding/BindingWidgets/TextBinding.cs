using UnityEngine;
using UnityEngine.UI;

namespace EasyLibs
{
    [RequireComponent(typeof(Text))]
    public class TextBinding : BaseBindingWidget
    {
        private Text m_text;


        void Awake()
        {
            m_text = GetComponent<Text>();
        }

        protected override void onDataChange(object val)
        {
            m_text.text = val.ToString();
        }
    }
}