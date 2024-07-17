using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyLibs
{
    [CreateAssetMenu(menuName = "EasyLibs/Create AnimClip")]
    public class AnimClip : ScriptableObject
    {
        public string m_clipName;
        public float m_interval;
        public eAnimPlayMode m_playMode;

        public List<Sprite> m_frames;
    }


    [Serializable]
    public enum eAnimPlayMode
    {
        once,
        loop,
    }
}