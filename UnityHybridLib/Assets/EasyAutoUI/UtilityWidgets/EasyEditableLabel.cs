using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

namespace EasyLibs
{
    public class EasyEditableLabel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_text;
        [SerializeField] private TMP_InputField m_inputField;
        [SerializeField] private GameObject m_noEditGo;
        [SerializeField] private Button m_btnEdit;
        [SerializeField] private int m_maxLength = 100;

        private Action<string> m_textUpdateCallback;
        private Action<string> m_changeCallback;
        private Action<string> m_submitCallback;
        private Action<string> m_pasteCallback;
        private bool m_isPasting = false;


        void Awake()
        {
            m_text.enabled = true;
            if(m_noEditGo != null)
                m_noEditGo.SetActive(true);
            m_inputField.gameObject.SetActive(false);

            if(m_btnEdit != null)
                m_btnEdit.onClick.AddListener(ActiveEdit);
            
            m_inputField.onValueChanged.AddListener(onValueChange);
            m_inputField.onEndEdit.AddListener(onEndEdit);
            m_inputField.onSubmit.AddListener(onSubmit);
        }

        public void ActiveEdit()
        {
            m_text.enabled = false;
            if (m_noEditGo != null)
                m_noEditGo?.SetActive(false);
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

        public void SetTextUpdateCallback(Action<string> callback)
        {
            m_textUpdateCallback = callback;
        }

        public void SetChangeCallback(Action<string> callback)
        {
            m_changeCallback = callback;
        }

        public void SetSubmitCallback(Action<string> callback)
        {
            m_submitCallback = callback;
        }

        public void SetPasteCallback(Action<string> callback)
        {
            m_pasteCallback = callback;
        }


        private void onValueChange(string str)
        {
            Util.Log($"EasyEditableLabel: onValueChange");

            if(str.Length > m_maxLength)
            {
                str = str.Substring(0, m_maxLength);
                m_inputField.text = str;

                return;
            }

            if (!m_isPasting && Input.GetKey(KeyCode.V) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
            {
                m_isPasting = true;
                StartCoroutine(handlePaste(str));
            }
            else
            {
                m_textUpdateCallback?.Invoke(str);
            }
        }

        private IEnumerator handlePaste(string str)
        {
            yield return new WaitForEndOfFrame();

            m_isPasting = false;
            
            m_pasteCallback?.Invoke(GUIUtility.systemCopyBuffer);

            m_textUpdateCallback?.Invoke(str);
        }

        private void onEndEdit(string str)
        {
            Util.Log($"EasyEditableLabel: onEditEnd");

            m_text.enabled = true;
            if (m_noEditGo != null)
                m_noEditGo?.SetActive(true);
            m_inputField.gameObject.SetActive(false);

            m_text.text = str;

            // invoke callback
            m_changeCallback?.Invoke(str);
        }

        private void onSubmit(string str)
        {
            m_submitCallback?.Invoke(str);
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