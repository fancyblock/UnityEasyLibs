using System;
using UnityEngine;


public class BaseBindingWidget : MonoBehaviour, IBindingWidget
{
    protected IBindingData m_bindingObject;
    protected string m_bindingField;
    protected Type m_fieldType;


    public void Bind(IBindingData bindingObject, string bindingField) 
    {
        Unbind();

        m_bindingObject = bindingObject;
        m_bindingField = bindingField;

        // check data type
        m_fieldType = bindingObject.GetFieldType(m_bindingField);

        onDataChanged(m_bindingField, m_bindingObject.GetField(m_bindingField));

        m_bindingObject.ON_DATA_CHANGED += onDataChanged;

        onBind();
    }

    public void Unbind()
    {
        if (m_bindingObject == null)
            return;

        m_bindingObject.ON_DATA_CHANGED -= onDataChanged;

        onUnbind();

        m_bindingObject = null;
        m_bindingField = null;
    }


    protected object BINDING_DATA
    {
        get
        {
            return m_bindingObject.GetField(m_bindingField);
        }
    }


    protected virtual void onBind() { }
    protected virtual void onUnbind() { }
    protected virtual void onDataChange(object val) { }

    
    private void onDataChanged(string field, object val) 
    {
        if (val == null)
            return;

        if (field != m_bindingField)
            return;

        onDataChange(val);
    }
}
