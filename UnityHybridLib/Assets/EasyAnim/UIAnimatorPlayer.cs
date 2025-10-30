using UnityEngine;
using UnityEngine.UI;

namespace EasyLibs
{
    [RequireComponent(typeof(Image))]
    public class UIAnimatorPlayer : MonoBehaviour
    {
        public Image m_image;
        public AnimClip m_anim;

        public bool autoPlay = true;
        public bool loop = true;
        public Sprite m_defaultSpr;

        private bool m_playing = false;
        private int m_curIndex;
        private float m_timer;


        void Awake()
        {
            if (autoPlay)
                reset();
        }

        public void PlayClip(AnimClip clip = null)
        {
            if (clip != null)
                m_anim = clip;

            reset();
        }

        public void Reset()
        {
            m_curIndex = 0;
            m_timer = 0;

            m_image.sprite = m_anim.m_frames[0];

            m_playing = false;
        }

        void Update()
        {
            if (!m_playing)
                return;

            m_timer += Time.deltaTime;

            if (m_timer >= m_anim.m_interval)
            {
                m_curIndex++;
                m_timer -= m_anim.m_interval;

                if (m_curIndex >= m_anim.m_frames.Count)
                {
                    m_curIndex = 0;

                    if (loop)
                    {
                        m_image.sprite = m_anim.m_frames[m_curIndex];
                    }
                    else
                    {
                        m_image.sprite = m_defaultSpr;
                        m_playing = false;
                    }
                }
                else
                {
                    m_image.sprite = m_anim.m_frames[m_curIndex];
                }
            }
        }


        void reset()
        {
            m_curIndex = 0;
            m_timer = 0;

            m_image.sprite = m_anim.m_frames[0];

            m_playing = true;
        }


        [ContextMenu("TestPlay")]
        void TestPlay()
        {
            PlayClip();
        }
    }
}