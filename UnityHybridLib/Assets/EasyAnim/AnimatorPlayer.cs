using System.Collections.Generic;
using UnityEngine;

namespace EasyLibs
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class AnimatorPlayer : MonoBehaviour
    {
        public SpriteRenderer m_sprite;
        public AnimSet m_animSet;
        public bool m_stopEmpty = false;

        Dictionary<string, AnimClip> m_clipDic = new Dictionary<string, AnimClip>();

        Sprite m_defaultSpr;
        bool m_playing = false;
        AnimClip m_curClip;
        int m_curIndex;
        float m_timer;
        string m_curClipName;

        private float m_playSpeed = 1.0f;


        void Awake()
        {
            foreach (AnimClip ac in m_animSet.m_clips)
                m_clipDic.Add(ac.m_clipName, ac);

            m_defaultSpr = m_sprite.sprite;
        }

        void Update()
        {
            if (m_playing)
            {
                m_timer += Time.deltaTime;
                float interval = m_curClip.m_interval / m_playSpeed;

                if (m_timer >= interval)
                {
                    m_curIndex++;
                    m_timer -= interval;

                    if (m_curIndex >= m_curClip.m_frames.Count)
                    {
                        if (m_curClip.m_playMode == eAnimPlayMode.loop)
                        {
                            m_curIndex = 0;
                        }
                        else
                        {
                            if (m_stopEmpty)
                                m_sprite.sprite = null;

                            m_playing = false;

                            return;
                        }
                    }

                    m_sprite.sprite = m_curClip.m_frames[m_curIndex];
                }
            }
        }

        public void Play(string clipName, float speed = 1.0f)
        {
            m_playSpeed = speed;
            AnimClip clip = null;

            if (m_clipDic.TryGetValue(clipName, out clip))
            {
                m_curClip = clip;
                m_curIndex = 0;
                m_timer = 0;

                m_sprite.sprite = m_curClip.m_frames[0];

                m_playing = true;
                m_curClipName = clipName;
            }
            else
            {
                Util.Log("Can not found clip. " + clipName);
            }
        }

        public void Stop()
        {
            m_playing = false;
            m_sprite.sprite = m_defaultSpr;
        }

        public bool PLAYING { get { return m_playing; } }

        public float PLAY_SPEED { get { return m_playSpeed; } }

        public string CLIP_NAME { get { return m_curClipName; } }
    }
}