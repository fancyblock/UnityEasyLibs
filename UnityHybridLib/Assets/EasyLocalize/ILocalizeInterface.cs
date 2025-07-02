using System.Collections.Generic;

namespace EasyLibs
{
    public interface ILocalizeWidget
    {
        void OnUpdateLanguage();
    }

    public interface ILanguageTextParser
    {
        void Parse(string rawText, Dictionary<string, List<string>> languageTextDic);
    }
}