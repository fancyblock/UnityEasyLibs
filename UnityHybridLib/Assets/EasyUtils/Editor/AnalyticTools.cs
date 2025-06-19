using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace EasyLibs
{
    public class AnalyticTools : Editor
    {
        [MenuItem("GameObject/EasyLibs/FindingAllImages")]
        private static void FindingAllImages()
        {
            GameObject selectedObject = Selection.activeGameObject;

            if (selectedObject != null)
            {
                HashSet<string> usedImagePath = new HashSet<string>();

                Image[] images = selectedObject.GetComponentsInChildren<Image>(true);

                foreach (var image in images)
                {
                    if (image.sprite != null)
                    {
                        Sprite spr = image.sprite;

                        string path = AssetDatabase.GetAssetPath(spr.GetInstanceID());
                        usedImagePath.Add(path);
                    }
                }

                RawImage[] rawImages = selectedObject.GetComponentsInChildren<RawImage>(true);

                foreach (var rawImage in rawImages)
                {
                    if (rawImage.texture != null)
                    {
                        Texture tex = rawImage.texture;
                        string path = AssetDatabase.GetAssetPath(tex.GetInstanceID());
                        usedImagePath.Add(path);
                    }
                }

                Debug.Log($"All used image count: {usedImagePath.Count}");

                foreach (var i in usedImagePath)
                {
                    Debug.Log(i);
                }
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "No Object selected ! ", "OK");
            }
        }
    }
}