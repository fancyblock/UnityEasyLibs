using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyLibs
{
    public class ComponentInject : MonoBehaviour
    {
        [SerializeField] protected List<Component> m_components;
        //TODO 


        void Awake()
        {
            foreach(Component com in m_components)
            {
                if(com != null)
                {
                    //TODO 
                }
                else
                {
                    throw new Exception($"Component is null in {gameObject.name}");
                }
            }
        }
    }
}