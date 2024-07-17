using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace EasyLibs
{
    public class AutoWidget : MonoBehaviour, IAutoWidget
    {
        public AutoContainer m_parent;

        private Button m_button = null;
        private Action m_buttonEvent;
        private Action<string> m_buttonEventWithParam;
        private bool m_buttonInited = false;

        private EventTrigger m_trigger = null;
        private Action m_triggerEvent;
        private bool m_triggerInited = false;

        private InputField m_inputField = null;
        private Action<string> m_inputFieldChangeEvent;

        private TMP_InputField m_tmpInputField = null;
        //TODO 

        private Dropdown m_dropDown = null;
        private Action<bool> m_toggleChangeEvent;

        private Action<string> m_buttonClkHook;

        // 缓存引用
        private AutoContainer m_container;
        private Text m_label = null;
        private TextMeshProUGUI m_labelTMP = null;
        private Toggle m_toggle = null;
        private ToggleGroup m_toggleGroup = null;
        private Image m_image = null;
        private RawImage m_rawImage = null;
        private Animator m_animator = null;
        private Slider m_slider = null;
        private RectTransform m_transform = null;

        private bool m_eventEnable = true;


        void Awake()
        {
            m_container = GetComponent<AutoContainer>();
        }

        /// <summary>
        /// 是否是虚拟widget
        /// </summary>
        /// <returns></returns>
        public bool IsVirtual()
        {
            return false;
        }

        /// <summary>
        /// Widget所属的GameObject打开或关闭
        /// </summary>
        /// <param name="enable"></param>
        public void SetEnable(bool enable)
        {
            gameObject.SetActive(enable);
        }

        /// <summary>
        /// 得到所属特定类型Component
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Component<T>()
        {
            return gameObject.GetComponent<T>();
        }

        /// <summary>
        /// 设置Text文本
        /// </summary>
        /// <param name="text"></param>
        public void SetValue(string text)
        {
            if (m_label == null)
                m_label = GetComponent<Text>();

            if (m_label == null)
                m_labelTMP = GetComponent<TextMeshProUGUI>();

            if (m_label != null)
                m_label.text = text;
            else
                m_labelTMP.text = text;
        }

        /// <summary>
        /// 获取Text文本
        /// </summary>
        /// <returns></returns>
        public string GetValue()
        {
            if (m_label == null)
                m_label = GetComponent<Text>();

            if (m_label == null)
                m_labelTMP = GetComponent<TextMeshProUGUI>();

            if (m_labelTMP == null)
                m_inputField = GetComponent<InputField>();

            if (m_labelTMP == null)
                m_tmpInputField = GetComponent<TMP_InputField>();

            if (m_label != null)
                return m_label.text;
            else if (m_labelTMP != null)
                return m_labelTMP.text;
            else if (m_inputField != null)
                return m_inputField.text;
            else if (m_tmpInputField != null)
                return m_tmpInputField.text;

            return "";
        }

        public void SetValue(bool toggle)
        {
            if (m_toggle == null)
                m_toggle = GetComponent<Toggle>();

            m_toggle.isOn = toggle;
        }

        public void SetValue(float progress)
        {
            if (m_slider == null)
                m_slider = GetComponent<Slider>();

            m_slider.value = progress;
        }

        /// <summary>
        /// 设置按钮是否可用
        /// </summary>
        /// <param name="enable"></param>
        public void SetButtonEnable(bool enable)
        {
            if (m_button == null)
                m_button = GetComponent<Button>();

            m_button.interactable = enable;
        }

        /// <summary>
        /// 设置按钮回调事件
        /// </summary>
        /// <param name="evt"></param>
        public void SetButtonEvent(Action evt)
        {
            m_buttonEvent = evt;

            if (!m_buttonInited)
            {
                if (m_button == null)
                    m_button = GetComponent<Button>();

                m_button.onClick.AddListener(onButtonCallback);

                m_buttonInited = true;
            }
        }

        /// <summary>
        /// 设置按钮回调事件
        /// </summary>
        /// <param name="evt"></param>
        public void SetButtonEvent(Action<string> evt)
        {
            m_buttonEventWithParam = evt;

            if (!m_buttonInited)
            {
                if (m_button == null)
                    m_button = GetComponent<Button>();

                m_button.onClick.AddListener(onButtonCallback);

                m_buttonInited = true;
            }
        }

        /// <summary>
        /// 设置trigger事件
        /// </summary>
        /// <param name="evt"></param>
        public void SetTriggerEvent(Action evt)
        {
            m_triggerEvent = evt;

            if (!m_triggerInited)
            {
                if (m_trigger == null)
                    m_trigger = GetComponent<EventTrigger>();

                List<EventTrigger.Entry> triggers = m_trigger.triggers;

                if (triggers == null)
                    triggers = new List<EventTrigger.Entry>();

                EventTrigger.Entry en = new EventTrigger.Entry();
                en.eventID = EventTriggerType.PointerClick;
                en.callback = new EventTrigger.TriggerEvent();
                en.callback.AddListener(onTriggerCallback);
                triggers.Add(en);

                m_trigger.triggers = triggers;

                m_triggerInited = true;
            }
        }

        private void onTriggerCallback(BaseEventData art)
        {
            if (m_triggerEvent != null)
                m_triggerEvent();
        }

        /// <summary>
        /// 按钮点击事件回调 
        /// </summary>
        private void onButtonCallback()
        {
            if (m_eventEnable)
            {
                int stamp = Time.frameCount;

                if (stamp == m_parent.LAST_BTN_FRAME)
                    return;

                if (m_buttonClkHook != null)
                    m_buttonClkHook(gameObject.name);

                if (m_buttonEvent != null)
                {
                    m_buttonEvent();
                    m_parent.LAST_BTN_FRAME = stamp;
                }

                if (m_buttonEventWithParam != null)
                {
                    m_buttonEventWithParam(gameObject.name);
                    m_parent.LAST_BTN_FRAME = stamp;
                }
            }
        }

        /// <summary>
        /// 按钮点击钩子
        /// </summary>
        public void SetButtonClkHook(Action<string> hook)
        {
            m_buttonClkHook = hook;
        }

        public void SetImageFillAmount(float value)
        {
            if (m_image == null)
                m_image = GetComponent<Image>();

            m_image.fillAmount = value;
        }

        public float GetImageFillAmount()
        {
            if (m_image == null)
                m_image = GetComponent<Image>();

            return m_image.fillAmount;
        }

        /// <summary>
        /// 是否接收事件 
        /// </summary>
        /// <param name="enable"></param>
        public void SetEventEnable(bool enable)
        {
            m_eventEnable = enable;
        }

        public void PlayAnimation(string ani)
        {
            if (m_animator == null)
                m_animator = GetComponent<Animator>();

            m_animator.Play(ani);
        }

        public void SetTextColor(Color color)
        {
            if (m_label == null)
                m_label = GetComponent<Text>();

            m_label.color = color;
        }

        public void SetMaskWidth(float width)
        {
            if (m_transform == null)
                m_transform = GetComponent<RectTransform>();
            m_transform.sizeDelta = new Vector2(width, m_transform.sizeDelta.y);
        }

        public void SetValue(Sprite sprite)
        {
            if (m_image == null)
                m_image = GetComponent<Image>();

            m_image.sprite = sprite;
            m_image.SetNativeSize();
        }

        public void SetInputFieldChangeEvent(Action<string> onFieldChange)
        {
            if (m_inputField == null)
            {
                m_inputField = GetComponent<InputField>();
            }

            if (m_inputField == null && m_tmpInputField == null)
                m_tmpInputField = GetComponent<TMP_InputField>();

            if (m_inputField != null)
            {
                m_inputField.onEndEdit.RemoveAllListeners();
                m_inputField.onEndEdit.AddListener(onInputFieldChange);
            }
            else if (m_tmpInputField != null)
            {
                m_tmpInputField.onEndEdit.RemoveAllListeners();
                m_tmpInputField.onEndEdit.AddListener(onInputFieldChange);
            }

            m_inputFieldChangeEvent = onFieldChange;
        }

        protected void onInputFieldChange(string text)
        {
            if (m_eventEnable && m_inputFieldChangeEvent != null)
                m_inputFieldChangeEvent(text);
        }

        public bool GetToggleValue()
        {
            if (m_toggle == null)
                m_toggle = GetComponent<Toggle>();

            return m_toggle.isOn;
        }

        public void SetToggleValue(bool isOn)
        {
            if (m_toggle == null)
                m_toggle = GetComponent<Toggle>();

            m_toggle.isOn = isOn;
        }

        public void SetToggleChangeEvent(Action<bool> onToggleChange)
        {
            if (m_toggle == null)
            {
                m_toggle = GetComponent<Toggle>();
            }

            m_toggle.onValueChanged.RemoveAllListeners();
            m_toggle.onValueChanged.AddListener(onToggleValueChanged);

            m_toggleChangeEvent = onToggleChange;
        }

        protected void onToggleValueChanged(bool toggled)
        {
            if (m_eventEnable && m_toggleChangeEvent != null)
                m_toggleChangeEvent(toggled);
        }

        public void SetToggleGroupEvent(Action<bool, string> onToggled)
        {
            if (m_toggleGroup == null)
            {
                m_toggleGroup = GetComponent<ToggleGroup>();
            }

            //TODO 
        }

        public void SetImageColor(Color color)
        {
            if (m_image == null)
                m_image = GetComponent<Image>();

            m_image.color = color;
        }

        public void SetRawImage(Texture2D texture)
        {
            if (m_rawImage == null)
                m_rawImage = GetComponent<RawImage>();

            m_rawImage.texture = texture;
        }

        public void SetDropListItems(List<string> items)
        {
            if (m_dropDown == null)
                m_dropDown = GetComponent<Dropdown>();

            m_dropDown.ClearOptions();
            m_dropDown.AddOptions(items);
        }

        public void SetInputFieldVal(string val)
        {
            if (m_inputField == null)
                m_inputField = GetComponent<InputField>();

            if (m_inputField == null && m_labelTMP == null)
                m_tmpInputField = GetComponent<TMP_InputField>();

            if (m_inputField != null)
                m_inputField.text = val;
            else if (m_tmpInputField != null)
                m_tmpInputField.text = val;
        }

        public string GetInputFieldVal()
        {
            if (m_inputField == null)
                m_inputField = GetComponent<InputField>();

            if (m_inputField != null)
                return m_inputField.text;

            if (m_tmpInputField == null)
                m_tmpInputField = GetComponent<TMP_InputField>();

            return m_tmpInputField.text;
        }

        public Color GetTextColor()
        {
            if (m_label == null)
                m_label = GetComponent<Text>();

            return m_label.color;
        }

        public GameObject GetGameObj()
        {
            return gameObject;
        }

        public void SetRectSize(Vector2 size)
        {
            var trans = GetComponent<RectTransform>();
            trans.sizeDelta = size;
        }

        public void ScrollLoad(int count, Action<int, GameObject> setFunc)
        {
            EasyScrollView scrollView = GetComponent<EasyScrollView>();
            scrollView.Load(count, setFunc);
        }

        public void SnapTo(int index)
        {
            EasyScrollView scrollView = GetComponent<EasyScrollView>();
            scrollView.SnapTo(index);
        }

        public void ShowTab(int index)
        {
            EasyTabView tabView = GetComponent<EasyTabView>();
            tabView.ShowTab(index);
        }

        public void ShowTab(string name)
        {
            EasyTabView tabView = GetComponent<EasyTabView>();
            tabView.ShowTab(name);
        }
    }
}