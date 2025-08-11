using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace EasyLibs
{
    public class SceneTools 
    {
        [MenuItem("GameObject/SceneTools/Show Text Infos")]
        static public void OnShowTextSize()
        {
            GameObject go = Selection.activeGameObject;

            if (go != null)
            {
                TextMeshProUGUI[] textList = go.GetComponentsInChildren<TextMeshProUGUI>(true);

                Debug.Log($"{textList.Length} text widgets in all.");

                Dictionary<TMP_FontAsset, int> fonts = new Dictionary<TMP_FontAsset, int>();

                foreach (var text in textList)
                {
                    if (text.font != null)
                    {
                        if (fonts.ContainsKey(text.font))
                            fonts[text.font]++;
                        else
                            fonts.Add(text.font, 1);
                    }
                }

                Debug.Log($"Total {fonts.Count} fonts used in all.");
                foreach (var font in fonts.Keys)
                {
                    if (font != null)
                        Debug.Log($"Font: {font.name},  {fonts[font]} used.");
                }
            }
            else
            {
                EditorUtility.DisplayDialog("Warnning", "Select gameObject ?", "Yes");
            }
        }

        [MenuItem("GameObject/SceneTools/Show GameObject Count")]
        static public void OnShowGameObjectCount()
        {
            GameObject go = Selection.activeGameObject;

            if (go != null)
            {
                var ts = go.GetComponentsInChildren<Transform>(true);

                Debug.Log($"GameObject {go.name} has {ts.Length} children in all.");
            }
            else
            {
                EditorUtility.DisplayDialog("Warnning", "Select gameObject ?", "Yes");
            }
        }

        [MenuItem("GameObject/SceneTools/Show InputField Info")]
        static public void OnShowInputFieldInfo()
        {
            GameObject go = Selection.activeGameObject;

            if (go != null)
            {
                TMP_InputField[] inputs = go.GetComponentsInChildren<TMP_InputField>(true);

                Debug.Log($"{inputs.Length} inputfiled widgets in all.");

                foreach (var i in inputs)
                {
                    Debug.Log($"{i.gameObject.name}", i.gameObject);
                }
            }
        }
    }
}