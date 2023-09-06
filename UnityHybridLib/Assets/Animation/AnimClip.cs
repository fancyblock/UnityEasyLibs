using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[CreateAssetMenu(menuName = "MyGame/Create AnimClip")]
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
