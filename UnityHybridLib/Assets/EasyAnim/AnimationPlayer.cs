using UnityEngine;

namespace EasyLibs
{
    public class AnimationPlayer : MonoBehaviour
    {
        public SpriteRenderer sprite;
        public AnimClip animClip;

        private int m_curIndex;
        private float m_timer;


        void Start()
        {
            m_curIndex = 0;
            m_timer = 0;

            sprite.sprite = animClip.m_frames[0];
        }

        void Update()
        {
            m_timer += Time.deltaTime;

            if (m_timer >= animClip.m_interval)
            {
                m_curIndex++;
                m_timer -= animClip.m_interval;

                if (m_curIndex >= animClip.m_frames.Count)
                {
                    m_curIndex = 0;
                }

                sprite.sprite = animClip.m_frames[m_curIndex];
            }
        }
    }
}