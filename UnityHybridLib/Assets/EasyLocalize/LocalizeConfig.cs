using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace EasyLibs
{
    [CreateAssetMenu(fileName = "LocalizeConfig", menuName = "EasyLibs/Localize/LocalizeConfig", order = 1)]
    public class LocalizeConfig : ScriptableObject
    {
        public TextAsset languageFile;
        public bool csvFormat = true;   // if true, use csv format, otherwise use json format

        public List<string> Languages = new List<string>();

        public List<STextProperty> TypeConfigs = new List<STextProperty>();
        public List<SLocalizeImage> LocalizeImages = new List<SLocalizeImage>();
    }


    [Serializable]
    public struct STextProperty
    {
        public int language;
        public string type;

        public TMP_FontAsset font;

        public float fontSizeRatio;
        public float fontSizeMinRatio;
        public float fontSizeMaxRatio;
    }

    [Serializable]
    public struct SLocalizeImage
    {
        public string key;
        public List<Sprite> sprites; // sort by language index
    }
}
