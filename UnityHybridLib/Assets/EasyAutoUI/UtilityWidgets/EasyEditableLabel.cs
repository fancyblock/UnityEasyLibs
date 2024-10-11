using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace EasyLibs
{
    public class EasyEditableLabel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_text;
        [SerializeField] private TMP_InputField m_inputField;

        private Action<string> m_changeCallback;


        void Awake()
        {
            m_text.enabled = true;
            m_inputField.gameObject.SetActive(false);

            m_inputField.onEndEdit.AddListener(onEndEdit);
        }

        void Start()
        {
        }

        public void ActiveEdit()
        {
            m_text.enabled = false;
            m_inputField.gameObject.SetActive(true);

            m_inputField.text = m_text.text;

            m_inputField.ActivateInputField();
        }

        public string TEXT
        {
            get
            {
                return m_text.text;
            }
            set
            {
                m_text.text = value;
            }
        }

        public void SetChangeCallback(Action<string> callback)
        {
            m_changeCallback = callback;
        }


        private void onEndEdit(string str)
        {
            m_text.enabled = true;
            m_inputField.gameObject.SetActive(false);

            m_text.text = str;

            // invoke callback
            m_changeCallback?.Invoke(str);
        }


#if UNITY_EDITOR

        [ContextMenu("Sync Text Format")]
        private void OnSyncTextFormat()
        {
            if (m_text == null)
                m_text = GetComponent<TextMeshProUGUI>();
            if(m_inputField == null)
                m_inputField = GetComponentInChildren<TMP_InputField>();

            m_inputField.targetGraphic.enabled = false;

            m_inputField.fontAsset = m_text.font;
            m_inputField.pointSize = m_text.fontSize;

            m_inputField.textComponent.fontSize = m_text.fontSize;
            m_inputField.textComponent.color = m_text.color;
            m_inputField.textComponent.alignment = m_text.alignment;

            // hide placeholder
            m_inputField.placeholder.GetComponent<TextMeshProUGUI>().text = "";

            RectTransform rectTrans = m_inputField.transform as RectTransform;
            rectTrans.anchorMin = new Vector2(0, 0);
            rectTrans.anchorMax = new Vector2(1, 1);
            rectTrans.offsetMin = Vector2.zero;
            rectTrans.offsetMax = Vector2.zero;
        }

#endif
    }
}
