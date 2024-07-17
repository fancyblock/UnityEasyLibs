using UnityEngine;
using UnityEngine.UI;

namespace EasyLibs
{
    [RequireComponent(typeof(Image))]
    public class ImageBinding : BaseBindingWidget
    {
        private Image m_image;


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
            if (m_image == null)
                m_image = GetComponent<Image>();
        }
    }
}