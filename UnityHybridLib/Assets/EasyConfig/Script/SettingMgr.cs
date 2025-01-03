using ExcelDataReader;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace EasyLibs
{
    public class SettingMgr : Singleton<SettingMgr>
    {
        private Dictionary<string, object> m_cacheSetting = new Dictionary<string, object>();
        private Dictionary<string, BaseCfg> m_cacheInfo = new Dictionary<string, BaseCfg>();


        public List<T> LoadData<T>(string name, bool noCache = false)
        {
            if (!noCache && m_cacheSetting.ContainsKey(name))
                return m_cacheSetting[name] as List<T>;

            string dataJson = loadJson(name);

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            List<T> result = JsonConvert.DeserializeObject<List<T>>(dataJson, settings);

            if (!noCache)
                m_cacheSetting[name] = result;

            return result;
        }

        public T GetInfo<T>(string id) where T : BaseCfg
        {
            string name = typeof(T).Name;
            string key = $"{name}-{id}";

            if (m_cacheInfo.ContainsKey(key))
                return m_cacheInfo[key] as T;

            List<T> list = LoadData<T>(name);

            foreach (T info in list)
            {
                if (info.id == id)
                {
                    m_cacheInfo[key] = info;
                    return info;
                }
            }

            return null;
        }

        public T GetInfo<T>(string name, string id) where T : BaseCfg
        {
            string key = $"{name}-{id}";

            if (m_cacheInfo.ContainsKey(key))
                return m_cacheInfo[key] as T;

            List<T> list = LoadData<T>(name);

            foreach (T info in list)
            {
                if (info.id == id)
                {
                    m_cacheInfo[key] = info;
                    return info;
                }
            }

            return null;
        }

        public T LoadSO<T>(string name) where T : ScriptableObject
        {
            string path = $"Settings/SO/{name}";

            return Resources.Load(path) as T;
        }


#if UNITY_EDITOR

        public void ExportTable(string tableName)
        {
            string excelPath = $"{Application.dataPath}/GameSettings~/{tableName}.xlsx";
            string jsonPath = $"{Application.dataPath}/Game/Resources/Settings/{tableName}.txt";

            string jsonStr = loadExcelAsJson(excelPath);
            File.WriteAllText(jsonPath, jsonStr);
        }

#endif


        private string loadJson(string name)
        {
            string filePath = null;
            string dataJson = null;

#if UNITY_EDITOR
            filePath = $"{Application.dataPath}/GameSettings~/{name}.xlsx";

            if (File.Exists(filePath))
            {
                dataJson = loadExcelAsJson(filePath);
            }
            else
            {
                filePath = $"Settings/{name}";
                dataJson = loadTextAsJson(filePath);
            }
#else
        filePath = $"Settings/{name}";
        dataJson = loadTextAsJson(filePath);
#endif

            return dataJson;
        }

        private string loadTextAsJson(string name)
        {
            TextAsset ta = Resources.Load<TextAsset>(name);

            return ta.text;
        }

        private string loadExcelAsJson(string fullName)
        {
            if (!File.Exists(fullName))
                throw new Exception($"找不到配置表 {fullName}");

            string dataJson = null;

            using (FileStream stream = File.Open(fullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                // Auto-detect format, supports:
                //  - Binary Excel files (2.0-2003 format; *.xls)
                //  - OpenXml Excel files (2007 format; *.xlsx, *.xlsb)
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    List<string> fields = new List<string>();
                    Dictionary<string, int> fieldIndex = new Dictionary<string, int>();

                    List<Dictionary<string, object>> itemList = new List<Dictionary<string, object>>();

                    Dictionary<int, string> datas = new Dictionary<int, string>();
                    Dictionary<int, string> types = new Dictionary<int, string>();

                    for (int rowIndex = 0; rowIndex < reader.RowCount; rowIndex++)
                    {
                        reader.Read();

                        if (rowIndex == 0)
                            continue;

                        // 字段名行
                        if (rowIndex == 1)
                        {
                            for (int colIndex = 0; colIndex < reader.FieldCount; colIndex++)
                            {
                                string fieldName = reader.GetString(colIndex);
                                if (string.IsNullOrEmpty(fieldName))
                                    continue;

                                datas.Add(colIndex, fieldName);
                            }
                        }
                        // 实际数据行
                        else
                        {
                            if (reader.GetString(0) != null && reader.GetString(0).StartsWith("#"))
                                continue;

                            Dictionary<string, object> item = new Dictionary<string, object>();

                            foreach (int index in datas.Keys)
                            {
                                try
                                {
                                    if (reader.GetValue(index) == null)
                                    {
                                        item.Add(datas[index], "");
                                        continue;
                                    }

                                    Type type = reader.GetFieldType(index);

                                    if (type == typeof(string))
                                    {
                                        item.Add(datas[index], reader.GetString(index));
                                    }
                                    else if (type == typeof(double))
                                    {
                                        // 判断是否能转为int
                                        double number = reader.GetDouble(index);
                                        if (Util.canDouble2Int(number))
                                            item.Add(datas[index], (int)number);
                                        else
                                            item.Add(datas[index], number);
                                    }
                                    else if (type == typeof(bool))
                                    {
                                        item.Add(datas[index], reader.GetBoolean(index));
                                    }
                                    else
                                    {
                                        Debug.LogError($"表格{fullName}含有不支持的字段类型: {type.Name} 在{index},{rowIndex + 1}");
                                    }
                                }
                                catch (Exception e)
                                {
                                    Debug.LogError($"表格{fullName}含有不支持的字段." + e);
                                }
                            }

                            itemList.Add(item);
                        }
                    }

                    dataJson = Newtonsoft.Json.JsonConvert.SerializeObject(itemList);
                }
            }

            return dataJson;
        }
    }
}