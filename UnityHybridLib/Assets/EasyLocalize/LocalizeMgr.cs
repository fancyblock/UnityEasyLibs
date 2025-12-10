using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Assertions;

namespace EasyLibs
{
    public class LocalizeMgr : Singleton<LocalizeMgr>
    {
        private LocalizeConfig m_config = null;
        private int m_curLanguageIndex;

        private Dictionary<string, List<string>> m_languageTextDic = new Dictionary<string, List<string>>();
        private HashSet<ILocalizeWidget> m_localizeWidgets = new HashSet<ILocalizeWidget>();


        public LocalizeMgr()
        {
            // load config
            m_config = Resources.Load<LocalizeConfig>("Localize/LocalizeConfig");
        }

        public void Init(int index, ILanguageTextParser parser)
        {
            // load language info
            string rawLanguageText = m_config.languageFile.text;

            parser.Parse(rawLanguageText, m_languageTextDic);

            m_curLanguageIndex = index;

            Util.Log($"LocalizeMgr init done.", Util.eColor.green);
        }

        public void AddWidget(ILocalizeWidget widget)
        {
            m_localizeWidgets.Add(widget);
        }

        public void RemoveWidget(ILocalizeWidget widget)
        {
            m_localizeWidgets.Remove(widget);
        }

        public List<string> LANGUAGE_LIST
        {
            get { return m_config.Languages; }
        }

        public int CURRENT_LANGUAGE_INDEX
        {
            get { return m_curLanguageIndex; }
        }

        public string CURRENT_LANGUAGE
        {
            get { return m_config.Languages[m_curLanguageIndex]; }
        }

        public void ChangeLanguage(string lang)
        {
            int index = m_config.Languages.IndexOf(lang);

            ChangeLanguage(index);
        }

        public void ChangeLanguage(int index)
        {
            if (index == m_curLanguageIndex)
                return;

            m_curLanguageIndex = index;

            foreach (var i in m_localizeWidgets)
                i.OnUpdateLanguage();
        }

        public string GetText(string key)
        {
            string txt = "";

            try
            {
                if (key.Contains('[') && key.Contains(']'))
                {
                    var matches = Regex.Matches(key, "\\[([^\\]]+)\\]");

                    Assert.IsTrue(matches.Count == 1, $"Localize key param error: {key}");

                    var match = matches[0];

                    string paramStr = match.Groups[1].Value;
                    string realKey = key.Replace(match.Value, "");

                    txt = m_languageTextDic[realKey][m_curLanguageIndex];
                    txt = System.Text.RegularExpressions.Regex.Unescape(txt);
                    txt = string.Format(txt, paramStr);
                }
                else
                {
                    txt = m_languageTextDic[key][m_curLanguageIndex];
                    txt = System.Text.RegularExpressions.Regex.Unescape(txt);
                }
            }
            catch (System.Exception)
            {
                Debug.LogError($"Localize text for key '{key}' not found for language index {m_curLanguageIndex}.");
            }

            return txt;
        }

        public string GetText(string key, int languageIndex)
        {
            return m_languageTextDic[key][languageIndex];
        }

        public SLocalizeImage GetLocalizeImage(string key)
        {
            SLocalizeImage imageConfig = default;

            foreach (var i in m_config.LocalizeImages)
            {
                if (i.key == key)
                {
                    imageConfig = i;
                    break;
                }
            }
            if (imageConfig.sprites.Count <= m_curLanguageIndex)
            {
                Debug.LogError($"Localize image for key '{key}' not found for language index {m_curLanguageIndex}.");
            }

            return imageConfig;
        }

        public STextProperty GetTextProperty(string type)
        {
            STextProperty sp = default;

            foreach (var i in m_config.TypeConfigs)
            {
                if (i.type == type && i.language == m_curLanguageIndex)
                {
                    sp = i;
                    break;
                }
            }

            return sp;
        }
    }
}