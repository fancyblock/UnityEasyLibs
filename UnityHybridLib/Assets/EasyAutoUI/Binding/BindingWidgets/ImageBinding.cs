using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Image))]
public class ImageBinding : BaseBindingWidget
{
    private Image m_image;


    // Start is called before the first frame update
    void Awake()
    {
        init();
    }

    protected override void onDataChange(object val)
    {
        init();
        m_image.sprite = val as Sprite;
    }


    private void init()
    {
        if(m_image == null)
            m_image = GetComponent<Image>();
    }
}
