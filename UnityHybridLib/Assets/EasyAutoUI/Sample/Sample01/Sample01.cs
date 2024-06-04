using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Sample01 : MonoBehaviour
{
    
    class SampleData01 : BaseBindingData
    {
        public string m_strValue;
        public int m_intValue;
        public Sprite m_sprValue;
        public bool m_boolValue;
    }


    [SerializeField] AutoContainer m_ui;

    SampleData01 data;

    // Start is called before the first frame update
    void Start()
    {
        data = new SampleData01();

        data.m_strValue = "Init data";
        data.m_intValue = 117;

        m_ui.Bind<InputFieldBinding>("input01", data, "m_intValue");
        m_ui.Bind<InputFieldBinding>("input02", data, "m_strValue");

        m_ui.Bind<TextBinding>("txt01", data, "m_strValue");

        m_ui.Bind<ImageBinding>("img01", data, "m_sprValue");

        m_ui.Bind<TextBinding>("txt02", data, "m_boolValue");
        m_ui.Bind<ToggleBinding>("toggle01", data, "m_boolValue");

        //m_ui.SetValue("aaa", "asdf");

    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
            data.SetField("m_sprValue", Resources.Load<Sprite>("korea"));
    }
}
