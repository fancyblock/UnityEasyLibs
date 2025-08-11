using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace EasyLibs
{
    public class ImageUsageFinder : EditorWindow
    {
        private string searchText = "";


        [MenuItem("EasyLibs/Image Usage Finder")]
        public static void ShowWindow()
        {
            GetWindow<ImageUsageFinder>("Image Usage Finder");
        }

        private void OnGUI()
        {
            GUILayout.Label("Image Usage Finder", EditorStyles.boldLabel);

            GUILayout.BeginHorizontal();
            searchText = EditorGUILayout.TextField(searchText, GUILayout.MinWidth(150));
            if (GUILayout.Button("ËÑË÷", GUILayout.Width(60)))
            {
                Debug.Log($"ËÑË÷ÄÚÈÝ: {searchText}");
                onSearch();
            }

            GUILayout.EndHorizontal();
        }

        
        private void onSearch()
        {
            if(string.IsNullOrEmpty(searchText))
            {
                Debug.LogWarning("ÇëÊäÈëËÑË÷ÄÚÈÝ");
                return;
            }

            var rectTransforms = FindObjectsByType<RectTransform>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            Util.Log($"{rectTransforms.Length} objects in all.");

            foreach (var rectTransform in rectTransforms)
            {
                GameObject go = rectTransform.gameObject;
                
                Image img = go.GetComponent<Image>();

                if(img != null && img.sprite != null && img.sprite.name.Contains(searchText))
                {
                    string path = getGameObjectPath(go);
                    Debug.Log($"ÕÒµ½Æ¥ÅäµÄImage: {path} - {img.sprite.name}", go);
                }

                RawImage rimg = go.GetComponent<RawImage>();

                if(rimg != null && rimg.texture != null && rimg.texture.name.Contains(searchText))
                {
                    string path = getGameObjectPath(go);
                    Debug.Log($"ÕÒµ½Æ¥ÅäµÄRawImage: {path} - {rimg.texture.name}", go);
                }
            }
        }

        private string getGameObjectPath(GameObject obj)
        {
            string path = obj.name;
            Transform current = obj.transform.parent;
            while (current != null)
            {
                path = current.name + "/" + path;
                current = current.parent;
            }
            return path;
        }

    }
}