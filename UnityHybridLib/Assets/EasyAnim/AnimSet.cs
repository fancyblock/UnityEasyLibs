using System.Collections.Generic;
using UnityEngine;

namespace EasyLibs
{
    [CreateAssetMenu(menuName = "EasyLibs/Create AnimSet")]
    public class AnimSet : ScriptableObject
    {
        public List<AnimClip> m_clips;
    }
}