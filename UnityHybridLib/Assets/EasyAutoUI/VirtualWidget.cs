using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyLibs
{
    public class VirtualWidget : IAutoWidget
    {
        protected static VirtualWidget m_instance = null;

        /// <summary>
        /// return the singleton 
        /// </summary>
        static public VirtualWidget Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new VirtualWidget();

                return m_instance;
            }
        }

        public bool IsVirtual() { return true; }

        public T Component<T>() { return default(T); }

        public void SetEnable(bool enable) { }

        public void SetValue(string text) { }

        public void SetValue(bool toggle) { }

        public void SetValue(float progress) { }

        public string GetValue() { return ""; }

        public void SetButtonEvent(Action evt) { }

        public void SetButtonEvent(Action<string> evt) { }

        public void SetTriggerEvent(Action evt) { }

        public void SetButtonEnable(bool enable) { }

        public void SetImageFillAmount(float value) { }

        public float GetImageFillAmount() { return 0.0f; }

        public void PlayAnimation(string ani) { }

        public void SetTextColor(Color color) { }

        public void SetMaskWidth(float width) { }

        public void SetValue(Sprite sprite) { }

        public void SetButtonInteractable(bool enable) { }

        public void SetInputFieldChangeEvent(Action<string> onFieldChange) { }

        public void SetToggleChangeEvent(Action<bool> onToggleChange) { }

        public void SetRawImage(Texture2D texture) { }

        public void SetImageColor(Color color) { }

        public void SetDropListItems(List<string> items) { }

        public bool GetToggleValue() { return false; }

        public void SetToggleValue(bool isOn) { }

        public void SetToggleGroupEvent(Action<bool, string> onToggled) { }

        public void SetInputFieldVal(string val) { }

        public string GetInputFieldVal() { return ""; }

        public Color GetTextColor() { return Color.white; }

        public GameObject GetGameObj() { return null; }

        public void SetRectSize(Vector2 size) { }

        public void ScrollLoad(int count, Action<int, GameObject> setFunc) { }

        public void SnapTo(int index) { }

        public void ShowTab(int index) { }

        public void ShowTab(string name) { }
    }
}