using System;
using System.Reflection;

namespace EasyLibs
{
    [Serializable]
    public class BaseBindingData : IBindingData
    {
        [field: NonSerialized] public event Action<string, object> ON_DATA_CHANGED;

        public object GetField(string name)
        {
            Type t = this.GetType();
            FieldInfo fieldInfo = t.GetField(name);

            return fieldInfo.GetValue(this);
        }

        public void SetField(string name, object val)
        {
            Type t = this.GetType();
            FieldInfo fieldInfo = t.GetField(name);

            fieldInfo.SetValue(this, val);

            ON_DATA_CHANGED.Invoke(name, val);
        }

        public Type GetFieldType(string name)
        {
            Type t = this.GetType();
            FieldInfo fieldInfo = t.GetField(name);

            return fieldInfo.FieldType;
        }
    }
}