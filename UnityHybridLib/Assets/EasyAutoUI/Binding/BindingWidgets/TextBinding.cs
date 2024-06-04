using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Text))]
public class TextBinding : BaseBindingWidget
{
    private Text m_text;


    // Start is called before the first frame update
    void Awake()
    {
        m_text = GetComponent<Text>();
    }

    protected override void onDataChange(object val) 
    {
        m_text.text = val.ToString();
    }
}
