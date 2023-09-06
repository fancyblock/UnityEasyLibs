using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace EasyInject
{

    public class BindingData 
    {
        private Type m_key;
        private object m_valueInstance = null;
        private Type m_valueType = null;

        private string m_id;

        private eBindingType m_bindingType;


        public void SetKey(Type t)
        {
            m_key = t;
        }

        public void SetInstValue(object valueInstance)
        {
            m_valueInstance = valueInstance;

            m_bindingType = eBindingType.single;
        }

        public void SetTypeValue(Type t)
        {
            m_valueType = t;
        }

        public BindingData To<T>() where T : class
        {
            m_valueType = typeof(T);

            return this;
        }

        public BindingData WithId(string id)
        {
            m_id = id;

            return this;
        }

        public void AsSingle()
        {
            m_bindingType = eBindingType.single;
        }

        public void AsTransient()
        {
            m_bindingType = eBindingType.transient;
        }


        public bool MatchId(string id)
        {
            if (m_id == id)
                return true;

            return false;
        }

        public bool MatchKey(Type t)
        {
            if(m_key.Name == t.Name)
                return true;

            return false;
        }

        public object GetValue()
        {
            object res = null;

            if (m_bindingType == eBindingType.single)
            {
                if(m_valueInstance == null)
                {
                    m_valueInstance = Activator.CreateInstance(m_valueType);
                }

                res = m_valueInstance;
            }
            else if(m_bindingType == eBindingType.transient)
            {
                res = Activator.CreateInstance(m_valueType);
            }

            return res;
        }
    }


    enum eBindingType
    {
        single,
        transient,
    }

}
