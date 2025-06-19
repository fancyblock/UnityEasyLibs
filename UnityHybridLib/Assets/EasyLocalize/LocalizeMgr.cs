using System.Collections.Generic;
using UnityEngine;
using Newtonsoft;

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

            // load language info
            string rawLanguageText = m_config.languageFile.text;
            parseLanguageText(rawLanguageText);

            Util.Log($"LocalizeMgr init done.");
        }

        public void Init(int index)
        {
            m_curLanguageIndex = index;
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
            return m_languageTextDic[key][m_curLanguageIndex];
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


        private void parseLanguageText(string rawText)
        {
            if (m_config.csvFormat)
                parseCsvLanguageText(rawText);
            else
                parseJsonLanguageText(rawText);
        }

        private void parseJsonLanguageText(string rawJsonText)
        {
            //try
            //{
            //    var languageInfoList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LanguageInfo>>(rawJsonText);  //TODO 
            //    foreach (var info in languageInfoList)
            //    {
            //        List<string> textList = new List<string>
            //        {
            //            info.chs,
            //            info.cht,
            //            info.eng
            //        };
            //        m_languageTextDic[info.key] = textList;
            //    }
            //}
            //catch (System.Exception e)
            //{
            //    Debug.LogError($"Failed to parse JSON language text: {e.Message}");
            //}
        }

        private void parseCsvLanguageText(string rawCSVText)
        {
            string[] lines = rawCSVText.Split(new char[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length < 2)
            {
                Debug.LogError("Invalid language file format.");
                return;
            }

            // First line is header
            string[] headers = lines[0].Split(',');
            for (int i = 1; i < lines.Length; i++)
            {
                string[] values = lines[i].Split(',');
                if (values.Length != headers.Length)
                {
                    Debug.LogError($"Line {i + 1} has an incorrect number of values.");
                    continue;
                }
                Dictionary<string, string> textDict = new Dictionary<string, string>();
                for (int j = 0; j < headers.Length; j++)
                {
                    textDict[headers[j].Trim()] = values[j].Trim();
                }

                List<string> textList = new List<string>();
                for (int k = 0; k < m_config.Languages.Count; k++)
                {
                    textList.Add(textDict[m_config.Languages[k]]);
                }

                m_languageTextDic[values[0].Trim()] = textList; // Assuming the first column is the language code
            }
        }
    }
}