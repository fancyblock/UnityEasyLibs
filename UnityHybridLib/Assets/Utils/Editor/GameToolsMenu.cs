using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;


public class GameToolsMenu
{
    [MenuItem("MyGame/Open Game")]
    static public void OpenGame()
    {
        Debug.Log("Open game scene.");

        EditorSceneManager.OpenScene("Assets/Game/Scene/Game.unity");
    }

    [MenuItem("MyGame/Clear saver")]
    static public void CleanSaver()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        Debug.Log("Clear PlayerPrefs done.");
    }

    [MenuItem("MyGame/Export Settings")]
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

    [MenuItem("MyGame/Convert to UTF-8")]
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

}