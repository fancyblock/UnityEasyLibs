using UnityEngine.UI;


public class ToggleBinding : BaseBindingWidget
{
    private Toggle m_toggle;


    void Awake()
    {
        m_toggle = GetComponent<Toggle>();
        m_toggle.onValueChanged.AddListener(onToggleValueChanged);
    }


    protected override void onDataChange(object val)
    {
        bool boolVal = (bool)val;

        if (m_toggle.isOn != boolVal)
            m_toggle.isOn = boolVal;
    }

    private void onToggleValueChanged(bool newVal)
    {
        bool boolVal = (bool)BINDING_DATA;

        if(boolVal != newVal)
        {
            m_bindingObject.SetField(m_bindingField, newVal);
        }
    }
}
