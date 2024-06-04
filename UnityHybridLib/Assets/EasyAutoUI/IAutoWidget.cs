using System;
using System.Collections.Generic;
using UnityEngine;

public interface IAutoWidget 
{
    bool IsVirtual();

    void SetEnable(bool enable);

    GameObject GetGameObj();

    T Component<T>();

    void SetValue(string text);

    void SetValue(bool toggle);

    void SetValue(float progress);

    void SetValue(Sprite sprite);

    string GetValue();

    void SetButtonEvent(Action evt);

    void SetButtonEvent(Action<string> evt);

    void SetTriggerEvent(Action evt);

    void SetButtonEnable(bool enable);

    void SetImageFillAmount(float value);

    void SetTextColor(Color color);

    Color GetTextColor();

    void SetRawImage(Texture2D texture);

    void SetImageColor(Color color);

    float GetImageFillAmount();

    void PlayAnimation(string ani);

    void SetMaskWidth(float width);

    void SetInputFieldVal(string val);

    string GetInputFieldVal();

    void SetInputFieldChangeEvent(Action<string> onFieldChange);

    void SetToggleChangeEvent(Action<bool> onToggleChange);

    bool GetToggleValue();

    void SetToggleValue(bool isOn);

    void SetToggleGroupEvent(Action<bool, string> onToggled);

    void SetDropListItems(List<string> items);

    void SetRectSize(Vector2 size);

    void ScrollLoad(int count, Action<int, GameObject> setFunc);

    void SnapTo(int index);

    void ShowTab(int index);

    void ShowTab(string name);

}
