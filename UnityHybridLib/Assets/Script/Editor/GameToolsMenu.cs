using System.Collections.Generic;
using System.IO;
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

}