using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace EasyInject
{
    public class ComponentInject : MonoBehaviour
    {
        [SerializeField] protected List<Component> m_components;
        //TODO 


        // Start is called before the first frame update
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
