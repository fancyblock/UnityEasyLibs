using System;


public interface IBindingData 
{
    event Action<string,object> ON_DATA_CHANGED;

    void SetField(string name, object val);

    object GetField(string name);

    Type GetFieldType(string name);

}
