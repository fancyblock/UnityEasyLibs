using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EasyLibs
{
    [RequireComponent(typeof(Toggle))]
    public class EasyToggleGo : MonoBehaviour
    {
        [SerializeField] private List<GameObject> m_targets;

        void Awake()
        {
            GetComponent<Toggle>().onValueChanged.AddListener(onValChange);
        }

        void Start()
        {
            Toggle t = GetComponent<Toggle>();

            m_targets.ForEach(i => i.SetActive(t.isOn));    
        }


        private void onValChange(bool toggle)
        {
            m_targets.ForEach(i => i.SetActive(toggle));
        }
    }
}
