using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace EasyLibs
{
    public class GameToolsMenu
    {
        [MenuItem("EasyLibs/Open Game")]
        static public void OpenGame()
        {
            Debug.Log("Open game scene.");

            EditorSceneManager.OpenScene("Assets/Game/Scene/Game.unity");
        }

        [MenuItem("EasyLibs/Clear saver")]
        static public void CleanSaver()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();

            Debug.Log("Clear PlayerPrefs done.");
        }

        [MenuItem("EasyLibs/Export Settings")]
        static public void OnExportSettings()
        {
            string excelPath = $"{Application.dataPath}/GameSettings~/";
            DirectoryInfo root = new DirectoryInfo(excelPath);
            FileInfo[] files = root.GetFiles();

            foreach (FileInfo file in files)
            {
                string name = file.Name;

                if (name.StartsWith("~"))
                    continue;

                SettingMgr.Instance.ExportTable(name.Substring(0, name.Length - 5));        // cut ".xlsx"
            }

            AssetDatabase.Refresh();
        }

        [MenuItem("EasyLibs/Convert to UTF-8")]
        static public void OnConvertSource2Utf8()
        {
            string folderPath = "Assets/Game/Script";
            string[] guids = AssetDatabase.FindAssets("t:script", new string[] { folderPath });

            Debug.Log(guids.Length);

            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);

                string text = File.ReadAllText(path, Encoding.Default);
                File.WriteAllText(path, text, Encoding.UTF8);
            }

            Debug.Log("convert done.");
        }

        [MenuItem("EasyLibs/Export Source Code")]
        static public void ExportAllSourceCode()
        {
            Debug.Log("Export all the source code.");

            string[] fileList = AssetDatabase.FindAssets("t:Object", new string[] { "Assets/Script" });

            int count = 0;
            StringBuilder sb = new StringBuilder();

            foreach (string file in fileList)
            {
                string path = AssetDatabase.GUIDToAssetPath(file);

                if (path.EndsWith(".cs"))
                {
                    string code = File.ReadAllText(path);

                    sb.Append(path);
                    sb.Append("\n\n");
                    sb.Append(code);
                    sb.Append("\n\n\n\n\n");

                    count++;
                }
            }

            Debug.Log(count + " script files.");

            // write the out 
            File.WriteAllText("code.txt", sb.ToString());
        }
    }
}