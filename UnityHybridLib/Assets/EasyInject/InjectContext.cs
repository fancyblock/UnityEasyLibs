using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;


namespace EasyInject
{

    public class InjectContext
    {
        private static InjectContext m_instance = null;
        private static readonly object m_lock = new object();

        //sigleton
        public static InjectContext Instance
        {
            get
            {
                lock(m_lock)
                {
                    if(m_instance == null)
                        m_instance = new InjectContext();
                }

                return m_instance;
            }
        }


        private List<BindingData> m_bindingDatas = new List<BindingData>();


        public InjectContext() 
        {
        }

        public BindingData Bind<T>(T obj)
        {
            BindingData data = new BindingData();

            data.SetKey(typeof(T));
            data.SetInstValue(obj);

            m_bindingDatas.Add(data);

            return data;
        }

        public BindingData Bind<T>()
        {
            BindingData data = new BindingData();

            data.SetKey(typeof(T));

            m_bindingDatas.Add(data);

            return data;
        }

        public BindingData Bind(Type t, object obj)
        {
            BindingData data = new BindingData();

            data.SetKey(t);
            data.SetInstValue(obj);

            m_bindingDatas.Add(data);

            return data;
        }

        public bool UnbindingData<T>(T obj)
        {
            //TODO 

            return true;
        }

        public bool UnbindingData(Type t, object obj)
        {
            //TODO 

            return true;
        }

        public void Inject(object obj)
        {
            FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            foreach(FieldInfo field in fields)
            {
                Attribute[] attributes = Attribute.GetCustomAttributes(field, false);

                bool isInjectField = false;
                Inject inject = null;

                foreach (Attribute attribute in attributes)
                {
                    if(attribute is Inject)
                    {
                        isInjectField = true;
                        inject = attribute as Inject;

                        break;
                    }
                }

                if(isInjectField)
                {
                    BindingData bd = getBindingData(field.FieldType, inject);

                    if(bd != null)
                    {
                        field.SetValue(obj, bd.GetValue());
                    }
                }
            }
        }


        private BindingData getBindingData(Type t, Inject inject)
        {
            BindingData bd = null;

            foreach(BindingData b in m_bindingDatas)
            {
                if(b.MatchKey(t) && b.MatchId(inject.Id))
                {
                    bd = b;
                    break;
                }
            }

            return bd;
        }

    }

}
