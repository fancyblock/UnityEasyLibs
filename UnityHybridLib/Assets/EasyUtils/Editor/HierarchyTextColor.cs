using UnityEditor;
using UnityEngine;

namespace EasyLibs
{
    [InitializeOnLoad]
    public static class HierarchyTextColor
    {
        static HierarchyTextColor()
        {
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
        }

        private static void OnHierarchyGUI(int instanceID, Rect selectionRect)
        {
            GameObject obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

            if (obj != null && obj.name.Contains("Community")) // 根据名字设置条件
            {
                //Rect rect = new Rect(selectionRect.xMax - 20, selectionRect.y, 16, 16);
                //GUI.Label(rect, EditorGUIUtility.IconContent("d_Favorite"));

/*
                EditorGUI.LabelField(selectionRect, obj.name, new GUIStyle()
                {                
                    normal = new GUIStyleState() { textColor = Color.green },
                    fontStyle = FontStyle.Bold
                });
*/
            }
        }
    }
}