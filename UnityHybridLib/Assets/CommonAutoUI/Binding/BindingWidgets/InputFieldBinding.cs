using System;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(InputField))]
public class InputFieldBinding : BaseBindingWidget
{
    private InputField m_inputField;


    void Awake()
    {
        m_inputField = GetComponent<InputField>();
        m_inputField.onValueChanged.AddListener(onInputfieldValueChanged);
    }


    protected override void onDataChange(object val) 
    {
        string strVal = val.ToString();

        if(m_inputField.text != strVal)
            m_inputField.text = strVal;
    }


    private void onInputfieldValueChanged(string newValue)
    {
        object bindingData = BINDING_DATA;

        string val = bindingData != null ? BINDING_DATA.ToString() : "";

        if (val != newValue)
        {
            object convertedValue = null;

            try
            {
                convertedValue = Convert.ChangeType(newValue, m_fieldType);

                object oldConvertedValue = Convert.ChangeType(val, m_fieldType);    // 用于修复输入数字时0和0.时，导致的每次输入到0.又会被重置为0的情况

                if(!convertedValue.Equals(oldConvertedValue))
                    m_bindingObject.SetField(m_bindingField, convertedValue);
            }
            catch(Exception e)
            {
                Debug.Log($"Value {newValue} can not be convert to {m_fieldType.Name} for {m_bindingObject.GetType().Name}.{m_bindingField}");
            }
        }
    }
}
