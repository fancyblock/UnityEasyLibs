using TMPro;
using UnityEngine;


[RequireComponent(typeof(TextMeshProUGUI))]
public class TMPBinding : BaseBindingWidget
{
    private TextMeshProUGUI m_text;


    // Start is called before the first frame update
    void Awake()
    {
        init();
    }

    protected override void onDataChange(object val)
    {
        init();
        m_text.text = val.ToString();
    }


    private void init()
    {
        if(m_text == null)
            m_text = GetComponent<TextMeshProUGUI>();
    }
}
